using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;
using FDIT.Core.Seguridad;
using FDIT.Core.Ventas;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.Controllers.Ventas
{
    public class ReciboController : PagoController
    {
        public ReciboController()
        {
            TargetObjectType = typeof (Recibo);
        }

        protected override void GenerarPagoAnticipo(Dictionary<int, decimal> saldos)
        {
            //TODO: dependencia fuerte a XPO
            var session = ((XPObjectSpace) ObjectSpace).Session;
            var identificadores = Identificadores.GetInstance(ObjectSpace);

            var anticipoComprobanteTipo = identificadores.AnticipoComprobanteTipo;
            var anticipoConcepto = identificadores.AnticipoConcepto;
            var anticipoCuenta = identificadores.AnticipoCuenta;
            var empresaActual = CoreAppLogonParameters.Instance.EmpresaActual(ObjectSpace);
            var especiePredeterminada = Core.Fondos.Identificadores.GetInstance(ObjectSpace).EspeciePredeterminada;

            if (anticipoComprobanteTipo == null || anticipoConcepto == null || anticipoCuenta == null ||
                especiePredeterminada == null)
            {
                throw new UserFriendlyException(
                    "Faltan valores de configuración para la generación de anticipos. Por favor revise.");
            }

            var compAnticipo = ObjectSpace.CreateObject<Comprobante>();

            var recibo = (Recibo) View.CurrentObject;
            var destinatario = recibo.Destinatario;

            var ultimoNumero = session.Evaluate<Comprobante>(CriteriaOperator.Parse("MAX(Numero)"),
                CriteriaOperator.Parse("Destinatario = ? AND Tipo = ? AND Sector = 1", destinatario,
                    anticipoComprobanteTipo));
            ultimoNumero = ultimoNumero != null ? (int) ultimoNumero + 1 : 1;

            compAnticipo.Empresa = empresaActual;
            compAnticipo.Destinatario = destinatario;
            compAnticipo.Originante = empresaActual.Persona;
            compAnticipo.Tipo = anticipoComprobanteTipo;
            compAnticipo.Fecha = recibo.Fecha;

            compAnticipo.Sector = 1;
            compAnticipo.Numero = (int) ultimoNumero;

            compAnticipo.Cuenta = anticipoCuenta;
            compAnticipo.Especie = especiePredeterminada;
            compAnticipo.Cambio = 1;

            decimal valorTotalLocalAnticipo = 0;

            foreach (var saldo in saldos)
            {
                if (saldo.Key == especiePredeterminada.Oid)
                {
                    valorTotalLocalAnticipo += saldo.Value;
                }
                else
                {
                    var itm = recibo.Items.FirstOrDefault(item => item.Especie.Moneda.Oid == saldo.Key);
                    if (itm == null) //nunca deberia pasar esto
                        throw new UserFriendlyException(
                            "No se pudo convertir el saldo del pago, a un anticipo en moneda local.");

                    valorTotalLocalAnticipo += saldo.Value*itm.Cambio;
                }
            }

            var comprobanteItem = ObjectSpace.CreateObject<ComprobanteItem>();
            comprobanteItem.Cantidad = 1;
            comprobanteItem.Concepto = anticipoConcepto;
            comprobanteItem.PrecioUnitario = valorTotalLocalAnticipo*-1;
            // el calculo de saldos fue aplicaciones - valores. Por lo tanto el excedente de valores está negativo, así que invertir signo

            compAnticipo.Items.Add(comprobanteItem);

            recibo.ComprobanteAnticipo = compAnticipo;
        }
    }
}