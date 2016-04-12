using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Interviniente por item")]
    [Persistent("seguros.DocumentoIntervinienteItem")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaIntervinienteItemPorDocumentoItem", DefaultContexts.Save,
        "DocumentoItem,PolizaIntervinienteItem",
        CustomMessageTemplate = "No puede modificar el mismo interviniente, mas de una vez, en el mismo item")]
    public class DocumentoIntervinienteItem : DocumentoIntervinienteBase,
        IMovimientoHistorial<PolizaIntervinienteItem, DocumentoIntervinienteItem>
    {
        private DocumentoItem fDocumentoItem;
        private PolizaIntervinienteItem fPolizaIntervinienteItem;

        public DocumentoIntervinienteItem(Session session)
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
        [DataSourceProperty("DocumentoItem.PolizaItem.Intervinientes")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostrarpolinterviniente", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        public PolizaIntervinienteItem PolizaIntervinienteItem
        {
            get { return fPolizaIntervinienteItem; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaIntervinienteItem != null)
                        if (fPolizaIntervinienteItem.IntervinientesItems.Count == 0) fPolizaIntervinienteItem.Delete();
                }
                SetPropertyValue("PolizaIntervinienteItem", ref fPolizaIntervinienteItem, value);
            }
        }

        [Browsable(false)]
        public XPCollection<PolizaIntervinienteItem> PadreObjetoConHistorial
        {
            get
            {
                if (DocumentoItem.PolizaItem == null)
                    DocumentoItem.IniciarMovimientoHistorial();

                return DocumentoItem.PolizaItem.Intervinientes;
            }
        }

        [Browsable(false)]
        public PolizaIntervinienteItem ObjetoConHistorial
        {
            get { return PolizaIntervinienteItem; }
            set { PolizaIntervinienteItem = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();

            if (!IsDeleted)
            {
                //Regla de coherencia: si el interviniente se definió PRINCIPAL, asegurarse que el resto (de ese rol/interviniente) no lo sea
                if (Principal)
                {
                    var intervRelacionados = DocumentoItem.Intervinientes;

                    foreach (var interv in intervRelacionados.Where(interv => interv.Rol.Oid == Rol.Oid))
                        interv.Principal = false;

                    Principal = true;
                }
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores por defecto
            TipoMovimiento = TipoMovimiento.Alta;
        }
    }
}