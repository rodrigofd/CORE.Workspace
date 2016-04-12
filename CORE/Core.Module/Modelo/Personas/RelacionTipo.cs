using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [Persistent(@"personas.RelacionTipo")]
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Tipo de relación")]
    public class RelacionTipo : BasicObject
    {
        private string fNombre;
        private string fNombreInverso;

        public RelacionTipo(Session session) : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Nombre + ' - (' + NombreInverso + ')'")]
        public string Descripcion => (string) EvaluateAlias("Descripcion");

        [Size(50)]
        [System.ComponentModel.DisplayName("Nombre")]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [Size(50)]
        [System.ComponentModel.DisplayName("Nombre (inverso)")]
        public string NombreInverso
        {
            get { return fNombreInverso; }
            set { SetPropertyValue("NombreInverso", ref fNombreInverso, value); }
        }

        [Aggregated]
        [System.ComponentModel.DisplayName("Tipos por grupos")]
        [Association(@"RelacionesTiposGruposReferencesRelacionesTipos", typeof (RelacionTipoGrupo))]
        public XPCollection<RelacionTipoGrupo> TiposPorGrupos => GetCollection<RelacionTipoGrupo>("TiposPorGrupos");

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}