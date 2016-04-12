using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Productor de seguros")]
    [DefaultProperty("Descripcion")]
    [Persistent(@"seguros.Productor")]
    public class Productor : Personas.Rol
    {
        public Productor(Session session)
            : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Persona.Nombre")]
        public string Descripcion
        {
            get { return Convert.ToString(EvaluateAlias("Descripcion")); }
        }
    }
}