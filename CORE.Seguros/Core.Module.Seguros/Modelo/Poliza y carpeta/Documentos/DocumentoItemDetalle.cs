using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [ImageName("ui-text-field")]
    [System.ComponentModel.DisplayName("Detalle de item")]
    [Persistent("seguros.DocumentoItemDetalle")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaItemDetallePorDocumentoItem", DefaultContexts.Save,
        "DocumentoItem,PolizaItemDetalle",
        CustomMessageTemplate = "No puede modificar el mismo detalle, mas de una vez, en el mismo item")]
    public class DocumentoItemDetalle : DetalleBase,
        IObjetoConImportes,
        IMovimientoHistorial<PolizaItemDetalle, DocumentoItemDetalle>
    {
        private DocumentoItem fDocumentoItem;
        private PolizaItemDetalle fPolizaItemDetalle;
        private TipoMovimiento fTipoMovimiento;

        public DocumentoItemDetalle(Session session)
            : base(session)
        {
        }

        [Association]
        public DocumentoItem DocumentoItem
        {
            get { return fDocumentoItem; }
            set { SetPropertyValue("DocumentoItem", ref fDocumentoItem, value); }
        }

        [VisibleInListView(false)]
        [ImmediatePostData]
        [Association]
        [DataSourceProperty("DocumentoItem.PolizaItem.Detalles")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostraritemdet", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        public PolizaItemDetalle PolizaItemDetalle
        {
            get { return fPolizaItemDetalle; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaItemDetalle != null)
                        if (fPolizaItemDetalle.Detalles.Count == 0) fPolizaItemDetalle.Delete();
                }
                SetPropertyValue("PolizaItemDetalle", ref fPolizaItemDetalle, value);
            }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoItemDetalleImporte> Importes
        {
            get { return GetCollection<DocumentoItemDetalleImporte>("Importes"); }
        }

        public override Persona AseguradoraAsociada
        {
            get { return DocumentoItem == null ? null : DocumentoItem.Documento.Poliza.Aseguradora.Interviniente; }
        }

        public override Ramo RamoAsociado
        {
            get { return DocumentoItem == null ? null : DocumentoItem.Documento.Ramo; }
        }

        public override Subramo SubramoAsociado
        {
            get { return DocumentoItem == null ? null : DocumentoItem.Documento.Subramo; }
        }

        [ImmediatePostData]
        public TipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        [Browsable(false)]
        public XPCollection<PolizaItemDetalle> PadreObjetoConHistorial
        {
            get
            {
                if (DocumentoItem.PolizaItem == null)
                    DocumentoItem.IniciarMovimientoHistorial();

                return DocumentoItem.PolizaItem.Detalles;
            }
        }

        [Browsable(false)]
        public PolizaItemDetalle ObjetoConHistorial
        {
            get { return PolizaItemDetalle; }
            set { PolizaItemDetalle = value; }
        }

        public void RecalcularImportes(DocumentoImporteBase excluir)
        {
            DocumentoImporteBase.RefreshImportes(Importes, excluir);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            TipoMovimiento = TipoMovimiento.Alta;
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();
        }
    }
}