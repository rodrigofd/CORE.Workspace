using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Detalle de plan")]
    [Persistent("seguros.DocumentoPlanDetalle")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaPlanDetallePorDocumentoPlan", DefaultContexts.Save,
        "DocumentoPlan,PolizaPlanDetalle",
        CustomMessageTemplate = "No puede modificar el mismo detalle, mas de una vez, en el mismo plan"
        )]
    public class DocumentoPlanDetalle : DetalleBase, IMovimientoHistorial<PolizaPlanDetalle, DocumentoPlanDetalle>
    {
        private DocumentoPlan fDocumentoPlan;
        private PolizaPlanDetalle fPolizaPlanDetalle;
        private TipoMovimiento fTipoMovimiento;

        public DocumentoPlanDetalle(Session session)
            : base(session)
        {
        }

        [Association]
        public DocumentoPlan DocumentoPlan
        {
            get { return fDocumentoPlan; }
            set { SetPropertyValue("DocumentoPlan", ref fDocumentoPlan, value); }
        }

        [VisibleInListView(false)]
        [ImmediatePostData]
        [Association]
        [DataSourceProperty("DocumentoPlan.PolizaPlan.Detalles")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostrarplandet", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        public PolizaPlanDetalle PolizaPlanDetalle
        {
            get { return fPolizaPlanDetalle; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaPlanDetalle != null)
                        if (fPolizaPlanDetalle.Detalles.Count == 0) fPolizaPlanDetalle.Delete();
                }
                SetPropertyValue("PolizaPlanDetalle", ref fPolizaPlanDetalle, value);
            }
        }

        public override Persona AseguradoraAsociada
        {
            get { return DocumentoPlan == null ? null : DocumentoPlan.Documento.Poliza.Aseguradora.Interviniente; }
        }

        public override Ramo RamoAsociado
        {
            get { return DocumentoPlan == null ? null : DocumentoPlan.Documento.Ramo; }
        }

        public override Subramo SubramoAsociado
        {
            get { return DocumentoPlan == null ? null : DocumentoPlan.Documento.Subramo; }
        }

        [ImmediatePostData]
        public TipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        [Browsable(false)]
        public XPCollection<PolizaPlanDetalle> PadreObjetoConHistorial
        {
            get
            {
                if (DocumentoPlan.PolizaPlan == null)
                    DocumentoPlan.IniciarMovimientoHistorial();

                return DocumentoPlan.PolizaPlan.Detalles;
            }
        }

        [Browsable(false)]
        public PolizaPlanDetalle ObjetoConHistorial
        {
            get { return PolizaPlanDetalle; }
            set { PolizaPlanDetalle = value; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores por defecto
            TipoMovimiento = TipoMovimiento.Alta;
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();
        }
    }
}