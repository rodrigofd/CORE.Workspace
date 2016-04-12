using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.CRM;
using FDIT.Core.Seguridad;

namespace FDIT.Core.Controllers.CRM
{
  public class ActividadResponsableController : ViewController<ListView>
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public ActividadResponsableController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( ActividadResponsable );
      TargetViewType = ViewType.ListView;
      TargetViewNesting = Nesting.Nested;
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      var ctrl = Frame.GetController<LinkUnlinkController>( );
      ctrl.UnlinkAction.Active[ "Overriden by ActividadResponsableController" ] = false;
      ctrl.CustomCreateLinkView += ActividadResponsableController_CustomCreateLinkView;
      ctrl.QueryLinkObjects += ActividadResponsableController_QueryLinkObjects;
    }

    protected override void OnDeactivated( )
    {
      var ctrl = Frame.GetController<LinkUnlinkController>( );
      ctrl.UnlinkAction.Active.RemoveItem( "Overriden by ActividadResponsableController" );
      ctrl.CustomCreateLinkView -= ActividadResponsableController_CustomCreateLinkView;
      ctrl.QueryLinkObjects -= ActividadResponsableController_QueryLinkObjects;

      base.OnDeactivated( );
    }

    private void ActividadResponsableController_CustomCreateLinkView( object sender, CustomCreateLinkViewEventArgs e )
    {
      e.LinkView = Application.CreateListView( ObjectSpace, typeof( Usuario ), false );
      e.Handled = true;
    }

    private void ActividadResponsableController_QueryLinkObjects( object sender, QueryLinkObjectsEventArgs e )
    {
      var parent = this.GetMasterObject<Actividad>( );
      var selectedObjects = e.LinkWindow.View.SelectedObjects;
      var linkObjects = new List< object >( );

      foreach( Usuario p in selectedObjects )
      {
        var nuevoItem = ObjectSpace.CreateObject<ActividadResponsable>( );

        nuevoItem.Actividad = parent;
        nuevoItem.Responsable = p;

        linkObjects.Add( nuevoItem );
      }

      e.LinkObjects = linkObjects;
      e.Handled = true;
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
      components = new System.ComponentModel.Container( );
    }

    #endregion
  }
}
