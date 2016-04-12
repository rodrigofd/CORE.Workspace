#region

using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using FDIT.Core.Seguridad;

#endregion

namespace FDIT.Core.Controllers
{
  public class CustomNavigationController : WindowController
  {
    private DetailView createdDetailView;
    private NewObjectViewController newController;

    public CustomNavigationController( )
    {
      TargetWindowType = WindowType.Main;
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      Frame.GetController< ShowNavigationItemController >( ).CustomShowNavigationItem += SingletonNavigationController_CustomShowNavigationItem;
    }

    protected override void OnDeactivated( )
    {
      Frame.GetController< ShowNavigationItemController >( ).CustomShowNavigationItem -= SingletonNavigationController_CustomShowNavigationItem;

      base.OnDeactivated( );
    }

    private void SingletonNavigationController_CustomShowNavigationItem( object sender, CustomShowNavigationItemEventArgs e )
    {
      if( e.Handled ) return;
      
      var args = e.ActionArguments;

      if( args.SelectedChoiceActionItem != null && args.SelectedChoiceActionItem.Enabled.ResultValue )
      {
        if( args.SelectedChoiceActionItem.Id.StartsWith( "Singleton_" ) )
        {
          var parts = args.SelectedChoiceActionItem.Id.Split( '_' );
          var typeName = parts[ 1 ];

          var ti = XafTypesInfo.Instance.FindTypeInfo( typeName );

          var objectSpace = Application.CreateObjectSpace( );

          var empresaId = CoreAppLogonParameters.Instance.EmpresaActualId;

          var singletonInstance = ( XPBaseObject ) objectSpace.GetObjectByKey( ti.Type, empresaId );
          if( singletonInstance == null )
          {
            singletonInstance = ( XPBaseObject ) objectSpace.CreateObject( ti.Type );
            singletonInstance.SetMemberValue( "Empresa", empresaId );
            singletonInstance.Save( );
          }

          args.ShowViewParameters.TargetWindow = TargetWindow.Current;
          args.ShowViewParameters.CreatedView = Application.CreateDetailView( objectSpace, singletonInstance, true );

          e.Handled = true;
        }
        if( args.SelectedChoiceActionItem.Id.StartsWith( "New_" ) )
        {
          var parts = args.SelectedChoiceActionItem.Id.Split( '_' );
          var typeName = parts[ 1 ];

          var ti = XafTypesInfo.Instance.FindTypeInfo( typeName );

          var workFrame = Application.CreateFrame( TemplateContext.ApplicationWindow );
          workFrame.SetView( Application.CreateListView( Application.CreateObjectSpace( ), ti.Type, true ) );
          newController = workFrame.GetController< NewObjectViewController >( );

          if( newController != null )
          {
            var newObjectItem = FindNewObjectItem( ti.Type );
            if( newObjectItem != null )
            {
              newController.NewObjectAction.Executed += NewObjectAction_Executed;
              newController.NewObjectAction.DoExecute( newObjectItem );
              newController.NewObjectAction.Executed -= NewObjectAction_Executed;
              args.ShowViewParameters.TargetWindow = TargetWindow.Default;
              args.ShowViewParameters.CreatedView = createdDetailView;

              e.Handled = true;
            }
          }
        }
        else if( args.SelectedChoiceActionItem.Id.StartsWith( "Wizard_" ) )
        {
          var parts = args.SelectedChoiceActionItem.Id.Split( '_' );
          var typeName = parts[ 1 ];

          var ti = XafTypesInfo.Instance.FindTypeInfo( typeName );

          var os = Application.CreateObjectSpace( );
          var v = Application.CreateDetailView( os, "Wizard_" + parts[ 1 ] + "_Page1", true, os.CreateObject( ti.Type ) );

          v.ViewEditMode = ViewEditMode.Edit;

          Application.ShowViewStrategy.ShowView( new ShowViewParameters { Context = TemplateContext.ApplicationWindow, CreatedView = v, CreateAllControllers = true, NewWindowTarget = NewWindowTarget.Default, TargetWindow = TargetWindow.Current },
                                                 new ShowViewSource( Frame, e.ActionArguments.Action ) );

          e.Handled = true;
        }
        else if( args.SelectedChoiceActionItem.Id.StartsWith( "NPD_" ) )
        {
          var parts = args.SelectedChoiceActionItem.Id.Split( '_' );
          var typeName = parts[ 1 ];

          var ti = XafTypesInfo.Instance.FindTypeInfo( typeName );

          var os = Application.CreateObjectSpace( );
          var v = Application.CreateDetailView( os, parts[ 1 ] + "_DetailView", true, os.CreateObject( ti.Type ) );

          v.ViewEditMode = ViewEditMode.Edit;

          Application.ShowViewStrategy.ShowView( new ShowViewParameters { Context = TemplateContext.ApplicationWindow, CreatedView = v, CreateAllControllers = true, NewWindowTarget = NewWindowTarget.Default, TargetWindow = TargetWindow.Current },
                                                 new ShowViewSource( Frame, e.ActionArguments.Action ) );

          e.Handled = true;
        }
        else if( args.SelectedChoiceActionItem.Id.StartsWith( "NPL_" ) )
        {
          var parts = args.SelectedChoiceActionItem.Id.Split( '_' );
          var typeName = parts[ 1 ];

          var ti = XafTypesInfo.Instance.FindTypeInfo( typeName );

          var os = Application.CreateObjectSpace( );
          var cs = new CollectionSource( os, ti.Type );

          var v = Application.CreateListView( parts[ 1 ] + "_ListView", cs, true );
          
          Application.ShowViewStrategy.ShowView( new ShowViewParameters { Context = TemplateContext.ApplicationWindow, CreatedView = v, CreateAllControllers = true, NewWindowTarget = NewWindowTarget.Default, TargetWindow = TargetWindow.Current },
                                                 new ShowViewSource( Frame, e.ActionArguments.Action ) );

          e.Handled = true;
        }
      }
    }

    private ChoiceActionItem FindNewObjectItem( Type type )
    {
      return newController.NewObjectAction.Items.FirstOrDefault( item => item.Data == type );
    }

    private void NewObjectAction_Executed( object sender, ActionBaseEventArgs e )
    {
      createdDetailView = e.ShowViewParameters.CreatedView as DetailView;
      //Cancel showing the default View by the NewObjectAction 
      e.ShowViewParameters.CreatedView = null;
    }
  }
}
