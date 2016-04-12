using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaPlan")]
    [DefaultProperty("Descripcion")]
    public class PolizaPlan : BasicObject, IObjetoConHistorial<DocumentoPlan>
    {
        private int fNumero;
        private DocumentoPlan fPlanVigente;
        private Poliza fPoliza;

        public PolizaPlan(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("TOSTR(Numero) + ' - ' + IIF(ISNULL(PlanVigente),'',PlanVigente.Nombre)")]
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
        public XPCollection<DocumentoPlan> Planes => GetCollection<DocumentoPlan>("Planes");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Detalles vigentes")]
        public XPCollection<PolizaPlanDetalle> Detalles => GetCollection<PolizaPlanDetalle>("Detalles");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoPlan PlanVigente
        {
            get { return fPlanVigente; }
            set { SetPropertyValue("PlanVigente", ref fPlanVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoPlan> Historial => Planes;

        [Browsable(false)]
        public DocumentoPlan Vigente
        {
            get { return PlanVigente; }
            set { PlanVigente = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            //Al guardar un nuevo item de poliza, numerarlo con una secuencia a nivel de poliza
            if (Session.IsNewObject(this))
            {
                var proximoNro =
                    Session.Evaluate(typeof (PolizaPlan), CriteriaOperator.Parse("MAX(Numero)"),
                        CriteriaOperator.Parse("Poliza = ?", Poliza)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}