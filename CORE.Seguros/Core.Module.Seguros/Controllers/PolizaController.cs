using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ViewVariantsModule;
using FDIT.Core.Controllers;
using FDIT.Core.Seguridad;

namespace FDIT.Core.Seguros.Controllers
{
  public class PolizaController : ViewController
  {
    private SingleChoiceAction nuevoEndosoAction;
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public PolizaController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( Poliza );
      TargetViewNesting = Nesting.Root;
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
      var tipos = ObjectSpace.CreateCollection( typeof( DocumentoTipo ), CriteriaOperator.Parse( "Clase = 'Endoso'" ) );
      foreach( DocumentoTipo tipo in tipos )
      {
        nuevoEndosoAction.Items.Add( new ChoiceActionItem( tipo.Nombre, tipo ) );
      }
      base.OnActivated( );
    }
    
    protected override void OnDeactivated( )
    {
      base.OnDeactivated( );
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container();
      this.nuevoEndosoAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
      // 
      // nuevoEndosoAction
      // 
      this.nuevoEndosoAction.Caption = "Nuevo endoso";
      this.nuevoEndosoAction.Category = "Edit";
      this.nuevoEndosoAction.ConfirmationMessage = null;
      this.nuevoEndosoAction.Id = "b4fd9b10-45aa-4492-bb24-fbfc40ddb5c6";
      this.nuevoEndosoAction.ImageName = "document-sticky-note";
      this.nuevoEndosoAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
      this.nuevoEndosoAction.Shortcut = null;
      this.nuevoEndosoAction.Tag = null;
      this.nuevoEndosoAction.TargetObjectsCriteria = null;
      this.nuevoEndosoAction.TargetViewId = null;
      this.nuevoEndosoAction.ToolTip = null;
      this.nuevoEndosoAction.TypeOfView = null;
      this.nuevoEndosoAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.nuevoEndosoAction_Execute);

    }

    #endregion

    private void nuevoEndosoAction_Execute( object sender, SingleChoiceActionExecuteEventArgs e )
    {
      var docTipo = e.SelectedChoiceActionItem.Data as DocumentoTipo;
      var poliza = this.View.CurrentObject as Poliza;

      var os = Application.CreateObjectSpace( );

      var nuevoDocumento = os.CreateObject<Documento>( );
      nuevoDocumento.Empresa = CoreAppLogonParameters.Instance.EmpresaActual( os );
      nuevoDocumento.Poliza = os.GetObject(poliza);
      nuevoDocumento.Tipo = os.GetObject(docTipo);
      nuevoDocumento.VigenciaInicio = DateTime.Today;
      nuevoDocumento.VigenciaFin = nuevoDocumento.Poliza.VigenciaFin;
      
      var view = Application.CreateDetailView( os, nuevoDocumento, true );

      e.ShowViewParameters.CreatedView = view;
      e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
      
    }
  }
}
