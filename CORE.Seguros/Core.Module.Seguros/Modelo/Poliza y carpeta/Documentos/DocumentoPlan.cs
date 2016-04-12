using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [ImageName("blue-document-table")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Plan de cobertura de documento")]
    [Persistent("seguros.DocumentoPlan")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaPlanPorDocumento", DefaultContexts.Save, "Documento,PolizaPlan",
        CustomMessageTemplate = "No puede modificar el mismo plan, mas de una vez, en el mismo documento")]
    public class DocumentoPlan : BasicObject,
        IMovimientoHistorial<PolizaPlan, DocumentoPlan>
    {
        private string fCodigo;
        private Documento fDocumento;
        private string fNombre;
        private int fOrden;
        private PlanCobertura fPlanCobertura;
        private PolizaPlan fPolizaPlan;
        private TipoMovimiento fTipoMovimiento;

        public DocumentoPlan(Session session) : base(session)
        {
        }

        [Association]
        [System.ComponentModel.DisplayName("Documento asociado")]
        public Documento Documento
        {
            get { return fDocumento; }
            set { SetPropertyValue("Documento", ref fDocumento, value); }
        }

        [VisibleInListView(false)]
        [ImmediatePostData]
        [Association]
        [DataSourceProperty("Documento.Poliza.Planes")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostrarpolplan", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        public PolizaPlan PolizaPlan
        {
            get { return fPolizaPlan; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaPlan != null)
                        if (fPolizaPlan.Planes.Count == 0) fPolizaPlan.Delete();
                }
                SetPropertyValue("PolizaPlan", ref fPolizaPlan, value);
            }
        }

        [Browsable(false)]
        public XPCollection<PlanCobertura> PlanesPosibles
        {
            get
            {
                var xcol = new XPCollection<PlanCobertura>(Session,
                    CriteriaOperator.Parse(
                        "NegocioRiesgo.Subramo = ? AND NegocioRiesgo.LineaAseguradora.Aseguradora.Persona = ?",
                        Documento.Subramo,
                        Documento.Poliza.Aseguradora.Interviniente));
                return xcol;
            }
        }

        [DataSourceProperty("PlanesPosibles")]
        public PlanCobertura PlanCobertura
        {
            get { return fPlanCobertura; }
            set
            {
                SetPropertyValue("PlanCobertura", ref fPlanCobertura, value);
                if (CanRaiseOnChanged)
                {
                    if (PlanCobertura != null)
                    {
                        Codigo = PlanCobertura.Codigo;
                        Nombre = PlanCobertura.Nombre;
                    }
                }
            }
        }

        [Size(10)]
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

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoPlanDetalle> Detalles
        {
            get { return GetCollection<DocumentoPlanDetalle>("Detalles"); }
        }

        [VisibleInDetailView(false)]
        public XPCollection<DocumentoPlanDetalle> Items
        {
            get { return Detalles; }
        }

        [ImmediatePostData]
        public TipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        [Browsable(false)]
        public XPCollection<PolizaPlan> PadreObjetoConHistorial
        {
            get { return Documento.Poliza.Planes; }
        }

        [Browsable(false)]
        public PolizaPlan ObjetoConHistorial
        {
            get { return PolizaPlan; }
            set { PolizaPlan = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores por defecto
            TipoMovimiento = TipoMovimiento.Alta;
        }
    }
}