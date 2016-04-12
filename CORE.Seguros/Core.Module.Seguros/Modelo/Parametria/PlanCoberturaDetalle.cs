using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Detalle de plan de cobertura")]
    [Persistent("seguros.PlanCoberturaDetalle")]
    public class PlanCoberturaDetalle : DetalleBase
    {
        private PlanCobertura fPlanCobertura;

        public PlanCoberturaDetalle(Session session)
            : base(session)
        {
        }

        [Association]
        public PlanCobertura PlanCobertura
        {
            get { return fPlanCobertura; }
            set { SetPropertyValue("PlanCobertura", ref fPlanCobertura, value); }
        }

        public override Persona AseguradoraAsociada
        {
            get
            {
                return PlanCobertura == null ? null : PlanCobertura.NegocioRiesgo.LineaAseguradora.Aseguradora.Persona;
            }
        }

        public override Ramo RamoAsociado
        {
            get { return PlanCobertura == null ? null : PlanCobertura.NegocioRiesgo.Subramo.Ramo; }
        }

        public override Subramo SubramoAsociado
        {
            get { return PlanCobertura == null ? null : PlanCobertura.NegocioRiesgo.Subramo; }
        }
    }
}