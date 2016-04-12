using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Tipo de Importe")]
    [Persistent("seguros.TipoImporte")]
    public class TipoImporte : BasicObject
    {
        private bool fCalculado;
        private string fCodigo;
        private Concepto fConceptoFacturacion;
        private string fFormulaBaseCalculo;

        private string fNombre;
        private UnidadMedidaTasa fUnidadMedidaTasa;

        public TipoImporte(Session session)
            : base(session)
        {
        }

        [Index(0)]
        [VisibleInLookupListView(true)]
        [RuleRequiredField]
        [Size(10)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Index(1)]
        [VisibleInLookupListView(true)]
        [RuleRequiredField]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [Index(2)]
        public Concepto ConceptoFacturacion
        {
            get { return fConceptoFacturacion; }
            set { SetPropertyValue("Var", ref fConceptoFacturacion, value); }
        }

        public string FormulaBaseCalculo
        {
            get { return fFormulaBaseCalculo; }
            set { SetPropertyValue("FormulaBaseCalculo", ref fFormulaBaseCalculo, value); }
        }

        public bool Calculado
        {
            get { return fCalculado; }
            set { SetPropertyValue("Calculado", ref fCalculado, value); }
        }

        public UnidadMedidaTasa UnidadMedidaTasa
        {
            get { return fUnidadMedidaTasa; }
            set { SetPropertyValue("UnidadMedidaTasa", ref fUnidadMedidaTasa, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<TipoImporteRegla> Reglas
        {
            get { return GetCollection<TipoImporteRegla>("Reglas"); }
        }
    }
}