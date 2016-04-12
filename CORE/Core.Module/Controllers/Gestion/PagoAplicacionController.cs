using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Gestion;

namespace FDIT.Core.Controllers.Gestion
{
  public class PagoAplicacionController : ViewController
  {
    protected SimpleAction PagoAplicarCuotaAction;

    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public PagoAplicacionController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( PagoAplicacion );
      TargetViewType = ViewType.ListView;
      TargetViewNesting = Nesting.Nested;
    }

    protected override void OnFrameAssigned( )
    {
      base.OnFrameAssigned( );

      //var newObjectViewController = Frame.GetController< ModificationsController >( );
      //if( newObjectViewController != null )
      //  newObjectViewController.NewObjectAction.Active.Changed += ( sender, e ) => { aplicarCuotaAction.Active[ "SyncNewAction" ] = ( ( BoolList ) sender ).ResultValue; };
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

    private void PagoAplicarCuota_Execute( object sender, SimpleActionExecuteEventArgs e )
    {
      var cuotasConSaldo = new CollectionSource( ObjectSpace, typeof( ComprobanteCuota ), true );

      SetupCollectionSource( cuotasConSaldo );

      SetupShowViewParameters( e, cuotasConSaldo );

      var dialogController = Application.CreateController< DialogController >( );
      e.ShowViewParameters.CreateAllControllers = true;
      dialogController.SaveOnAccept = false;
      dialogController.Accepting += PagoAplicarCuota_DialogController_Accepting;
      e.ShowViewParameters.CreatedView.Tag = View;
      e.ShowViewParameters.Controllers.Add( dialogController );
    }

    protected virtual void SetupCollectionSource( CollectionSource collectionSource )
    {
      collectionSource.Criteria.Add( "SaldoPositivo", CriteriaOperator.Parse( "Saldo > 0" ) );
    }

    protected virtual void SetupShowViewParameters( SimpleActionExecuteEventArgs e, CollectionSource collectionSource )
    {
      e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
      e.ShowViewParameters.Context = TemplateContext.PopupWindow;
    }

    private void PagoAplicarCuota_DialogController_Accepting( object sender, DialogControllerAcceptingEventArgs e )
    {
      var selectedCuotas = e.AcceptActionArgs.SelectedObjects;
      if( selectedCuotas.Count == 0 )
      {
        e.Cancel = true;
        return;
      }

      var pago = this.GetMasterObject< Pago >( );

      foreach( ComprobanteCuota cuota in selectedCuotas )
      {
        var apl = ObjectSpace.CreateObject< PagoAplicacion >( );
        apl.Cuota = cuota;
        //El uso del enum DebitoCredito del tipo de comprobante, nos permite considerar signo de la aplicación (los comprobantes y cuotas se guardan todos positivos)
        apl.Importe = cuota.Saldo * ( decimal ) cuota.Comprobante.Tipo.DebitoCredito;
        apl.Cambio = 1;
        apl.Especie = cuota.Comprobante.Especie;

        pago.Aplicaciones.Add( apl );
      }
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container( );
      CreateActions( );
    }

    protected virtual void CreateActions( )
    {
      this.PagoAplicarCuotaAction = new DevExpress.ExpressApp.Actions.SimpleAction( this.components );
      // 
      // PagoAplicarCuotaAction
      // 
      this.PagoAplicarCuotaAction.Caption = "Aplicar cuota con saldo";
      this.PagoAplicarCuotaAction.Category = "ObjectsCreation";
      this.PagoAplicarCuotaAction.ConfirmationMessage = null;
      this.PagoAplicarCuotaAction.Id = "PagoAplicarCuotaAction";
      this.PagoAplicarCuotaAction.ImageName = "receipt_stamp";
      this.PagoAplicarCuotaAction.Shortcut = null;
      this.PagoAplicarCuotaAction.Tag = null;
      this.PagoAplicarCuotaAction.TargetObjectsCriteria = null;
      this.PagoAplicarCuotaAction.TargetViewId = null;
      this.PagoAplicarCuotaAction.ToolTip = null;
      this.PagoAplicarCuotaAction.TypeOfView = null;
      this.PagoAplicarCuotaAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler( this.PagoAplicarCuota_Execute );
    }

    #endregion
  }
}
