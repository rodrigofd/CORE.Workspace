using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

namespace FDIT.Core.Controllers
{
    public class VinculosController : ViewController<ListView>
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        protected SingleChoiceAction nuevoVinculoAction;

        public VinculosController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (Vinculo);
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            nuevoVinculoAction.Items.Clear();
            foreach (var type in XafTypesInfo.Instance.PersistentTypes.OrderBy(info => info.Name))
            {
                if (type.FindAttribute<DefaultClassOptionsAttribute>() == null) continue;

                var desc = type.Name;
                var displayNameAttr = type.FindAttribute<DisplayNameAttribute>();
                if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                nuevoVinculoAction.Items.Add(new ChoiceActionItem(desc, type.Type));
            }

            UpdateActionState();
        }

        protected virtual void UpdateActionState()
        {
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            nuevoVinculoAction.Active["SecurityAllowNewByPermissions"] =
                newObjectViewController.NewObjectAction.Active["SecurityAllowNewByPermissions"];
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void nuevoVinculoAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var objectType = (Type) e.SelectedChoiceActionItem.Data;
            e.ShowViewParameters.CreatedView = Application.CreateListView(ObjectSpace, objectType, false);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.LookupWindow;

            var dc = Application.CreateController<DialogController>();
            dc.SaveOnAccept = false;
            dc.WindowTemplateChanged += dialogController_WindowTemplateChanged;
            dc.Accepting += DialogController_Accepting;
            e.ShowViewParameters.Controllers.Add(dc);
        }

        protected virtual void dialogController_WindowTemplateChanged(object sender, EventArgs e)
        {
            var window = ((WindowController) sender).Window;
            var template = window.Template;

            if (template == null) return;

            ((ILookupPopupFrameTemplate) template).IsSearchEnabled = true;
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var selectedObjects = e.AcceptActionArgs.SelectedObjects;
            var originante = (BasicObject) ((PropertyCollectionSource) View.CollectionSource).MasterObject;

            foreach (BasicObject selectedObject in selectedObjects)
            {
                var nuevoVinculo = ObjectSpace.CreateObject<Vinculo>();
                nuevoVinculo.Originante = originante;
                nuevoVinculo.Destinatario = selectedObject;

                View.CollectionSource.Add(nuevoVinculo);
            }
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.nuevoVinculoAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // nuevoVinculoAction
            // 
            this.nuevoVinculoAction.Caption = "Vincular con";
            this.nuevoVinculoAction.Category = "RecordEdit";
            this.nuevoVinculoAction.ConfirmationMessage = null;
            this.nuevoVinculoAction.Id = "nuevoVinculoAction";
            this.nuevoVinculoAction.ImageName = "MenuBar_Link";
            this.nuevoVinculoAction.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.nuevoVinculoAction.Shortcut = null;
            this.nuevoVinculoAction.Tag = null;
            this.nuevoVinculoAction.TargetObjectsCriteria = null;
            this.nuevoVinculoAction.TargetViewId = null;
            this.nuevoVinculoAction.ToolTip = null;
            this.nuevoVinculoAction.TypeOfView = null;
            this.nuevoVinculoAction.Execute +=
                new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.nuevoVinculoAction_Execute);
        }

        #endregion
    }
}