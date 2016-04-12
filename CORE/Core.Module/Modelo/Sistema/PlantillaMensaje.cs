using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Sistema
{
    [Persistent("sistema.PlantillaMensaje")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    public class PlantillaMensaje : BasicObject
    {
        private string fContenido;
        private string fNombre;

        public PlantillaMensaje(Session session) : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Contenido
        {
            get { return fContenido; }
            set { SetPropertyValue("Contenido", ref fContenido, value); }
        }
    }
}