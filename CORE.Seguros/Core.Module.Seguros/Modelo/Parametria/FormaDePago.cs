using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Forma de pago")]
    [Persistent("seguros.FormaDePago")]
    public class FormaDePago : BasicObject
    {
        private string fCodigo;
        private Especie fEspecie;
        private string fNombre;
        private int fOrden;

        public FormaDePago(Session session)
            : base(session)
        {
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Size(50)]
        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        public Especie Especie
        {
            get { return fEspecie; }
            set { SetPropertyValue("Especie", ref fEspecie, value); }
        }
    }
}