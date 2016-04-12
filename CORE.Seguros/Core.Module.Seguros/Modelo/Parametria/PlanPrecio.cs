using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    //[DefaultProperty( "Nombre" )]
    [System.ComponentModel.DisplayName("Precio por plan")]
    [Persistent("seguros.PlanPrecio")]
    public class PlanPrecio : BasicObject
    {
        private decimal fIva1Tasa;
        private ListaDePrecios fLista;
        private PlanCobertura fPlan;
        private decimal fPremio;
        private decimal fPrimaGravada;
        private decimal fPrimaNoGravada;

        public PlanPrecio(Session session)
            : base(session)
        {
        }

        [Association]
        public PlanCobertura Plan
        {
            get { return fPlan; }
            set { SetPropertyValue("Plan", ref fPlan, value); }
        }

        public ListaDePrecios Lista
        {
            get { return fLista; }
            set { SetPropertyValue("Lista", ref fLista, value); }
        }

        public decimal PrimaGravada
        {
            get { return fPrimaGravada; }
            set { SetPropertyValue<decimal>("PrimaGravada", ref fPrimaGravada, value); }
        }

        public decimal PrimaNoGravada
        {
            get { return fPrimaNoGravada; }
            set { SetPropertyValue<decimal>("PrimaNoGravada", ref fPrimaNoGravada, value); }
        }

        public decimal Iva1Tasa
        {
            get { return fIva1Tasa; }
            set { SetPropertyValue<decimal>("Iva1Tasa", ref fIva1Tasa, value); }
        }

        public decimal Premio
        {
            get { return fPremio; }
            set { SetPropertyValue<decimal>("Premio", ref fPremio, value); }
        }
    }
}