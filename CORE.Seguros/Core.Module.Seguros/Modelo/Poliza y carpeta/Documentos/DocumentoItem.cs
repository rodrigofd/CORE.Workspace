using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Personas;
using FDIT.Core.Util;

namespace FDIT.Core.Seguros
{
    [ImageName("table_select_row")]
    [System.ComponentModel.DisplayName("Item de documento (general)")]
    [DefaultProperty("Descripcion")]
    [Persistent("seguros.DocumentoItem")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaItemPorDocumento", DefaultContexts.Save, "Documento,PolizaItem",
        CustomMessageTemplate = "No puede modificar el mismo item, mas de una vez, en el mismo documento")]
    public class DocumentoItem : BasicObject, IObjetoConImportes, IMovimientoHistorial<PolizaItem, DocumentoItem>
    {
        private Documento fDocumento;
        private string fMateriaAsegurada;
        private string fNumeroAseguradora;
        private PolizaPlan fPlan;
        private PolizaItem fPolizaItem;
        private TipoMovimiento fTipoMovimiento;
        private Direccion fUbicacion;

        public DocumentoItem(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("MateriaAsegurada")]
        public string Descripcion
        {
            get { return (string) EvaluateAlias("Descripcion"); }
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
        [DataSourceProperty("Documento.Poliza.Items")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostrarpolitem", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        [ModelDefault("AllowEdit", "false")]
        public PolizaItem PolizaItem
        {
            get { return fPolizaItem; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaItem != null)
                        if (fPolizaItem.Items.Count == 0) fPolizaItem.Delete();
                }
                SetPropertyValue("PolizaItem", ref fPolizaItem, value);
            }
        }

        [Size(20)]
        public string NumeroAseguradora
        {
            get { return fNumeroAseguradora; }
            set { SetPropertyValue("NumeroAseguradora", ref fNumeroAseguradora, value); }
        }

        [RuleRequiredField]
        [VisibleInListView(true)]
        [Size(SizeAttribute.Unlimited)]
        public string MateriaAsegurada
        {
            get { return fMateriaAsegurada; }
            set { SetPropertyValue("MateriaAsegurada", ref fMateriaAsegurada, value); }
        }

        [DataSourceProperty("Documento.Poliza.Planes")]
        public PolizaPlan Plan
        {
            get { return fPlan; }
            set { SetPropertyValue("Plan", ref fPlan, value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public Direccion Ubicacion
        {
            get { return fUbicacion; }
            set { SetPropertyValue("Ubicacion", ref fUbicacion, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoItemImporte> Importes
        {
            get { return GetCollection<DocumentoItemImporte>("Importes"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoIntervinienteItem> Intervinientes
        {
            get { return GetCollection<DocumentoIntervinienteItem>("Intervinientes"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoItemDetalle> Detalles
        {
            get { return GetCollection<DocumentoItemDetalle>("Detalles"); }
        }

        [Association]
        public XPCollection<Siniestro> Siniestros
        {
            get { return GetCollection<Siniestro>("Siniestros"); }
        }

        [ImmediatePostData]
        [ModelDefault("AllowEdit", "false")]
        public TipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        [Browsable(false)]
        public XPCollection<PolizaItem> PadreObjetoConHistorial
        {
            get { return Documento.Poliza.Items; }
        }

        [Browsable(false)]
        public PolizaItem ObjetoConHistorial
        {
            get { return PolizaItem; }
            set { PolizaItem = value; }
        }

        public void RecalcularImportes(DocumentoImporteBase excluir)
        {
            DocumentoImporteBase.RefreshImportes(Importes, excluir);
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Ubicacion != null)
                Ubicacion.Delete();
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();
        }

        public void ActualizarImportesSegunDetalles()
        {
            var subtotales = new Dictionary<int, decimal>();

            foreach (var detalleImporte in Detalles.SelectMany(detalle => detalle.Importes))
            {
                if (!subtotales.ContainsKey(detalleImporte.Tipo.Oid))
                    subtotales[detalleImporte.Tipo.Oid] = 0;
                subtotales[detalleImporte.Tipo.Oid] += detalleImporte.Importe;
            }

            Importes.Empty();

            foreach (var subtotal in subtotales)
                Importes.Add(new DocumentoItemImporte(Session)
                {
                    Importe = subtotal.Value,
                    Tipo = Session.GetObjectByKey<TipoImporte>(subtotal.Key)
                });
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores por defecto
            TipoMovimiento = TipoMovimiento.Alta;
        }
    }
}