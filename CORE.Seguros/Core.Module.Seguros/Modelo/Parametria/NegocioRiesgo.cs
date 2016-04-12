using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Riesgo por negocio")]
    [Persistent("seguros.NegocioRiesgo")]
    public class NegocioRiesgo : BasicObject
    {
        private LineaAseguradora fLineaAseguradora;
        private Subramo fSubramo;

        public NegocioRiesgo(Session session)
            : base(session)
        {
        }

        [PersistentAlias("LineaAseguradora + '-' + Subramo")]
        public string Descripcion
        {
            get { return (string) EvaluateAlias("Descripcion"); }
        }

        [Association]
        public LineaAseguradora LineaAseguradora
        {
            get { return fLineaAseguradora; }
            set { SetPropertyValue("LineaAseguradora", ref fLineaAseguradora, value); }
        }

        [RuleRequiredField]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Subramo Subramo
        {
            get { return fSubramo; }
            set { SetPropertyValue("Subramo", ref fSubramo, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<PlanCobertura> Planes
        {
            get { return GetCollection<PlanCobertura>("Planes"); }
        }
    }
}