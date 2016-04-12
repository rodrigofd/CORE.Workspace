using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraPrinting.Native;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;
using Microsoft.Exchange.WebServices.Data;

namespace FDIT.Core.EWS.Controllers
{
  public class SincronizarContactosController : ViewController
  {
    private IContainer components;
    private SimpleAction sincronizarContactosExchangeAction;

    public SincronizarContactosController( )
    {
      InitializeComponent( );

      RegisterActions( components );
    }

    private void InitializeComponent( )
    {
      components = new Container( );
      sincronizarContactosExchangeAction = new SimpleAction( components );
      // 
      // recuperarAutorizacionAction
      // 
      sincronizarContactosExchangeAction.Caption = "Sincronizar con Exchange";
      sincronizarContactosExchangeAction.ConfirmationMessage = null;
      sincronizarContactosExchangeAction.Id = "sincronizarContactosExchangeAction";
      sincronizarContactosExchangeAction.ImageName = "ms_exchange";
      sincronizarContactosExchangeAction.Shortcut = null;
      sincronizarContactosExchangeAction.Tag = null;
      sincronizarContactosExchangeAction.TargetObjectsCriteria = null;
      sincronizarContactosExchangeAction.TargetViewId = null;
      sincronizarContactosExchangeAction.ToolTip = null;
      sincronizarContactosExchangeAction.TypeOfView = null;
      sincronizarContactosExchangeAction.Execute += SincronizarContactosActionExecute;
      // 
      // AutorizarComprobanteVentaController
      // 
      TargetObjectType = typeof( Persona );
      TargetViewNesting = Nesting.Root;
      TargetViewType = ViewType.ListView;
    }

    private void SincronizarContactosActionExecute( object sender, SimpleActionExecuteEventArgs e )
    {
      var config = Identificadores.GetInstance( ObjectSpace );

      if( config.CuentaSincContactos == null )
      {
        throw new UserFriendlyException( "No se configuró la cuenta de Exchange a utilizar para sincronizar contactos." );
      }
      if( config.IdentificacionTipoEmail == null )
      {
        throw new UserFriendlyException( "No se configuró el tipo de identif. para emails, para sincronizar contactos." );
      }
      if( config.IdentificacionTipoTelOtro == null )
      {
        throw new UserFriendlyException( "No se configuró el tipo de identif. para 'otros teléfonos', para sincronizar contactos." );
      }

      var service = config.CuentaSincContactos.ExchangeService;

      var objectSpace = Application.CreateObjectSpace( );

      var paresDirecciones = new List< ParPersonaDireccionDireccion >( );

      // Get the number of items in the Contacts folder. To keep the response smaller, request only the TotalCount property.
      var contactsfolder = ContactsFolder.Bind( service, WellKnownFolderName.Contacts );

      // Instantiate the item view with the number of items to retrieve from the Contacts folder.
      FindItemsResults< Item > findResults;

      var view = new ItemView( EWSModule.NumContactosPorLote, 0, OffsetBasePoint.Beginning );

      var propSet = new PropertySet( BasePropertySet.FirstClassProperties,
                                          ContactSchema.HasPicture,
                                          ContactSchema.Birthday,
                                          ContactSchema.Notes,
                                          ContactSchema.WeddingAnniversary,
                                          ContactSchema.PhysicalAddresses,
                                          ContactSchema.ImAddresses,
                                          ContactSchema.PhoneNumbers,
                                          ContactSchema.EmailAddresses,
                                          ContactSchema.CompleteName,
                                          ContactSchema.Attachments );

      do
      {
        // Retrieve the items in the Contacts folder that have the properties you've selected.
        findResults = contactsfolder.FindItems( view );
        service.LoadPropertiesForItems( findResults, propSet );

        processItems( findResults, objectSpace, config, paresDirecciones );

        if( findResults.NextPageOffset.HasValue ) view.Offset = findResults.NextPageOffset.Value;
      } while( findResults.MoreAvailable );

      
      objectSpace.CommitChanges( );
      objectSpace.Dispose(  );

      if( !paresDirecciones.IsEmpty( ) )
      {
        var auxObjectSpace = Application.CreateObjectSpace( );
        foreach( var par in paresDirecciones )
        {
          var persDir = auxObjectSpace.GetObjectByKey< PersonaDireccion >( par.PersonaDireccion.Oid );
          persDir.Direccion =
            auxObjectSpace.GetObjectByKey<Direccion>( par.Direccion.Oid );

          persDir.Save( );
        }
        auxObjectSpace.CommitChanges( );
      }

      ObjectSpace.Refresh( );
    }

