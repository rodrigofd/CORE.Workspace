using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.XtraPrinting;
using FDIT.Core.Gestion;
using FDIT.Core.Modelo.Sistema;

namespace FDIT.Core.Controllers.Gestion
{
    public abstract class ComprobanteBaseController : ViewController
    {
        protected IContainer components;

        protected SimpleAction ComprobanteConfirmarAction;
        protected SimpleAction ComprobanteDuplicarAction;
        protected SingleChoiceAction ComprobanteExportReportAction;

        protected ComprobanteBaseController()
        {
            InitializeComponent();
        }

        public abstract string RutaExpComprobantes { get; }

        public virtual event ComprobanteAutorizandoEvent ComprobanteAutorizando;

        protected virtual void InitializeComponent()
        {
            components = new Container();
            CreateActions();
            // 
            // ComprobanteController
            // 
            TargetObjectType = typeof (ComprobanteBase);
            TargetViewNesting = Nesting.Root;

            RegisterActions(components);
        }

        protected void OnComprobanteAutorizando(object sender, ComprobanteAutorizandoArgs args)
        {
            ComprobanteAutorizando?.Invoke(sender, args);
        }

        protected virtual void CreateActions()
        {
            // 
            // exportReportAction
            // 
            ComprobanteExportReportAction = new SingleChoiceAction(this, GetType() + "ExportReportAction", PredefinedCategory.Reports);
            ComprobanteExportReportAction.Caption = "Exportar PDF";
            ComprobanteExportReportAction.ToolTip = "Exportar comprobantes seleccionados al disco";
            ComprobanteExportReportAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            ComprobanteExportReportAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            ComprobanteExportReportAction.ImageName = "printer";
            ComprobanteExportReportAction.Execute += ComprobanteExportReportAction_Execute;
            // 
            // confirmarAction
            // 
            ComprobanteConfirmarAction = new SimpleAction(components);
            ComprobanteConfirmarAction.Caption = "Confirmar comprobante";
            ComprobanteConfirmarAction.ConfirmationMessage = "Confirma?";
            ComprobanteConfirmarAction.Id = GetType() + "ConfirmarAction";
            ComprobanteConfirmarAction.ImageName = "document_mark_as_final";
            ComprobanteConfirmarAction.Shortcut = null;
            ComprobanteConfirmarAction.Tag = null;
            ComprobanteConfirmarAction.TargetObjectsCriteria = null;
            ComprobanteConfirmarAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            ComprobanteConfirmarAction.TargetViewId = null;
            ComprobanteConfirmarAction.ToolTip = null;
            ComprobanteConfirmarAction.TypeOfView = null;
            ComprobanteConfirmarAction.Execute += ComprobanteConfirmarAction_Execute;
            // 
            // duplicarAction
            // 
            ComprobanteDuplicarAction = new SimpleAction(components);
            ComprobanteDuplicarAction.Caption = "Duplicar";
            ComprobanteDuplicarAction.Id = GetType() + "DuplicarAction";
            ComprobanteDuplicarAction.ImageName = "page_copy";
            ComprobanteDuplicarAction.Shortcut = null;
            ComprobanteDuplicarAction.Tag = null;
            ComprobanteDuplicarAction.TargetObjectsCriteria = null;
            ComprobanteDuplicarAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            ComprobanteDuplicarAction.TargetViewId = null;
            ComprobanteDuplicarAction.ToolTip = null;
            ComprobanteDuplicarAction.TypeOfView = null;
            ComprobanteDuplicarAction.Execute += ComprobanteDuplicarAction_Execute;
        }

