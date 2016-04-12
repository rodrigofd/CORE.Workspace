using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaPlanDetalle")]
    [DefaultProperty("Descripcion")]
    public class PolizaPlanDetalle : BasicObject, IObjetoConHistorial<DocumentoPlanDetalle>
    {
        private DocumentoPlanDetalle fDetalleVigente;
        private int fNumero;
        private PolizaPlan fPolizaPlan;

        public PolizaPlanDetalle(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("TOSTR(Numero) + ' - ' + IIF(ISNULL(DetalleVigente), '', DetalleVigente)")]
        public string Descripcion => (string) EvaluateAlias("Descripcion");

        [Association]
        public PolizaPlan PolizaPlan
        {
            get { return fPolizaPlan; }
            set { SetPropertyValue("PolizaPlan", ref fPolizaPlan, value); }
        }

        public int Numero
        {
            get { return fNumero; }
            set { SetPropertyValue<int>("Numero", ref fNumero, value); }
        }

        [System.ComponentModel.DisplayName("Historial de movimientos")]
        [Association]
        public XPCollection<DocumentoPlanDetalle> Detalles => GetCollection<DocumentoPlanDetalle>("Detalles");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoPlanDetalle DetalleVigente
        {
            get { return fDetalleVigente; }
            set { SetPropertyValue("DetalleVigente", ref fDetalleVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoPlanDetalle> Historial
        {
            get { return Detalles; }
        }

        [Browsable(false)]
        public DocumentoPlanDetalle Vigente
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
                var proximoNro =
                    Session.Evaluate(typeof (PolizaPlanDetalle), CriteriaOperator.Parse("MAX(Numero)"), CriteriaOperator.Parse("PolizaPlan = ?", PolizaPlan)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}