    private void processItems( IEnumerable< Item > findResults, IObjectSpace objectSpace, Identificadores config, List< ParPersonaDireccionDireccion > paresDirecciones )
    {
      foreach( var item in findResults )
      {
        if( !( item is Contact ) ) continue;

        var contact = item as Contact;

        var persona = objectSpace.FindObject<Persona>( new BinaryOperator( EWSModule.PropertyCodigoEWS, item.Id.UniqueId ) ) ?? objectSpace.CreateObject<Persona>( );

        var modifEWS = ( DateTime? ) persona.GetMemberValue( EWSModule.PropertyModifFechaEWS );
        if( modifEWS.HasValue && modifEWS.Value == item.LastModifiedTime ) continue;

        persona.SetMemberValue( EWSModule.PropertyCodigoEWS, item.Id.UniqueId );
        persona.SetMemberValue( EWSModule.PropertyModifFechaEWS, item.LastModifiedTime );

        persona.Grupo = CoreAppLogonParameters.Instance.GrupoActual( persona.Session );

        if( persona.Oid == -1 ) //Persona nueva
        {
          persona.Tipo = TipoPersona.Juridica;

          //si tiene asignado un apellido, o un telefono celular, inducimos que probablemente es una persona fisica
          if( !string.IsNullOrEmpty( contact.CompleteName.Surname ) || contact.PhoneNumbers.Contains( PhoneNumberKey.MobilePhone ) )
            persona.Tipo = TipoPersona.Fisica;
        }

        persona.Nombre = contact.CompleteName.FullName;

        persona.Tratamiento = contact.CompleteName.Title;
        persona.NombrePila = contact.CompleteName.GivenName;
        persona.SegundoNombre = contact.CompleteName.MiddleName;
        persona.ApellidosPaterno = contact.CompleteName.Surname;
        persona.NombreFantasia = contact.CompleteName.NickName;
        DateTime? birthday;
        if( contact.TryGetProperty( ContactSchema.Birthday, out birthday ) )
          persona.NacimientoFecha = birthday;
        persona.Notas = contact.Notes;
        DateTime? anniversary;
        if( contact.TryGetProperty( ContactSchema.WeddingAnniversary, out anniversary ) )
          persona.AniversarioFecha = contact.WeddingAnniversary;

        SyncIdentificacion( persona, config.IdentificacionTipoWebTrabajo, objectSpace, contact.BusinessHomePage, "BusinessHomePage" );

        SyncPropiedad( persona, config.PropiedadPuesto, objectSpace, contact.JobTitle, "JobTitle" );
        SyncPropiedad( persona, config.PropiedadDepartamento, objectSpace, contact.Department, "Department" );
        SyncPropiedad( persona, config.PropiedadAsistente, objectSpace, contact.AssistantName, "AssistantName" );
        SyncPropiedad( persona, config.PropiedadProfesion, objectSpace, contact.Profession, "Profession" );
        SyncPropiedad( persona, config.PropiedadEsposo, objectSpace, contact.SpouseName, "SpouseName" );

        SyncIM( ImAddressKey.ImAddress1, config.IdentificacionTipoIM, contact, persona, objectSpace );
        SyncIM( ImAddressKey.ImAddress2, config.IdentificacionTipoIM, contact, persona, objectSpace );
        SyncIM( ImAddressKey.ImAddress3, config.IdentificacionTipoIM, contact, persona, objectSpace );

        SyncDireccion( PhysicalAddressKey.Home, config.DireccionTipoParticular, contact, persona, objectSpace, paresDirecciones );
        SyncDireccion( PhysicalAddressKey.Business, config.DireccionTipoLaboral, contact, persona, objectSpace, paresDirecciones );
        SyncDireccion( PhysicalAddressKey.Other, config.DireccionTipoOtra, contact, persona, objectSpace, paresDirecciones );

        SyncTelefono( PhoneNumberKey.AssistantPhone, config.IdentificacionTipoTelAsistente, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.BusinessFax, config.IdentificacionTipoTelFaxTrabajo, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.BusinessPhone, config.IdentificacionTipoTelTrabajo, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.BusinessPhone2, config.IdentificacionTipoTelTrabajo, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.Callback, config.IdentificacionTipoTelDevLlamada, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.CarPhone, config.IdentificacionTipoTelAuto, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.CompanyMainPhone, config.IdentificacionTipoTelPpalCia, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.HomeFax, config.IdentificacionTipoFaxParticular, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.HomePhone, config.IdentificacionTipoTelParticular, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.HomePhone2, config.IdentificacionTipoTelParticular, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.Isdn, config.IdentificacionTipoTelISDN, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.MobilePhone, config.IdentificacionTipoTelMovil, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.OtherFax, config.IdentificacionTipoFaxOtro, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.OtherTelephone, config.IdentificacionTipoTelOtro, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.Pager, config.IdentificacionTipoTelPager, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.PrimaryPhone, config.IdentificacionTipoTelPrimario, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.RadioPhone, config.IdentificacionTipoTelRadio, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.Telex, config.IdentificacionTipoTelex, contact, persona, objectSpace, config );
        SyncTelefono( PhoneNumberKey.TtyTddPhone, config.IdentificacionTipoTTY, contact, persona, objectSpace, config );

        SyncEmail( EmailAddressKey.EmailAddress1, config.IdentificacionTipoEmail, contact, persona, objectSpace );
        SyncEmail( EmailAddressKey.EmailAddress2, config.IdentificacionTipoEmail, contact, persona, objectSpace );
        SyncEmail( EmailAddressKey.EmailAddress3, config.IdentificacionTipoEmail, contact, persona, objectSpace );

        //TODO: buscar otro mecanismo para cargar la foto (manual, con un boton on-demand?)
        //es demasiado pesado para que sea automatico
        /*var hasPicture = false;
        contact.TryGetProperty( ContactSchema.HasPicture, out hasPicture );

        if ( hasPicture )
        {
          //contact.Load( new PropertySet( ItemSchema.Attachments ) );
          var pictureAttachment = contact.GetContactPictureAttachment( );
          if ( pictureAttachment != null )
          {
            var ms = new MemoryStream( );
            pictureAttachment.Load( ms );
            p.Imagen = Image.FromStream( ms );
          }
        }*/

        persona.Save( );

        Thread.Sleep( 1 );
      }
    }

