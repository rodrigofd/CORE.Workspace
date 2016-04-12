using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.CRM
{
    [Persistent(@"crm.ActividadEstado")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Estados de actividades")]
    public class ActividadEstado : BasicObject
    {
        private string fNombre;
        private int? fPorcentajePredeterminado;

        public ActividadEstado(Session session) : base(session)
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