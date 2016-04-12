using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Seguridad;

namespace FDIT.Core.Seguros.Controllers
{
  //Controlador que captura la accion CREAR NUEVO para el tipo Documento (desde cualquier lugar de la app.)
  public class NuevoDocumentoController : ViewController
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public NuevoDocumentoController( )
    {
      InitializeComponent( );
      RegisterActions( components );
    }

    /// <summary>
    ///   Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      Frame.GetController<NewObjectViewController>( ).ObjectCreated += NewObjectViewController_ObjectCreated;
    }

    private void NewObjectViewController_ObjectCreated( object sender, ObjectCreatedEventArgs e )
    {
      if( !( e.CreatedObject is Documento ) ) return;

      var objectSpace = e.ObjectSpace;
      var nuevoDocumento = ( Documento ) e.CreatedObject;

      //Al inicializar un nuevo objeto Documento, encapsularlo en una nueva Poliza, y este a su vez en una nueva Carpeta
      //(cuando comienza un nuevo negocio desde cero)

      var identif = Identificadores.GetInstance( objectSpace );

      nuevoDocumento.Tipo = objectSpace.FindObject< DocumentoTipo >( CriteriaOperator.Parse( "Clase = 'Poliza'" ) );
      nuevoDocumento.Poliza = objectSpace.CreateObject< Poliza >( );
      nuevoDocumento.Poliza.Carpeta = objectSpace.CreateObject< Carpeta >( );

      var interviniente = objectSpace.CreateObject< DocumentoInterviniente >( );
      interviniente.Rol = identif.RolAseguradora;
      interviniente.Participacion = 100;
      interviniente.Principal = true;
      nuevoDocumento.Intervinientes.Add( interviniente );
      nuevoDocumento.Poliza.Aseguradora = interviniente;

      interviniente = objectSpace.CreateObject< DocumentoInterviniente >( );
      interviniente.Rol = identif.RolTomador;
      interviniente.Participacion = 100;
      interviniente.Principal = true;
      nuevoDocumento.Poliza.Tomador = interviniente;

      interviniente = objectSpace.CreateObject< DocumentoInterviniente >( );
      interviniente.Rol = identif.RolOrganizador;
      interviniente.Participacion = 100;
      interviniente.Principal = true;
      interviniente.Interviniente = CoreAppLogonParameters.Instance.EmpresaActual( objectSpace ).Persona;
      nuevoDocumento.Poliza.Organizador = interviniente;

      /*interv = e.ObjectSpace.CreateObject< DocumentoInterviniente >( );
      interv.Rol = ident.RolProductor;
      interv.Participacion = 100;
      interv.Principal = true;
      nuevoDoc.Poliza.Productor = interv;*/

      nuevoDocumento.Intervinientes.Add( nuevoDocumento.Poliza.Aseguradora );
      nuevoDocumento.Intervinientes.Add( nuevoDocumento.Poliza.Tomador );
      nuevoDocumento.Intervinientes.Add( nuevoDocumento.Poliza.Organizador );
      //nuevoDoc.Intervinientes.Add( nuevoDoc.Poliza.Productor );
    }

    protected override void OnDeactivated( )
    {
      Frame.GetController<NewObjectViewController>( ).ObjectCreated -= NewObjectViewController_ObjectCreated;
      
      base.OnDeactivated( );
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container( );
    }

    #endregion
  }
}