    private static void SyncIdentificacion( Persona persona, IdentificacionTipo identificacion, IObjectSpace objectSpace, string value, string srcPropertyName )
    {
      if( !string.IsNullOrWhiteSpace( value ) && identificacion != null )
      {
        var identif = persona.Identificaciones.FirstOrDefault( r => r.Tipo.Oid == identificacion.Oid && r.Notas.Contains( "EXCH:" + srcPropertyName ) );
        if( identif == null )
        {
          identif = objectSpace.CreateObject< Identificacion >( );
          identif.Tipo = objectSpace.GetObjectByKey<IdentificacionTipo>( identificacion.Oid );
          identif.Notas += "\r\nEXCH:" + srcPropertyName;
          persona.Identificaciones.Add( identif );
        }
        identif.Valor = value;
      }
    }

    private static void SyncPropiedad( Persona persona, Propiedad propiedad, IObjectSpace objectSpace, string value, string srcPropertyName )
    {
      if( !string.IsNullOrWhiteSpace( value ) && propiedad != null )
      {
        var prop = persona.Propiedades.FirstOrDefault( r => r.Propiedad.Oid == propiedad.Oid && r.Notas.Contains( "EXCH:" + srcPropertyName ) );
        if( prop == null )
        {
          prop = objectSpace.CreateObject< PersonaPropiedad >( );
          prop.Propiedad = objectSpace.GetObjectByKey<Propiedad>( propiedad.Oid );
          prop.Notas += "\r\nEXCH:" + srcPropertyName;
          persona.Propiedades.Add( prop );
        }
        prop.Valor = value;
      }
    }

