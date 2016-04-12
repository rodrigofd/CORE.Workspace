using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.Persistent.Base;
using FDIT.Core.Fondos;

namespace FDIT.Core.Controllers.Fondos
{
  public class ComprobanteItemValorController : ViewController
  {
    private const string fInactiveReason = "Hidden by ComprobanteItemValorController";
    private PopupWindowShowAction asignarValorExistente;

    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    private SingleChoiceAction nuevoFondoValor;

    public ComprobanteItemValorController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( ComprobanteItemValor );
      TargetViewType = ViewType.ListView;
      TargetViewNesting = Nesting.Nested;
    }

    protected override void OnViewChanging( View view )
    {
      base.OnViewChanging( view );

      bool active = false;

      var listView = view as ListView;
      if( listView != null )
      {
        var masterObject = this.GetMasterObject( listView );
        active = masterObject is ComprobanteItem;
      }

      Active[ "Master object is ComprobanteItem" ] = active;
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
      Frame.GetController< ChangeVariantController >( ).ChangeVariantAction.Active[ fInactiveReason ] = false;
      Frame.GetController< NewObjectViewController >( ).NewObjectAction.Active[ fInactiveReason ] = false;
      Frame.GetController< ListViewProcessCurrentObjectController >( ).CustomizeShowViewParameters += ListViewProcessCurrentObjectController_CustomizeShowViewParameters;

      FillActionItems( );
    }

    void ObjectSpace_ObjectChanged( object sender, ObjectChangedEventArgs e )
    {
      if( e.PropertyName != "Especie" ) return;

      FillActionItems( );
    }

    private void FillActionItems( )
    {
      var comprobanteItem = this.GetMasterObject< ComprobanteItem >( );
      nuevoFondoValor.Items.Clear( );

      var especie = comprobanteItem.Especie;

      if( especie == null ) return;
      //var especies = ObjectSpace.CreateCollection( typeof( Especie ) );
      //foreach( Especie especie in especies )
      {
        var t = !String.IsNullOrEmpty( especie.TipoValor ) ? Type.GetType( especie.TipoValor ) : typeof( Valor );
        var imgName = t.GetCustomAttribute< ImageNameAttribute >( );

        nuevoFondoValor.Items.Add( new ChoiceActionItem { Caption = especie.Nombre, Data = especie, ImageName = imgName != null ? imgName.ImageName : null } );
      }
    }

    protected override void OnDeactivated( )
    {
      var masterObject = this.GetMasterObject( );
      if( masterObject is ComprobanteItem )
      {
        ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
        Frame.GetController< ChangeVariantController >( ).ChangeVariantAction.Active.RemoveItem( fInactiveReason );
        Frame.GetController< NewObjectViewController >( ).NewObjectAction.Active.RemoveItem( fInactiveReason );
        Frame.GetController< ListViewProcessCurrentObjectController >( ).CustomizeShowViewParameters -= ListViewProcessCurrentObjectController_CustomizeShowViewParameters;
      }

      base.OnDeactivated( );
    }

    private void ListViewProcessCurrentObjectController_CustomizeShowViewParameters( object sender, CustomizeShowViewParametersEventArgs e )
    {
      var comprobanteItemValor = ( ComprobanteItemValor ) e.ShowViewParameters.CreatedView.CurrentObject;
      var nestedObjectSpace = ObjectSpace.CreateNestedObjectSpace( );
      e.ShowViewParameters.CreatedView = Application.CreateDetailView( nestedObjectSpace, nestedObjectSpace.GetObject( comprobanteItemValor.Valor ), true );
      e.ShowViewParameters.CreateAllControllers = true;
      e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
    }

    private void asignarValorExistente_CustomizePopupWindowParams( object sender, CustomizePopupWindowParamsEventArgs e )
    {
      var comprobanteItem = this.GetMasterObject< ComprobanteItem >( );
      var especie = comprobanteItem.Especie;

      if( especie == null ) throw new UserFriendlyException( "Debe indicar la especie del item." );

      var t = !String.IsNullOrEmpty( especie.TipoValor ) ? Application.TypesInfo.FindTypeInfo( especie.TipoValor ).Type : typeof( Valor );

      e.View = Application.CreateListView( ObjectSpace, t, false );
      e.DialogController.SaveOnAccept = false;
    }

