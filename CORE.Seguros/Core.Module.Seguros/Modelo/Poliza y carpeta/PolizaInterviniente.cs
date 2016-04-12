using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.PolizaInterviniente")]
    [DefaultProperty("Descripcion")]
    public class PolizaInterviniente : BasicObject, IObjetoConHistorial<DocumentoInterviniente>
    {
        private DocumentoInterviniente _intervinienteVigente;
        private int _numero;
        private Poliza _poliza;

        public PolizaInterviniente(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias(
            "TOSTR(Numero) + ' - ' + IIF(ISNULL(IntervinienteVigente), '', IntervinienteVigente.Interviniente.Nombre)")]
        public string Descripcion => (string) EvaluateAlias("Descripcion");

        [Association]
        public Poliza Poliza
        {
            get { return _poliza; }
            set { SetPropertyValue("Poliza", ref _poliza, value); }
        }

        public int Numero
        {
            get { return _numero; }
            set { SetPropertyValue<int>("Numero", ref _numero, value); }
        }

        [System.ComponentModel.DisplayName("Historial")]
        [Association]
        public XPCollection<DocumentoInterviniente> Intervinientes
            => GetCollection<DocumentoInterviniente>("Intervinientes");

        [ExpandObjectMembers(ExpandObjectMembers.InListView)]
        public DocumentoInterviniente IntervinienteVigente
        {
            get { return _intervinienteVigente; }
            set { SetPropertyValue("IntervinienteVigente", ref _intervinienteVigente, value); }
        }

        [Browsable(false)]
        public XPCollection<DocumentoInterviniente> Historial => Intervinientes;

        [Browsable(false)]
        public DocumentoInterviniente Vigente
        {
            get { return IntervinienteVigente; }
            set { IntervinienteVigente = value; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            // Al guardar un nuevo interviniente de poliza, numerarlo con una secuencia a nivel de poliza
            if (Session.IsNewObject(this))
            {
                var proximoNro =
                    Session.Evaluate(typeof (PolizaInterviniente), CriteriaOperator.Parse("MAX(Numero)"),
                        CriteriaOperator.Parse("Poliza = ?", Poliza)) ?? 0;
                Numero = (int) proximoNro + 1;
            }
        }
    }
}