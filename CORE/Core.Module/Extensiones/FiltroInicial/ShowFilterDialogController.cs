using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.FiltroInicial;

namespace FDIT.Core.Controllers.ViewFilter
{
    public class ShowFilterDialogController : WindowController, IModelExtender
    {
        public ShowFilterDialogController()
        {
            TargetWindowType = WindowType.Main;
        }
        
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelListView, IModelViewFiltroInicial>();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.Execute +=
                CustomShowNavigationItem_Execute;
        }

        private IModelNavigationItem GetModelByChoiceItem(ChoiceActionItem item, IModelNavigationItems items)
        {
            foreach (var modelNavigationItem in items)
            {
                if (modelNavigationItem.Id == item.Id) return modelNavigationItem;
                var tmp = GetModelByChoiceItem(item, modelNavigationItem.Items);
                if (tmp != null) return tmp;
            }

            return null;
        }

        private void CustomShowNavigationItem_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var showViewParameters = e.ShowViewParameters;
            var oldListView = showViewParameters.CreatedView;
            if (!(showViewParameters.CreatedView is ListView)) return;

            var modelViewFiltroInicial = (IModelViewFiltroInicial) showViewParameters.CreatedView.Model;

            if (modelViewFiltroInicial.TipoFiltro == TipoFiltro.Automatico)
            {
                var objectSpace = Application.CreateObjectSpace();

                var newViewFilterContainer = objectSpace.CreateObject<ViewFilterContainer>();
                newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type;

                showViewParameters.CreatedView = Application.CreateDetailView(objectSpace, newViewFilterContainer);
                showViewParameters.CreatedView.Caption =
                    !string.IsNullOrEmpty(modelViewFiltroInicial.TituloVistaParametros)
                        ? modelViewFiltroInicial.TituloVistaParametros
                        : $"Filtro para vista {oldListView.Caption}";
                ((DetailView) showViewParameters.CreatedView).ViewEditMode = ViewEditMode.Edit;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;

                var dialogController = Application.CreateController<DialogController>();
                dialogController.AcceptAction.Execute += (sender1, e1) =>
                {
                    var currentViewFilterContainer = (ViewFilterContainer) e1.CurrentObject;
                    ((ListView) oldListView).CollectionSource.Criteria["ByViewFilterObject"] =
                        CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria,
                            currentViewFilterContainer.ObjectType,
                            oldListView.ObjectSpace);
                    Application.MainWindow.SetView(oldListView);
                };
                showViewParameters.Controllers.Add(dialogController);
            }
        }
    }
}