    private void asignarValorExistente_Execute( object sender, PopupWindowShowActionExecuteEventArgs e )
    {
      var comprobanteItem = this.GetMasterObject< ComprobanteItem >( );

      foreach( Valor valor in e.PopupWindow.View.SelectedObjects )
      {
        if( comprobanteItem.Valores.Any( civ => (civ.Valor.Oid == valor.Oid && !ObjectSpace.IsNewObject( valor ) )|| civ.Valor.Equals(valor) ) )
          continue;

        var nuevoIV = ObjectSpace.CreateObject< ComprobanteItemValor >( );
        nuevoIV.Valor = valor;
        comprobanteItem.Valores.Add( nuevoIV );
      }
    }

    private void nuevoFondoValor_Execute( object sender, SingleChoiceActionExecuteEventArgs e )
    {
      var especie = ( Especie ) e.SelectedChoiceActionItem.Data;

      var t = !String.IsNullOrEmpty( especie.TipoValor ) ? Application.TypesInfo.FindTypeInfo( especie.TipoValor ).Type : typeof( Valor );
      var nuevoValor = ( Valor ) ObjectSpace.CreateObject( t );

      e.ShowViewParameters.CreatedView = Application.CreateDetailView( ObjectSpace, nuevoValor, false );
      e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
      e.ShowViewParameters.Context = TemplateContext.PopupWindow;

      nuevoValor.Especie = especie;

      var dc = Application.CreateController< DialogController >( );
      dc.SaveOnAccept = false;
      dc.AcceptAction.Execute += nuevoFondoValor_DialogController_Execute;
      e.ShowViewParameters.Controllers.Add( dc );
    }

    private void nuevoFondoValor_DialogController_Execute( object sender, SimpleActionExecuteEventArgs e )
    {
      var valor = ( Valor ) e.CurrentObject;
      var ci = this.GetMasterObject< ComprobanteItem >( );

      var nuevoIV = ObjectSpace.CreateObject< ComprobanteItemValor >( );
      nuevoIV.Valor = valor;

      ci.Valores.Add( nuevoIV );
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
      // nuevoFondoValor
      // 
      this.nuevoFondoValor = new SingleChoiceAction( this.components );
      this.nuevoFondoValor.Caption = "Nuevo valor";
      this.nuevoFondoValor.Category = "ObjectsCreation";
      this.nuevoFondoValor.ItemType = SingleChoiceActionItemType.ItemIsOperation;
      this.nuevoFondoValor.ConfirmationMessage = null;
      this.nuevoFondoValor.Id = "nuevoFondoValor";
      this.nuevoFondoValor.ImageName = "MenuBar_New";
      this.nuevoFondoValor.Shortcut = null;
      this.nuevoFondoValor.Tag = null;
      this.nuevoFondoValor.TargetObjectsCriteria = null;
      this.nuevoFondoValor.TargetViewId = null;
      this.nuevoFondoValor.ToolTip = null;
      this.nuevoFondoValor.TypeOfView = null;
      this.nuevoFondoValor.Execute += this.nuevoFondoValor_Execute;
      // 
      // asignarValorExistente
      // 
      this.asignarValorExistente = new PopupWindowShowAction( this.components );
      this.asignarValorExistente.Caption = "Valores en cartera";
      this.asignarValorExistente.AcceptButtonCaption = "Asociar";
      this.asignarValorExistente.Category = "ObjectsCreation";
      this.asignarValorExistente.ConfirmationMessage = null;
      this.asignarValorExistente.Id = "asignarValorExistente";
      this.asignarValorExistente.ImageName = "MenuBar_Link";
      this.asignarValorExistente.Shortcut = null;
      this.asignarValorExistente.Tag = null;
      this.asignarValorExistente.TargetObjectsCriteria = null;
      this.asignarValorExistente.TargetViewId = null;
      this.asignarValorExistente.ToolTip = null;
      this.asignarValorExistente.TypeOfView = null;
      this.asignarValorExistente.CustomizePopupWindowParams += asignarValorExistente_CustomizePopupWindowParams;
      this.asignarValorExistente.Execute += asignarValorExistente_Execute;
    }

    #endregion
  }
}
