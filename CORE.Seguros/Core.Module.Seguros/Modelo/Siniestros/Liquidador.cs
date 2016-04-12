using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Liquidador")]
    [DefaultProperty("Descripcion")]
    [Persistent("seguros.Liquidador")]
    public class Liquidador : Personas.Rol
    {
        private string fCodigo;
        private int fOrden;

        public Liquidador(Session session)
            : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Persona.Nombre")]
        public string Descripcion
        {
            get { return Convert.ToString(EvaluateAlias("Descripcion")); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}