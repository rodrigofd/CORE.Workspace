using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Plan de cobertura base")]
    [Persistent("seguros.PlanCobertura")]
    public class PlanCobertura : BasicObject
    {
        private string fBeneficios;
        private string fCodigo;
        private string fDeducible;
        private string fDetalle;
        private NegocioRiesgo fNegocioRiesgo;
        private string fNombre;
        private int fOrden;

        public PlanCobertura(Session session)
            : base(session)
        {
        }

        [Association]
        public NegocioRiesgo NegocioRiesgo
        {
            get { return fNegocioRiesgo; }
            set { SetPropertyValue("NegocioRiesgo", ref fNegocioRiesgo, value); }
        }

        [Size(30)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Detalle
        {
            get { return fDetalle; }
            set { SetPropertyValue("Detalle", ref fDetalle, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Deducible
        {
            get { return fDeducible; }
            set { SetPropertyValue("Deducible", ref fDeducible, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Beneficios
        {
            get { return fBeneficios; }
            set { SetPropertyValue("Beneficios", ref fBeneficios, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<PlanCoberturaDetalle> Detalles
        {
            get { return GetCollection<PlanCoberturaDetalle>("Detalles"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<PlanPrecio> Precios
        {
            get { return GetCollection<PlanPrecio>("Precios"); }
        }
    }
}