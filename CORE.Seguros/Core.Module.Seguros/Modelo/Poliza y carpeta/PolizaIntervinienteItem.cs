using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaIntervinienteItem")]
    [DefaultProperty("Descripcion")]
    public class PolizaIntervinienteItem : BasicObject, IObjetoConHistorial<DocumentoIntervinienteItem>
    {
        private DocumentoIntervinienteItem fIntervinienteItemVigente;
        private int fNumero;
        private PolizaItem fPolizaItem;

        public PolizaIntervinienteItem(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("TOSTR(Numero) + ' - ' + IIF(ISNULL(IntervinienteItemVigente),'',IntervinienteItemVigente.Interviniente.Nombre)")]
        public string Descripcion => (string) EvaluateAlias("Descripcion");

        [Association]
        public PolizaItem PolizaItem
        {
            get { return fPolizaItem; }
            set { SetPropertyValue("PolizaItem", ref fPolizaItem, value); }
        }

        public int Numero
        {
            get { return fNumero; }
            set { SetPropertyValue<int>("Numero", ref fNumero, value); }
        }

        [System.ComponentModel.DisplayName("Historial de movimientos")]
        [Association]
        public XPCollection<DocumentoIntervinienteItem> IntervinientesItems => GetCollection<DocumentoIntervinienteItem>("IntervinientesItems");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoIntervinienteItem IntervinienteItemVigente
        {
            get { return fIntervinienteItemVigente; }
            set { SetPropertyValue("IntervinienteItemVigente", ref fIntervinienteItemVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoIntervinienteItem> Historial => IntervinientesItems;

        [Browsable(false)]
        public DocumentoIntervinienteItem Vigente
        {
            get { return IntervinienteItemVigente; }
            set { IntervinienteItemVigente = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            //Al guardar un nuevo interviniente de poliza, numerarlo con una secuencia a nivel de poliza
            if (Session.IsNewObject(this))
            {
                var proximoNro = Session.Evaluate(typeof (PolizaIntervinienteItem), CriteriaOperator.Parse("MAX(Numero)"), CriteriaOperator.Parse("PolizaItem = ?", PolizaItem)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}