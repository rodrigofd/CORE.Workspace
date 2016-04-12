using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.CRM
{
    [Persistent(@"crm.CasoEstado")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Estado de caso")]
    public class CasoEstado : BasicObject
    {
        private string fNombre;
        private int? fPorcentajePredeterminado;

        public CasoEstado(Session session)
            : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public int? PorcentajePredeterminado
        {
            get { return fPorcentajePredeterminado; }
            set { SetPropertyValue("PorcentajePredeterminado", ref fPorcentajePredeterminado, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}