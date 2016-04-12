using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;

namespace FDIT.Core.Controllers
{
  public static class ControllerExtensions
  {
    public static object GetMasterObject( this ViewController controller )
    {
      var listView = ( ListView ) controller.View;
      return GetMasterObject( controller, listView );
    }

    public static object GetMasterObject( this ViewController controller, ListView listView )
    {
      var collectionSource = listView.CollectionSource as PropertyCollectionSource;
      return collectionSource != null ? collectionSource.MasterObject : null;
    }

    public static Type GetMasterType( this ViewController controller, ListView listView )
    {
      var masterObject = GetMasterObject( controller, listView );
      return masterObject != null ? masterObject.GetType( ) : null;
    }

    public static T GetMasterObject< T >( this ViewController controller )
    {
      return ( T ) controller.GetMasterObject( );
    }

    public static T GetMasterObject< T >( this ViewController controller, ListView listView )
    {
      return ( T ) controller.GetMasterObject( listView );
    }

    public static void ShowPopupListView( this ViewController controller, IObjectSpace objectSpace, ActionBaseEventArgs e, Type type, CriteriaOperator criteria, string viewId, EventHandler< DialogControllerAcceptingEventArgs > dcOnAccepting = null,
                                          EventHandler dcOnCancelling = null, SelectionDependencyType selectionDependencyType = SelectionDependencyType.Independent )
    {
      var polizaItems = new CollectionSource( objectSpace, type, true );
      polizaItems.Criteria[ "fixed" ] = criteria;

      e.ShowViewParameters.CreatedView = controller.Application.CreateListView( viewId, polizaItems, false );
      e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
      e.ShowViewParameters.Context = TemplateContext.PopupWindow;

      var dc = controller.Application.CreateController< DialogController >( );
      dc.AcceptAction.SelectionDependencyType = selectionDependencyType;
      dc.SaveOnAccept = false;
      dc.Accepting += dcOnAccepting;
      dc.Cancelling += dcOnCancelling;

      e.ShowViewParameters.Controllers.Add( dc );
    }
  }
}
