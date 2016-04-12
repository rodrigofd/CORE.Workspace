using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace FDIT.Core.Seguros
{
    [NonPersistent]
    public abstract class DocumentoImporteBase : BasicObject
    {
        private decimal? fBaseCalculo;
        private decimal fImporte;
        private decimal? fTasa;
        private TipoImporte fTipo;
        private UnidadMedidaTasa fUnidadMedida;

        public DocumentoImporteBase(Session session)
            : base(session)
        {
        }

        [ImmediatePostData]
        [RuleRequiredField]
        public TipoImporte Tipo
        {
            get { return fTipo; }
            set
            {
                SetPropertyValue("Tipo", ref fTipo, value);

                if (CanRaiseOnChanged)
                {
                    if (Tipo.Calculado)
                    {
                        if (UnidadMedida == null)
                            SetPropertyValue("UnidadMedida", ref fUnidadMedida, Tipo.UnidadMedidaTasa);
                        if (Tasa == null)
                            SetPropertyValue("Tasa", ref fTasa, 0);
                        CalcularBaseCalculo();
                    }
                    else
                    {
                        SetPropertyValue("BaseCalculo", ref fBaseCalculo, null);
                        SetPropertyValue("Tasa", ref fTasa, null);
                        SetPropertyValue("UnidadMedida", ref fUnidadMedida, null);
                    }
                }
            }
        }

        [Appearance("ImporteBaseCalculoShow", "NOT Tipo.Calculado", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        public decimal? BaseCalculo
        {
            get { return fBaseCalculo; }
            set
            {
                SetPropertyValue("BaseCalculo", ref fBaseCalculo, value);
                if (CanRaiseOnChanged)
                {
                    CalcularImporte();
                }
            }
        }

        [Appearance("ImporteTasaShow", "NOT Tipo.Calculado", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:n4} %")]
        [ModelDefault("EditMask", "n4")]
        public decimal? Tasa
        {
            get { return fTasa; }
            set
            {
                SetPropertyValue("Tasa", ref fTasa, value);
                if (CanRaiseOnChanged)
                {
                    CalcularImporte();
                }
            }
        }

        [Appearance("ImporteUnidadMedidaShow", "NOT Tipo.Calculado", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        [ImmediatePostData]
        public UnidadMedidaTasa UnidadMedida
        {
            get { return fUnidadMedida; }
            set
            {
                SetPropertyValue("UnidadMedida", ref fUnidadMedida, value);
                if (CanRaiseOnChanged)
                {
                    CalcularImporte();
                }
            }
        }

        [RuleRequiredField]
        public decimal Importe
        {
            get { return fImporte; }
            set
            {
                SetPropertyValue<decimal>("Importe", ref fImporte, value);
                if (CanRaiseOnChanged)
                {
                    Padre?.RecalcularImportes(this);
                }
            }
        }

        protected abstract IObjetoConImportes Padre { get; }

        public void CalcularImporte()
        {
            Importe = UnidadMedida == null
                ? 0
                : BaseCalculo.GetValueOrDefault()*Tasa.GetValueOrDefault()*UnidadMedida.Coeficiente;
        }

        public virtual void CalcularBaseCalculo()
        {
            if (Tipo == null) return;

            var formula = Tipo.FormulaBaseCalculo;
            if (string.IsNullOrEmpty(formula)) return;

            BaseCalculo = (decimal) Padre.Evaluate(formula);
        }

        public static void RefreshImportes(IEnumerable importes, DocumentoImporteBase exclude)
        {
            foreach (
                var importe in
                    importes.Cast<DocumentoImporteBase>().Where(importe => !ReferenceEquals(importe, exclude)))
                importe.CalcularBaseCalculo();
        }

        public static List<int> ImportesPredeterminados(Session session, Ramo ramo, Subramo subramo)
        {
            var tiposPredet = new List<int>();

            var tipos = new XPCollection<TipoImporteRegla>(
                            session,
                            CriteriaOperator.Parse("(ISNULL(Ramo) Or Ramo = ?) AND (ISNULL(Subramo) OR Subramo = ?)", ramo, subramo),
                            new SortProperty("TipoImporte", SortingDirection.Ascending),
                            new SortProperty("Ramo", SortingDirection.Ascending),
                            new SortProperty("Subramo", SortingDirection.Ascending));

            foreach (var tipo in tipos)
            {
                if (tipo.Predeterminado && !tiposPredet.Contains(tipo.TipoImporte.Oid))
                    tiposPredet.Add(tipo.TipoImporte.Oid);
                else if (!tipo.Predeterminado && tiposPredet.Contains(tipo.TipoImporte.Oid))
                    tiposPredet.Remove(tipo.TipoImporte.Oid);
            }

            return tiposPredet;
        }
    }
}