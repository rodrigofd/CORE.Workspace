using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.StoredProcedures;

namespace FDIT.Core.Controllers
{
    public class ConsultaStoredProcedureController : ViewController
    {
        private Container components;
        private SimpleAction ejecutarConsultaAction;

        public ConsultaStoredProcedureController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (IConsultaStoredProcedure);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var modificationsController = Frame.GetController<ModificationsController>();
            modificationsController.CancelAction.Active["IsNonPersistent"] =
                modificationsController.SaveAction.Active["IsNonPersistent"] =
                    modificationsController.SaveAndCloseAction.Active["IsNonPersistent"] =
                        modificationsController.SaveAndNewAction.Active["IsNonPersistent"] = false;

            var deleteObjectsViewController = Frame.GetController<DeleteObjectsViewController>();
            deleteObjectsViewController.DeleteAction.Active["IsNonPersistent"] = false;
        }

        protected override void OnDeactivated()
        {
            var modificationsController = Frame.GetController<ModificationsController>();
            modificationsController.CancelAction.Active.RemoveItem("IsNonPersistent");
            modificationsController.SaveAction.Active.RemoveItem("IsNonPersistent");
            modificationsController.SaveAndCloseAction.Active.RemoveItem("IsNonPersistent");
            modificationsController.SaveAndNewAction.Active.RemoveItem("IsNonPersistent");

            var deleteObjectsViewController = Frame.GetController<DeleteObjectsViewController>();
            deleteObjectsViewController.DeleteAction.Active.RemoveItem("IsNonPersistent");

            base.OnDeactivated();
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

        private void EjecutarConsultaActionOnExecute(object sender, SimpleActionExecuteEventArgs e)
        {
            var objectSpace = (XPObjectSpace) Application.CreateObjectSpace();

            var objetoParametros = (IConsultaStoredProcedure) View.CurrentObject;
            var xpClassInfo =
                XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary.QueryClassInfo(objetoParametros.ClaseResultados);

            var objectsFromSproc = objectSpace.Session.GetObjectsFromSproc(xpClassInfo,
                objetoParametros.NombreStoredProcedure, objetoParametros.Parametros);
            var collectionSource = new CollectionSource(objectSpace, objetoParametros.ClaseResultados);
            foreach (var item in objectsFromSproc)
                collectionSource.Add(item);

            var view = Application.CreateListView(Application.Model.BOModel[xpClassInfo.FullName].DefaultListView,
                collectionSource, true);
            view.Editor.AllowEdit = false;
            Application.MainWindow.SetView(view);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
            e.ShowViewParameters.Context = TemplateContext.ApplicationWindow;
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.ejecutarConsultaAction = new SimpleAction(this.components);
            // 
            // ejecutarConsultaAction
            // 
            this.ejecutarConsultaAction.Caption = "Ejecutar";
            this.ejecutarConsultaAction.Category = "RecordEdit";
            this.ejecutarConsultaAction.ConfirmationMessage = null;
            this.ejecutarConsultaAction.Id = "ejecutarConsultaAction";
            this.ejecutarConsultaAction.ImageName = "";
            this.ejecutarConsultaAction.Shortcut = null;
            this.ejecutarConsultaAction.Tag = null;
            this.ejecutarConsultaAction.TargetObjectsCriteria = null;
            this.ejecutarConsultaAction.TargetViewId = null;
            this.ejecutarConsultaAction.ToolTip = null;
            this.ejecutarConsultaAction.TypeOfView = null;
            this.ejecutarConsultaAction.Execute += EjecutarConsultaActionOnExecute;
        }

        #endregion
    }
}