    private static void SyncDireccion( PhysicalAddressKey physicalAddressKey,
                                       DireccionTipo direccionTipo,
                                       Contact contact,
                                       Persona persona, IObjectSpace objectSpace,
                                       List< ParPersonaDireccionDireccion > paresDirecciones )
    {
      if( contact.PhysicalAddresses.Contains( physicalAddressKey ) && direccionTipo != null )
      {
        Direccion direccion;

        var homeAddr = contact.PhysicalAddresses[ physicalAddressKey ];

        var personaDir = persona.Direcciones.FirstOrDefault( r => r.DireccionTipo.Oid == direccionTipo.Oid );
        if( personaDir == null )
        {
          personaDir = objectSpace.CreateObject< PersonaDireccion >( );
          personaDir.DireccionTipo = objectSpace.GetObjectByKey< DireccionTipo >( direccionTipo.Oid );
          persona.Direcciones.Add( personaDir );

          direccion = objectSpace.CreateObject< Direccion >( );
          paresDirecciones.Add( new ParPersonaDireccionDireccion { Direccion = direccion, PersonaDireccion = personaDir } );
        }
        else
        {
          direccion = personaDir.Direccion;
        }

        direccion.Calle = homeAddr.Street;
        direccion.CiudadOtra = homeAddr.City;
        direccion.ProvinciaOtra = homeAddr.State;
        direccion.PaisOtro = homeAddr.CountryOrRegion;
        direccion.CodigoPostal = homeAddr.PostalCode;

        direccion.Save( );
      }
    }

    private static void SyncTelefono( PhoneNumberKey phoneNumberKey,
                                      IdentificacionTipo identificacionTipo,
                                      Contact contact,
                                      Persona persona, IObjectSpace objectSpace, Identificadores config )
    {
      if( identificacionTipo == null )
        identificacionTipo = config.IdentificacionTipoTelOtro;

      if( contact.PhoneNumbers.Contains( phoneNumberKey ) )
      {
        var phone = contact.PhoneNumbers[ phoneNumberKey ];
        SyncIdentificacion( persona, identificacionTipo, objectSpace, phone, phoneNumberKey.ToString( ) );
      }
    }

    private static void SyncEmail( EmailAddressKey emailAddressKey,
                                   IdentificacionTipo identificacionTipo,
                                   Contact contact,
                                   Persona persona, IObjectSpace objectSpace )
    {

      if( contact.EmailAddresses.Contains( emailAddressKey ) )
      {
        var email = contact.EmailAddresses[ emailAddressKey ];
        SyncIdentificacion( persona, identificacionTipo, objectSpace, email.Address, emailAddressKey.ToString( ) );
      }
    }

    private static void SyncIM( ImAddressKey imAddressKey,
                                   IdentificacionTipo identificacionTipo,
                                   Contact contact,
                                   Persona persona, IObjectSpace objectSpace )
    {

      if( contact.ImAddresses.Contains( imAddressKey ) )
      {
        var im = contact.ImAddresses[ imAddressKey ];
        SyncIdentificacion( persona, identificacionTipo, objectSpace, im, imAddressKey.ToString( ) );
      }
    }

    public struct ParPersonaDireccionDireccion
    {
      public Direccion Direccion;
      public PersonaDireccion PersonaDireccion;
    }
  }
}
