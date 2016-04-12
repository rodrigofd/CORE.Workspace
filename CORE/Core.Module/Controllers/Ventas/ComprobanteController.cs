using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;
using FDIT.Core.Ventas;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.Controllers.Ventas
{
    public class ComprobanteController : ComprobanteBaseController
    {
        private SingleChoiceAction _comprobanteExportAndMailReportAction;

        public ComprobanteController()
        {
            TargetObjectType = typeof (Comprobante);
        }

        public override string RutaExpComprobantes => Identificadores.GetInstance(ObjectSpace).RutaExpComprobantes;

        protected override void CreateActions()
        {
            base.CreateActions();

            ComprobanteExportReportAction.Id = "VentasComprobanteExportReportAction";
            ComprobanteConfirmarAction.Id = "VentasComprobanteConfirmarAction";
            ComprobanteDuplicarAction.Id = "VentasComprobanteDuplicarAction";

            _comprobanteExportAndMailReportAction = new SingleChoiceAction(this, "ComprobanteExportAndMailReportAction",
                PredefinedCategory.Reports);
            _comprobanteExportAndMailReportAction.Caption = "Exportar y mail a cliente";
            _comprobanteExportAndMailReportAction.ToolTip = "Exportar y adjuntar por mail al cliente";
            _comprobanteExportAndMailReportAction.Execute += ComprobanteExportAndMailReportAction_Execute;
            _comprobanteExportAndMailReportAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            _comprobanteExportAndMailReportAction.SelectionDependencyType =
                SelectionDependencyType.RequireMultipleObjects;
            _comprobanteExportAndMailReportAction.ImageName = "printer";
        }

        //TODO: crear motor de expansion de variables por campos de un objeto
        protected override string ExpandFilename(object obj, string rutaBase)
        {
            var comp = (Comprobante) obj;

            rutaBase = rutaBase.Replace("{Nombre}", comp.Nombre);
            rutaBase = rutaBase.Replace("{Tipo}", comp.Tipo.Descripcion);
            rutaBase = rutaBase.Replace("{Tipo.Codigo}", comp.Tipo.Codigo);
            rutaBase = rutaBase.Replace("{Sector}", comp.Sector.ToString("0000"));
            rutaBase = rutaBase.Replace("{Numero}", comp.Numero.ToString("00000000"));
            rutaBase = rutaBase.Replace("{Empresa}", comp.Empresa.Descripcion);
            rutaBase = rutaBase.Replace("{Fecha:yyyy}", comp.Fecha.ToString("yyyy"));
            rutaBase = rutaBase.Replace("{Fecha:MM}", comp.Fecha.ToString("MM"));
            rutaBase = rutaBase.Replace("{Fecha:MMM}", comp.Fecha.ToString("MMM"));
            rutaBase = rutaBase.Replace("{Fecha:dd}", comp.Fecha.ToString("dd"));
            rutaBase = rutaBase.Replace("{Fecha}", comp.Fecha.ToString("yyyy.MM.dd"));

            var ruta = Path.GetFileNameWithoutExtension(rutaBase);
            while (ruta.EndsWith("."))
                ruta = ruta.Substring(0, ruta.Length - 1);

            ruta = Path.GetInvalidPathChars().Aggregate(ruta, (current, c) => current.Replace(c, '_'));

            ruta = Path.GetDirectoryName(rutaBase) + @"\" + ruta + Path.GetExtension(rutaBase);

            return ruta;
        }

        protected override void GetAvailableReports()
        {
            base.GetAvailableReports();

            _comprobanteExportAndMailReportAction.Items.Clear();
            _comprobanteExportAndMailReportAction.Items.AddRange(ComprobanteExportReportAction.Items);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void ComprobanteExportAndMailReportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count == 0)
                return;

            var rutaBase = RutaExpComprobantes;
            if (string.IsNullOrEmpty(rutaBase))
            {
                throw new UserFriendlyException("No está definida la ruta para exportación de comprobantes");
            }

            var cuentaEmail = Identificadores.GetInstance(ObjectSpace).CuentaEmailFacturacion;
            if (cuentaEmail == null)
            {
                throw new UserFriendlyException("No está definida la cuenta de mail para el envío de comprobantes");
            }

            var plantillaMensaje = Identificadores.GetInstance(ObjectSpace).PlantillaMensajeFacturacion;
            var plantillaContenido = plantillaMensaje != null ? plantillaMensaje.Contenido : "";

            foreach (Comprobante obj in e.SelectedObjects)
            {
                var criteria =
                    (CriteriaOperator)
                        new BinaryOperator(View.ObjectTypeInfo.KeyMember.Name, ObjectSpace.GetKeyValue(obj));

                var reportData = ObjectSpace.GetObject((IReportData) e.SelectedChoiceActionItem.Data);
                var rep = (XafReport) reportData.LoadReport(ObjectSpace);
                rep.SetFilteringObject(new LocalizedCriteriaWrapper(View.ObjectTypeInfo.Type, criteria));

                var ruta = this.ExpandFilename(obj, rutaBase);
                var fileName = Path.GetFileName(ruta);
                var ms = new MemoryStream();
                rep.ExportToPdf(ms);

                var mail = new MailMessage();
                mail.Attachments.Add(new Attachment(ms, fileName, "application/pdf"));

                var cliente = ObjectSpace.FindObject<Cliente>(new BinaryOperator("Persona.Oid", obj.Destinatario.Oid));
                if (cliente == null) return;

                var destinatario = cliente.EmailEnvioFacturacion;
                if (destinatario == null)
                {
                    continue;
                }

                mail.From = new MailAddress(cuentaEmail.DireccionEmail, cuentaEmail.NombreMostrar);
                mail.To.Add(new MailAddress(destinatario.Valor, cliente.Persona.Nombre));
                //TODO
                mail.CC.Add(new MailAddress("info@fd-it.com", "Info (Facturacion)"));
                mail.Subject = fileName;
                mail.Body = plantillaContenido;
                mail.IsBodyHtml = true;

                cuentaEmail.SendMail(mail);
            }
        }

        public override void DuplicarComprobante(ComprobanteBase comprobante)
        {
            base.DuplicarComprobante(comprobante);

            var comprobanteVentas = (Comprobante) comprobante;

            comprobanteVentas.AutorizadaCodigo = null;
            comprobanteVentas.AutorizadaCodigoFecVto = null;
            comprobanteVentas.AutorizadaFecha = null;
            comprobanteVentas.AutorizadaNotas = null;
            comprobanteVentas.AutorizadaResultado = null;

            comprobanteVentas.Fecha = (DateTime) (comprobanteVentas.Vencimiento = DateTime.Today);
            comprobanteVentas.InicioPrestacion = new DateTime(comprobanteVentas.Fecha.Year, comprobanteVentas.Fecha.Month, 1);
            comprobanteVentas.FinPrestacion = new DateTime(comprobanteVentas.Fecha.Year, comprobanteVentas.Fecha.Month,
                DateTime.DaysInMonth(comprobanteVentas.Fecha.Year, comprobanteVentas.Fecha.Month));

            comprobanteVentas.AplicarImpuestos();
            comprobanteVentas.GenerarCuotas();
        }
    }
}