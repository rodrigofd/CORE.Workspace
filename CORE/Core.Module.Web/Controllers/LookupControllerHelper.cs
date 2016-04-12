using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Templates;

namespace FDIT.Core.Web.Controllers.Personas
{
  public class LookupControllerHelper
  {
    private Controller fController;
    private ActionBase fAction;

    public LookupControllerHelper( Controller controller, ActionBase action )
    {
      fController = controller;
      fAction = action;
    }

    public void dc_WindowTemplateChanged( object sender, EventArgs e )
    {
      var window = ( ( WindowController ) sender ).Window;
      var template = window.Template;
      if( template == null ) return;

      var lookup = template as BaseXafPage;
      if( lookup == null ) return;

      lookup.Init += page_Init;
    }

    public void OnActivated( )
    {
      var nf = (NestedFrame)fController.Frame;
      var dv = (DetailView)nf.ViewItem.View;

      dv.ViewEditModeChanged += dv_ViewEditModeChanged;
    }

    public void OnDeactivated( )
    {
      var nf = ( NestedFrame ) fController.Frame;
      var dv = ( DetailView ) nf.ViewItem.View;

      dv.ViewEditModeChanged -= dv_ViewEditModeChanged;
    }

    public void UpdateActionState( )
    {
      var nf = ( NestedFrame ) fController.Frame;
      var dv = ( DetailView ) nf.ViewItem.View;
      
      fAction.Active[ "InEditMode" ] = ( dv.ViewEditMode == ViewEditMode.Edit );
    }

    private void page_Init( object sender, EventArgs e )
    {
      var page = ( BaseXafPage ) sender;
      page.Init -= page_Init;
      if( page.TemplateContent != null )
      {
        ( ( ILookupPopupFrameTemplate ) page.TemplateContent ).IsSearchEnabled = true;
      }
    }

    private void dv_ViewEditModeChanged( object sender, EventArgs e )
    {
      UpdateActionState( );
    }
  }
}