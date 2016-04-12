using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Personas;

namespace FDIT.Core.Controllers.Personas
{
    public class CPAController : ViewController
    {
        private PopupWindowShowAction buscarCPAAction;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public CPAController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (PersonaDireccion);
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

        private void buscarCPAAction_Execute(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var dir = ((PersonaDireccion) View.CurrentObject).Direccion;
            var loc = dir.Localidad != null ? dir.Localidad.Nombre : dir.LocalidadOtra;

            if (dir.Provincia == null) throw new UserFriendlyException("Debe indicar primero la provincia.");

            var prov = dir.Provincia.Codigo;

            IList<CPA.LocalidadResultItem> locs;

            if (prov == "C")
            {
                locs = new List<CPA.LocalidadResultItem>
                {
                    new CPA.LocalidadResultItem
                    {
                        Codigo = 5001,
                        CodigoProvincia = "C",
                        Nombre = "Ciudad Autonoma de Buenos Aires"
                    }
                };
            }
            else
                locs = CPA.ObtenerLocalidades(prov, loc);

            var collectionSource = new CollectionSource(ObjectSpace, typeof (CPA.LocalidadResultItem));
            foreach (var loci in locs)
                collectionSource.Add(loci);

            var view = Application.CreateListView(Application.GetListViewId(typeof (CPA.LocalidadResultItem)),
                collectionSource, false);
            view.Editor.AllowEdit = false;
            e.View = view;

            e.DialogController.SaveOnAccept = false;
            e.DialogController.Accepting += DialogController_Execute;
        }

        private void DialogController_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            var dir = ((PersonaDireccion) View.CurrentObject).Direccion;

            if (e.AcceptActionArgs.SelectedObjects.Count < 1) return;
            var selectedLoc = (CPA.LocalidadResultItem) e.AcceptActionArgs.SelectedObjects[0];

            var cps = CPA.ObtenerCPA(selectedLoc.Codigo, dir.Calle, dir.Numero);

            var collectionSource = new CollectionSource(ObjectSpace, typeof (CPA.CPAResultItem));
            foreach (var cp in cps)
                collectionSource.Add(cp);

            var view = Application.CreateListView(Application.GetListViewId(typeof (CPA.CPAResultItem)), collectionSource,
                false);
            view.Editor.AllowEdit = false;
            e.AcceptActionArgs.ShowViewParameters.CreatedView = view;
            e.AcceptActionArgs.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.AcceptActionArgs.ShowViewParameters.Context = TemplateContext.PopupWindow;
            var dc = Application.CreateController<DialogController>();
            e.AcceptActionArgs.ShowViewParameters.Controllers.Add(dc);

            dc.Tag = selectedLoc;
            dc.SaveOnAccept = false;
            dc.Accepting += CPAResultItem_DialogController_Accepting;
            //Truco para forzar la apertura de la view; por lo visto es la unica forma de encadenar dos Popup views
            Application.ShowViewStrategy.ShowView(e.AcceptActionArgs.ShowViewParameters,
                new ShowViewSource(Frame, e.AcceptActionArgs.Action));
        }

        private void CPAResultItem_DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var dir = ((PersonaDireccion) View.CurrentObject).Direccion;

            if (e.AcceptActionArgs.SelectedObjects.Count < 1) return;
            var selectedCPA = (CPA.CPAResultItem) e.AcceptActionArgs.SelectedObjects[0];
            var selectedLoc = (CPA.LocalidadResultItem) ((DialogController) sender).Tag;

            dir.CodigoPostal = selectedCPA.CPA;
            dir.Calle = selectedCPA.Calle;
            dir.Localidad = null;
            dir.LocalidadOtra = selectedLoc.Nombre;
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buscarCPAAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // buscarCPAAction
            // 
            this.buscarCPAAction.Caption = "Buscar CPA";
            this.buscarCPAAction.Category = "RecordEdit";
            this.buscarCPAAction.ConfirmationMessage = null;
            this.buscarCPAAction.Id = "buscarCPAAction";
            this.buscarCPAAction.ImageName = "postage-stamp";
            this.buscarCPAAction.Shortcut = null;
            this.buscarCPAAction.Tag = null;
            this.buscarCPAAction.TargetObjectsCriteria = null;
            this.buscarCPAAction.TargetViewId = null;
            this.buscarCPAAction.ToolTip = null;
            this.buscarCPAAction.TypeOfView = null;
            this.buscarCPAAction.CustomizePopupWindowParams +=
                new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.buscarCPAAction_Execute);
        }

        #endregion
    }
}