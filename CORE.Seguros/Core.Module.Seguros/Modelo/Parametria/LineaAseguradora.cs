using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    //[DefaultProperty( "Descripcion" )]
    [System.ComponentModel.DisplayName("Linea Aseguradora")]
    [Persistent("seguros.LineaAseguradora")]
    public class LineaAseguradora : BasicObject
    {
        private Aseguradora fAseguradora;
        private LineaNegocio fLinea;

        public LineaAseguradora(Session session)
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

        [RuleRequiredField]
        public LineaNegocio Linea
        {
            get { return fLinea; }
            set { SetPropertyValue("Linea", ref fLinea, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<NegocioRiesgo> Riesgos
        {
            get { return GetCollection<NegocioRiesgo>("Riesgos"); }
        }
    }
}