using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaItem")]
    [DefaultProperty("Descripcion")]
    public class PolizaItem : BasicObject, IObjetoConHistorial<DocumentoItem>
    {
        private DocumentoItem fItemVigente;
        private int fNumero;
        private Poliza fPoliza;

        public PolizaItem(Session session) : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("TOSTR(Numero) + ' - ' + IIF(ISNULL(ItemVigente), '', ItemVigente.MateriaAsegurada)")]
        public string Descripcion => (string) EvaluateAlias("Descripcion");

        [Association]
        public Poliza Poliza
        {
            get { return fPoliza; }
            set { SetPropertyValue("Poliza", ref fPoliza, value); }
        }

        public int Numero
        {
            get { return fNumero; }
            set { SetPropertyValue<int>("Numero", ref fNumero, value); }
        }

        [System.ComponentModel.DisplayName("Historial")]
        [Association]
        public XPCollection<DocumentoItem> Items => GetCollection<DocumentoItem>("Items");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Intervinientes vigentes")]
        public XPCollection<PolizaIntervinienteItem> Intervinientes => GetCollection<PolizaIntervinienteItem>("Intervinientes");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Detalles vigentes")]
        public XPCollection<PolizaItemDetalle> Detalles => GetCollection<PolizaItemDetalle>("Detalles");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoItem ItemVigente
        {
            get { return fItemVigente; }
            set { SetPropertyValue("ItemVigente", ref fItemVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoItem> Historial => Items;

        [Browsable(false)]
        public DocumentoItem Vigente
        {
            get { return ItemVigente; }
            set { ItemVigente = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            // Al guardar un nuevo item de poliza, numerarlo con una secuencia a nivel de poliza
            if (Session.IsNewObject(this))
            {
                var proximoNro = Session.Evaluate(typeof (PolizaItem), CriteriaOperator.Parse("MAX(Numero)"), CriteriaOperator.Parse("Poliza = ?", Poliza)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}