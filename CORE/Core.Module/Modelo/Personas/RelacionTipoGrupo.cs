using System.ComponentModel;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [Persistent(@"personas.RelacionTipoGrupo")]
    [System.ComponentModel.DisplayName("Tipo de relación por grupo")]
    public class RelacionTipoGrupo : BasicObject
    {
        private RelacionTipo fRelacionTipo;
        private bool fUniversal;

        public RelacionTipoGrupo(Session session) : base(session)
        {
        }

        [Browsable(false)]
        [Association(@"RelacionesTiposGruposReferencesRelacionesTipos")]
        public RelacionTipo RelacionTipo
        {
            get { return fRelacionTipo; }
            set { SetPropertyValue("RelacionTipo", ref fRelacionTipo, value); }
        }

        public bool Universal
        {
            get { return fUniversal; }
            set { SetPropertyValue("Universal", ref fUniversal, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}