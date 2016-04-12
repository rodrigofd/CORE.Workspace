using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using FDIT.Core.Controllers.Fondos;
using FDIT.Core.Gestion;

namespace FDIT.Core.Controllers.Gestion
{
    public abstract class PagoController : ComprobanteController
    {
        protected override void ConfirmarComprobanteValidar(ComprobanteBase comprobante)
        {
            var pago = (Pago) View.CurrentObject;

            var saldosAplicaciones = pago.CalcularSaldosAplicaciones();
            var saldosValores = pago.CalcularSaldosValores();

            if (pago.Aplicaciones.Count == 0 && saldosValores.Cast<ViewRecord>().All(vr => (decimal) vr["Importe"] == 0))
                throw new UserFriendlyException(
                    "Debe realizar alguna aplicación, o bien ingresar valores por un monto mayor a cero.");

            if (saldosAplicaciones.Cast<ViewRecord>().Any(vr => (decimal) vr["Importe"] < 0))
                throw new UserFriendlyException(
                    "No puede aplicar comprobantes de crédito por un valor superior a los comprobantes aplicados.");

            var saldos = pago.CalcularSaldoPago(saldosAplicaciones, saldosValores);

            //Cualquier saldo de moneda mayor a cero, significa mas aplicaciones que valores
            if (saldos.Any(saldo => saldo.Value > 0))
                throw new UserFriendlyException("No ingresó suficientes valores para las aplicaciones ingresadas.");

            //si hay saldos menores a cero, significan mas valores que aplicaciones => Valido => Generación de OPA (ver ConfirmarComprobanteAfter)
        }

        protected override void ConfirmarComprobanteAfter(ComprobanteBase comprobante)
        {
            base.ConfirmarComprobanteAfter(comprobante);

            var pago = (Pago) View.CurrentObject;

            var saldos = pago.CalcularSaldoPago(pago.CalcularSaldosAplicaciones(), pago.CalcularSaldosValores());

            GenerarPagoAnticipo(saldos);
        }

        protected abstract void GenerarPagoAnticipo(Dictionary<int, decimal> saldos);
    }
}