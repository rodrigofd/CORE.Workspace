using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaItemDetalle")]
    [DefaultProperty("Descripcion")]
    public class PolizaItemDetalle : BasicObject, IObjetoConHistorial<DocumentoItemDetalle>
    {
        private DocumentoItemDetalle fDetalleVigente;
        private int fNumero;
        private PolizaItem fPolizaItem;

        public PolizaItemDetalle(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("TOSTR(Numero) + ' - ' + IIF(ISNULL(DetalleVigente), '', DetalleVigente)")]
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
        public XPCollection<DocumentoItemDetalle> Detalles => GetCollection<DocumentoItemDetalle>("Detalles");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoItemDetalle DetalleVigente
        {
            get { return fDetalleVigente; }
            set { SetPropertyValue("DetalleVigente", ref fDetalleVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoItemDetalle> Historial => Detalles;

        [Browsable(false)]
        public DocumentoItemDetalle Vigente
        {
            get { return DetalleVigente; }
            set { DetalleVigente = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            //Al guardar un nuevo interviniente de poliza, numerarlo con una secuencia a nivel de poliza
            if (Session.IsNewObject(this))
            {
                var proximoNro = Session.Evaluate(typeof (PolizaItemDetalle), CriteriaOperator.Parse("MAX(Numero)"), CriteriaOperator.Parse("PolizaItem = ?", PolizaItem)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}