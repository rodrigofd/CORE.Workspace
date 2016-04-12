using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using FDIT.Core.Fondos;

namespace FDIT.Core.Controllers.Fondos
{
  public class ComprobanteItemListViewController : ViewController<ListView>
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;
    private PopupWindowShowAction generarContrapartidaAction;

    public ComprobanteItemListViewController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( ComprobanteItem );
    }

    protected override void OnFrameAssigned( )
    {
      base.OnFrameAssigned( );

      var newObjectViewController = Frame.GetController< NewObjectViewController >( );
      if( newObjectViewController != null )
        newObjectViewController.NewObjectAction.Active.Changed += ( sender, e ) => { generarContrapartidaAction.Active[ "SyncNewAction" ] = ( ( BoolList ) sender ).ResultValue; };
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

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new Container( );
      // 
      // asignarValorExistente
      // 
      this.generarContrapartidaAction = new PopupWindowShowAction( this.components );
      this.generarContrapartidaAction.Caption = "Generar contrapartida";
      this.generarContrapartidaAction.AcceptButtonCaption = "Generar";
      this.generarContrapartidaAction.Category = "ObjectsCreation";
      this.generarContrapartidaAction.ConfirmationMessage = null;
      this.generarContrapartidaAction.Id = "generarContrapartidaAction";
      this.generarContrapartidaAction.ImageName = "table-select-cells";
      this.generarContrapartidaAction.Shortcut = null;
      this.generarContrapartidaAction.Tag = null;
      this.generarContrapartidaAction.TargetObjectsCriteria = null;
      this.generarContrapartidaAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
      this.generarContrapartidaAction.TargetViewId = null;
      this.generarContrapartidaAction.ToolTip = null;
      this.generarContrapartidaAction.TypeOfView = null;
      this.generarContrapartidaAction.CustomizePopupWindowParams += generarContrapartidaAction_CustomizePopupWindowParams;
      this.generarContrapartidaAction.Execute += generarContrapartidaAction_Execute;
    }

    #endregion

    private void generarContrapartidaAction_CustomizePopupWindowParams( object sender, CustomizePopupWindowParamsEventArgs e )
    {
      e.View = Application.CreateListView( ObjectSpace, typeof( FDIT.Core.Fondos.Cuenta ), false );
      e.DialogController.SaveOnAccept = false;
    }

    private void generarContrapartidaAction_Execute( object sender, PopupWindowShowActionExecuteEventArgs e )
    {
      if( e.PopupWindow.View.SelectedObjects.Count < 1 ) return;

      var cuentaDestino = ( Cuenta ) e.PopupWindow.View.SelectedObjects[ 0 ];

      var comprobante = this.GetMasterObject<Comprobante>( );

      foreach( ComprobanteItem comprobanteItem in View.SelectedObjects )
      {
        var nuevoComprobanteItem = ObjectSpace.CreateObject<ComprobanteItem>( );

        nuevoComprobanteItem.Cuenta = cuentaDestino;
        nuevoComprobanteItem.DebeHaber = comprobanteItem.DebeHaber == DebeHaber.Debe ? DebeHaber.Haber : DebeHaber.Debe;
        nuevoComprobanteItem.Especie = comprobanteItem.Especie;
        nuevoComprobanteItem.Importe = comprobanteItem.Importe;
        nuevoComprobanteItem.Cambio = comprobanteItem.Cambio;

        foreach( ComprobanteItemValor valor in comprobanteItem.Valores )
        {
          var comprobanteItemValor = ObjectSpace.CreateObject< ComprobanteItemValor >( );
          comprobanteItemValor.Valor = valor.Valor;
          nuevoComprobanteItem.Valores.Add( comprobanteItemValor );
        }

        comprobante.Items.Add( nuevoComprobanteItem );
      }
    }
  }
}