using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Plan de pago")]
    [Persistent("seguros.PlanDePago")]
    public class PlanDePago : BasicObject
    {
        private decimal fAnticipo;
        private Aseguradora fAseguradora;
        private int fCantidadCuotas;
        private string fCodigo;
        private string fNombre;
        private int fOrden;

        public PlanDePago(Session session)
            : base(session)
        {
        }

        [RuleRequiredField]
        [Association]
        public Aseguradora Aseguradora
        {
            get { return fAseguradora; }
            set { SetPropertyValue("Aseguradora", ref fAseguradora, value); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Size(100)]
        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public decimal Anticipo
        {
            get { return fAnticipo; }
            set { SetPropertyValue<decimal>("Anticipo", ref fAnticipo, value); }
        }

        public int CantidadCuotas
        {
            get { return fCantidadCuotas; }
            set { SetPropertyValue<int>("CantidadCuotas", ref fCantidadCuotas, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}