        protected virtual void GetAvailableReports()
        {
            var reportDataList = InplaceReportCacheController.GetReportDataList(Frame, View.ObjectTypeInfo.Type);
            var list = reportDataList.Select(reportData => new ChoiceActionItem(reportData.ReportName, reportData)
            {
                ImageName = "printer"
            }).ToList();
            list.Sort((left, right) => Comparer<string>.Default.Compare(left.Caption, right.Caption));
            ComprobanteExportReportAction.Items.Clear();
            ComprobanteExportReportAction.Items.AddRange(list);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var act = Frame.GetController<PrintSelectionBaseController>().ShowInReportAction;

            act.Caption = "Imprimir comprobante";
            act.ImageName = "printer";

            GetAvailableReports();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void ComprobanteConfirmarAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var selectedComprobantes = View.SelectedObjects;

            if (selectedComprobantes.Count == 0)
                throw new UserFriendlyException("Seleccione al menos un comprobante");

            var mensajes = "";

            foreach (ComprobanteBase comprobante in selectedComprobantes)
            {
                try
                {
                    ConfirmarComprobante(comprobante);
                }
                catch (Exception exc)
                {
                    mensajes += "\n" + comprobante.Descripcion + ": " + exc.Message;
                }
            }

            ObjectSpace.CommitChanges();

            if (mensajes != "")
                throw new UserFriendlyException("Errores en la confirmación de uno o más comprobantes:" + mensajes);

            View.Refresh();
        }

        public virtual void ConfirmarComprobante(ComprobanteBase comprobante)
        {
            //Validaciones
            if (comprobante.Estado == ComprobanteEstado.Confirmado)
                throw new UserFriendlyException("Comprobante ya confirmado");

            if (comprobante.Estado == ComprobanteEstado.Anulado)
                throw new UserFriendlyException("Comprobante anulado");

            ConfirmarComprobanteValidar(comprobante);

            if (comprobante.Talonario != null)
            {
                comprobante.NumerarAutomatico();
                comprobante.Talonario.UltimoNumero = comprobante.Numero;
            }

            var args = new ComprobanteAutorizandoArgs {Autorizado = true, Comprobante = comprobante};
            OnComprobanteAutorizando(comprobante, args);

            if (!args.Autorizado) return;

            comprobante.Estado = ComprobanteEstado.Confirmado;

            ConfirmarComprobanteAfter(comprobante);
        }

        protected virtual void ConfirmarComprobanteValidar(ComprobanteBase comprobante)
        {
        }

        protected virtual void ConfirmarComprobanteAfter(ComprobanteBase comprobante)
        {
        }

        private void ComprobanteDuplicarAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var selectedComprobantes = View.SelectedObjects;

            if (selectedComprobantes.Count == 0)
                throw new UserFriendlyException("Seleccione al menos un comprobante");

            var cloner = new BasicObjectCloner(true);

            foreach (ComprobanteBase selected in selectedComprobantes)
            {
                selected.SetIgnoreOnChangedRecursive(true);
                var copy = (ComprobanteBase) cloner.CloneTo(selected, selected.GetType());

                DuplicarComprobante(copy);
            }

            ObjectSpace.CommitChanges();
            View.Refresh();
        }

        public virtual void DuplicarComprobante(ComprobanteBase comprobante)
        {
            comprobante.Estado = ComprobanteEstado.Pendiente;
            comprobante.Fecha = DateTime.Today;
            comprobante.Numero = 0;
        }

        private void ComprobanteExportReportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count == 0)
                return;

            var rutaBase = RutaExpComprobantes;

            if (string.IsNullOrEmpty(rutaBase))
                throw new UserFriendlyException("No está definida la ruta para exportación de comprobantes");

            foreach (ComprobanteBase obj in e.SelectedObjects)
            {
                var criteria =
                    (CriteriaOperator)
                        new BinaryOperator(View.ObjectTypeInfo.KeyMember.Name, ObjectSpace.GetKeyValue(obj));

                var reportData = ObjectSpace.GetObject((IReportData) e.SelectedChoiceActionItem.Data);
                var rep = (XafReport) reportData.LoadReport(ObjectSpace);
                rep.SetFilteringObject(new LocalizedCriteriaWrapper(View.ObjectTypeInfo.Type, criteria));

                var ruta = ExpandFilename(obj, rutaBase);
                var directorio = Path.GetDirectoryName(ruta) ?? "";
                if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);
                rep.ExportToPdf(ruta, new PdfExportOptions());
            }
        }

        protected virtual string ExpandFilename(object obj, string filename)
        {
            return filename;
        }
    }
}