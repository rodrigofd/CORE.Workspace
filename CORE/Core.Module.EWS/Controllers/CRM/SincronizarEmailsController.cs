using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using FDIT.Core.CRM;
using FDIT.Core.Seguridad;
using FDIT.Core.Ventas;
using Microsoft.Exchange.WebServices.Data;

namespace FDIT.Core.EWS.Controllers
{
  public class SincronizarEmailsController : ViewController
  {
    private IContainer components;
    private SimpleAction sincronizarEmailsExchangeAction;

    public SincronizarEmailsController( )
    {
      InitializeComponent( );

      RegisterActions( components );
    }

    private void InitializeComponent( )
    {
      components = new Container( );
      sincronizarEmailsExchangeAction = new SimpleAction( components );
      // 
      // recuperarAutorizacionAction
      // 
      sincronizarEmailsExchangeAction.Caption = "Sincronizar con Exchange";
      sincronizarEmailsExchangeAction.ConfirmationMessage = null;
      sincronizarEmailsExchangeAction.Id = "sincronizarEmailsExchangeAction";
      sincronizarEmailsExchangeAction.ImageName = "ms_exchange";
      sincronizarEmailsExchangeAction.Shortcut = null;
      sincronizarEmailsExchangeAction.Tag = null;
      sincronizarEmailsExchangeAction.TargetObjectsCriteria = null;
      sincronizarEmailsExchangeAction.TargetViewId = null;
      sincronizarEmailsExchangeAction.ToolTip = null;
      sincronizarEmailsExchangeAction.TypeOfView = null;
      sincronizarEmailsExchangeAction.Execute += SincronizarEmailsActionExecute;
      // 
      // AutorizarComprobanteVentaController
      // 
      TargetObjectType = typeof( Actividad );
      TargetViewNesting = Nesting.Root;
      TargetViewType = ViewType.ListView;
    }

    private void SincronizarEmailsActionExecute( object sender, SimpleActionExecuteEventArgs e )
    {
      var config = Identificadores.GetInstance( ObjectSpace );

      if( config.CuentaSincEmails == null )
      {
        throw new UserFriendlyException( "No se configuró la cuenta de Exchange a utilizar para sincronizar emails." );
      }

      if( config.ActividadTipoEmail == null )
      {
        throw new UserFriendlyException( "No se configuró el tipo de actividad CRM para sincronizar emails." );
      }

      if( config.ActividadEstadoEmailPendiente == null )
      {
        throw new UserFriendlyException( "No se configuró el estado de actividad CRM para los emails con marca de pendiente." );
      }

      if( config.ActividadEstadoEmailCompletado == null )
      {
        throw new UserFriendlyException( "No se configuró el estado de actividad CRM para los emails con marca de completado." );
      }

      var service = config.CuentaSincEmails.ExchangeService;

      var objectSpace = Application.CreateObjectSpace( );

      var clientes = objectSpace.GetObjects( typeof( Cliente ) );
      var patrones = ( from Cliente cliente in clientes where !string.IsNullOrEmpty( cliente.PatronEmailEntrante ) select new ClientePatron { Cliente = cliente, Patron = new Regex( cliente.PatronEmailEntrante ) } ).ToList( );

      var results = service.FindItems( WellKnownFolderName.Inbox, "", new ItemView( 2000 ) );
      service.LoadPropertiesForItems( results, new PropertySet( BasePropertySet.FirstClassProperties, ItemSchema.NormalizedBody, ItemSchema.Attachments ) );

      foreach( var email in results.OfType< EmailMessage >( ).Select( item => item ) )
      {
        var actividad = objectSpace.FindObject< Actividad >( new BinaryOperator( EWSModule.PropertyCodigoEWS, email.Id.UniqueId ) ) ?? objectSpace.CreateObject< Actividad >( );

        var modifEWS = ( DateTime? ) actividad.GetMemberValue( EWSModule.PropertyModifFechaEWS );
        if( modifEWS.HasValue && modifEWS.Value == email.LastModifiedTime ) continue;

        actividad.SetMemberValue( EWSModule.PropertyCodigoEWS, email.Id.UniqueId );
        actividad.SetMemberValue( EWSModule.PropertyModifFechaEWS, email.LastModifiedTime );

        actividad.Grupo = CoreAppLogonParameters.Instance.GrupoActual( actividad.Session );

        actividad.Tipo = objectSpace.GetObjectByKey< ActividadTipo >( config.ActividadTipoEmail.Oid );
        actividad.Asunto = email.Subject;
        actividad.Contenido = email.NormalizedBody.Text;

        if( email.Flag != null )
        {
          if( email.Flag.FlagStatus == ItemFlagStatus.Flagged )
            actividad.Estado = objectSpace.GetObjectByKey< ActividadEstado >( config.ActividadEstadoEmailPendiente.Oid );
          else if( email.Flag != null && email.Flag.FlagStatus == ItemFlagStatus.Complete )
            actividad.Estado = objectSpace.GetObjectByKey< ActividadEstado >( config.ActividadEstadoEmailCompletado.Oid );
        }

        actividad.DireccionRemitente = email.From.Address;

        actividad.DireccionDestinatario = email.ToRecipients.Aggregate( "", ( current, dir ) => current + ( dir.Address + ";" ) );
        if( actividad.DireccionDestinatario.Length > 0 )
          actividad.DireccionDestinatario = actividad.DireccionDestinatario.Remove( actividad.DireccionDestinatario.Length - 1 );

        actividad.DireccionCC = email.CcRecipients.Aggregate( "", ( current, dir ) => current + ( dir.Address + ";" ) );
        if( actividad.DireccionCC.Length > 0 )
          actividad.DireccionCC = actividad.DireccionCC.Remove( actividad.DireccionCC.Length - 1 );

        actividad.DireccionBCC = email.BccRecipients.Aggregate( "", ( current, dir ) => current + ( dir.Address + ";" ) );
        if( actividad.DireccionBCC.Length > 0 )
          actividad.DireccionBCC = actividad.DireccionBCC.Remove( actividad.DireccionBCC.Length - 1 );

        actividad.Inicio = actividad.Fin = email.DateTimeReceived;

        foreach( var patron in patrones.Where( patron => patron.Patron.IsMatch( email.From.Address ) ) )
        {
          actividad.Cliente = patron.Cliente;
          break;
        }

        actividad.Save( );
      }

      objectSpace.CommitChanges( );

      ObjectSpace.Refresh( );
    }

    private struct ClientePatron
    {
      public Cliente Cliente;
      public Regex Patron;
    }
  }
}
