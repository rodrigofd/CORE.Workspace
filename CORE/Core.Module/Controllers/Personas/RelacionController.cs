using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Personas;

namespace FDIT.Core.Controllers.Personas
{
    /// <summary>
    ///     Controlador para la funcion de RELACIONAR una persona a otras, mediante un popup
    /// </summary>
    public class RelacionController : ViewController<ListView>
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        protected SimpleAction nuevaRelacionAction;

        public RelacionController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (Relacion);
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            UpdateActionState();
        }

        protected virtual void UpdateActionState()
        {
            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            nuevaRelacionAction.Active["SecurityAllowNewByPermissions"] =
                newObjectViewController.NewObjectAction.Active["SecurityAllowNewByPermissions"];
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void nuevaRelacionAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            e.ShowViewParameters.CreatedView = Application.CreateListView(ObjectSpace, typeof (Persona), false);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.LookupWindow;

            var dialogController = Application.CreateController<DialogController>();
            dialogController.SaveOnAccept = false;
            dialogController.WindowTemplateChanged += dialogController_WindowTemplateChanged;
            dialogController.Accepting += dialogController_Accepting;
            e.ShowViewParameters.Controllers.Add(dialogController);
        }

        protected virtual void dialogController_WindowTemplateChanged(object sender, EventArgs e)
        {
            var window = ((WindowController) sender).Window;
            var template = window.Template;
            if (template == null) return;

            var lookup = template as ILookupPopupFrameTemplate;
            if (lookup == null) return;

            lookup.IsSearchEnabled = true;
        }

        private void dialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var selectedObjects = e.AcceptActionArgs.SelectedObjects;
            var originante = this.GetMasterObject<Persona>();

            foreach (Persona selectedObject in selectedObjects)
            {
                var nuevoVinculo = ObjectSpace.CreateObject<Relacion>();
                nuevoVinculo.PersonaVinculante = originante;
                nuevoVinculo.PersonaVinculado = selectedObject;
                nuevoVinculo.RelacionTipo = null;

                View.CollectionSource.Add(nuevoVinculo);
            }
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
            this.components = new System.ComponentModel.Container();
            this.nuevaRelacionAction = new SimpleAction(this.components);
            // 
            // nuevoVinculoAction
            // 
            this.nuevaRelacionAction.Caption = "Relacionar";
            this.nuevaRelacionAction.Category = "RecordEdit";
            this.nuevaRelacionAction.ConfirmationMessage = null;
            this.nuevaRelacionAction.Id = "nuevaRelacionAction";
            this.nuevaRelacionAction.ImageName = "MenuBar_Link";
            this.nuevaRelacionAction.Shortcut = null;
            this.nuevaRelacionAction.Tag = null;
            this.nuevaRelacionAction.TargetObjectsCriteria = null;
            this.nuevaRelacionAction.TargetViewId = null;
            this.nuevaRelacionAction.ToolTip = null;
            this.nuevaRelacionAction.TypeOfView = null;
            this.nuevaRelacionAction.Execute += new SimpleActionExecuteEventHandler(this.nuevaRelacionAction_Execute);
        }

        #endregion
    }
}