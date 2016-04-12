using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.CRM
{
    [Persistent(@"crm.CasoPrioridad")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Prioridad de caso")]
    public class CasoPrioridad : BasicObject
    {
        private string fNombre;

        public CasoPrioridad(Session session)
            : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}