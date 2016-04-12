using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Personas;

namespace FDIT.Core.Controllers.Personas
{
    public class RolController : ViewController
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        private SingleChoiceAction nuevoRolAction;

        public RolController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (Rol);

            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ObjectTypeInfo.Type != typeof (Rol)) return;

            Frame.GetController<NewObjectViewController>().NewObjectAction.Active["fixed"] = false;

            nuevoRolAction.Items.Clear();

            var rolType = XafTypesInfo.Instance.PersistentTypes.First(info => info.Type == typeof (Rol));
            foreach (var type in rolType.Descendants.OrderBy(info => info.Name))
            {
                var desc = type.Name;
                var displayNameAttr = type.FindAttribute<DisplayNameAttribute>();
                if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                nuevoRolAction.Items.Add(new ChoiceActionItem(desc, type.Type));
            }
        }

        protected override void OnDeactivated()
        {
            if (View.ObjectTypeInfo.Type != typeof (Rol)) return;

            Frame.GetController<NewObjectViewController>().NewObjectAction.Active.RemoveItem("fixed");
            base.OnDeactivated();
        }

        private void nuevoRolAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var objectType = (Type) e.SelectedChoiceActionItem.Data;

            var newObj = (Rol) ObjectSpace.CreateObject(objectType);
            newObj.Persona = (Persona) ((PropertyCollectionSource) ((ListView) View).CollectionSource).MasterObject;

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(ObjectSpace, newObj, false);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.PopupWindow;

            var dialogController = Application.CreateController<DialogController>();
            dialogController.SaveOnAccept = false;
            dialogController.Accepting += DialogController_Accepting;
            e.ShowViewParameters.CreatedView.Tag = View;
            e.ShowViewParameters.Controllers.Add(dialogController);
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var view = ((DialogController) sender).Frame.View;
            var selectedObjects = (Rol) view.CurrentObject;
            var masterView = (ListView) view.Tag;

            var persona = (Persona) ((PropertyCollectionSource) masterView.CollectionSource).MasterObject;

            persona.Roles.Add(selectedObjects);
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
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
            this.components = new Container();
            this.nuevoRolAction = new SingleChoiceAction(this.components);
            // 
            // nuevoVinculoAction
            // 
            this.nuevoRolAction.Caption = "Asignar rol";
            this.nuevoRolAction.Category = "RecordEdit";
            this.nuevoRolAction.ConfirmationMessage = null;
            this.nuevoRolAction.Id = "nuevoRolAction";
            this.nuevoRolAction.ImageName = "MenuBar_Link";
            this.nuevoRolAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            this.nuevoRolAction.Shortcut = null;
            this.nuevoRolAction.Tag = null;
            this.nuevoRolAction.TargetObjectsCriteria = null;
            this.nuevoRolAction.TargetViewId = null;
            this.nuevoRolAction.ToolTip = null;
            this.nuevoRolAction.TypeOfView = null;
            this.nuevoRolAction.Execute += new SingleChoiceActionExecuteEventHandler(this.nuevoRolAction_Execute);
        }

        #endregion
    }
}