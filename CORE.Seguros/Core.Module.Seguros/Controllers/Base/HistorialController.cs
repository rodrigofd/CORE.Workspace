using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
    public abstract class HistorialController<TObjetoConHistorial, TMovimiento> : ViewController<ListView>
        where TMovimiento : IMovimientoHistorial<TObjetoConHistorial, TMovimiento>
        where TObjetoConHistorial : IObjetoConHistorial<TMovimiento>
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        protected string LookupViewId = "";
        protected SimpleAction MovimientoBajaAction;
        protected SimpleAction MovimientoModificacionAction;

        public HistorialController()
        {
            LookupViewId = typeof (TMovimiento).FullName + "_LookupListView";

            InitializeComponent();

            var name = typeof (TMovimiento).Name;

            MovimientoModificacionAction.Id = name + MovimientoModificacionAction.Id;
            MovimientoBajaAction.Id = name + MovimientoBajaAction.Id;

            RegisterActions(components);

            TargetViewNesting = Nesting.Nested;
            TargetObjectType = typeof (TMovimiento);
        }

        protected abstract CriteriaOperator PopupCriteria { get; }

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

        protected override void OnActivated()
        {
            base.OnActivated();

            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated += NewObjectViewController_ObjectCreated;
        }

        private void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if (!(e.CreatedObject is TMovimiento)) return;

            var documentoItem = (TMovimiento) e.CreatedObject;

            documentoItem.TipoMovimiento = TipoMovimiento.Alta;
            InicializarValores(e.ObjectSpace, documentoItem);
        }

        protected override void OnDeactivated()
        {
            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated -= NewObjectViewController_ObjectCreated;

            base.OnDeactivated();
        }

        private void ModificarItemAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            this.ShowPopupListView(ObjectSpace,
                e,
                typeof (TObjetoConHistorial),
                PopupCriteria,
                LookupViewId,
                (s, e1) => prepararNuevoMovimiento(e1, TipoMovimiento.Modificacion),
                selectionDependencyType: SelectionDependencyType.RequireSingleObject);
        }

        private void BajaItemAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            this.ShowPopupListView(ObjectSpace,
                e,
                typeof (TObjetoConHistorial),
                PopupCriteria,
                LookupViewId,
                (s, e1) => prepararNuevoMovimiento(e1, TipoMovimiento.Baja),
                selectionDependencyType: SelectionDependencyType.RequireSingleObject);
        }

        private void prepararNuevoMovimiento(DialogControllerAcceptingEventArgs e, TipoMovimiento tipoMovimiento)
        {
            var nuevoMovimiento = ObjectSpace.CreateObject<TMovimiento>();
            nuevoMovimiento.TipoMovimiento = tipoMovimiento;
            nuevoMovimiento.ObjetoConHistorial = (TObjetoConHistorial) e.AcceptActionArgs.SelectedObjects[0];
            InicializarValores(ObjectSpace, nuevoMovimiento);

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(ObjectSpace, nuevoMovimiento, false);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.PopupWindow;

            var dc = Application.CreateController<DialogController>();
            dc.SaveOnAccept = false;
            dc.Accepting += (s, e1) => View.CollectionSource.Add((TMovimiento) e1.AcceptActionArgs.CurrentObject);
            dc.Cancelling += (s, e1) => ((TMovimiento) ((DialogController) s).Frame.View.CurrentObject).Delete();

            e.ShowViewParameters.Controllers.Add(dc);
        }

        protected virtual void InicializarValores(IObjectSpace objectSpace, TMovimiento movimiento)
        {
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MovimientoModificacionAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.MovimientoBajaAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ModificarItemAction
            // 
            this.MovimientoModificacionAction.Caption = "Modificar existente";
            this.MovimientoModificacionAction.ConfirmationMessage = null;
            this.MovimientoModificacionAction.Id = "MovimientoModificacionAction";
            this.MovimientoModificacionAction.ImageName = "table_edit.png";
            this.MovimientoModificacionAction.Shortcut = null;
            this.MovimientoModificacionAction.Tag = null;
            this.MovimientoModificacionAction.TargetObjectsCriteria = null;
            this.MovimientoModificacionAction.TargetViewId = null;
            this.MovimientoModificacionAction.ToolTip = null;
            this.MovimientoModificacionAction.TypeOfView = null;
            this.MovimientoModificacionAction.Execute +=
                new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ModificarItemAction_Execute);
            // 
            // BajaItemAction
            // 
            this.MovimientoBajaAction.Caption = "Baja de item";
            this.MovimientoBajaAction.ConfirmationMessage = null;
            this.MovimientoBajaAction.Id = "MovimientoBajaAction";
            this.MovimientoBajaAction.ImageName = "table_delete.png";
            this.MovimientoBajaAction.Shortcut = null;
            this.MovimientoBajaAction.Tag = null;
            this.MovimientoBajaAction.TargetObjectsCriteria = null;
            this.MovimientoBajaAction.TargetViewId = null;
            this.MovimientoBajaAction.ToolTip = null;
            this.MovimientoBajaAction.TypeOfView = null;
            this.MovimientoBajaAction.Execute +=
                new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BajaItemAction_Execute);
        }

        #endregion
    }
}