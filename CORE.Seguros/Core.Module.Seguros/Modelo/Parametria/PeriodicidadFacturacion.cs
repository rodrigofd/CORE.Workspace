using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Periodicidad de facturación")]
    [Persistent("seguros.PeriodicidadFacturacion")]
    public class PeriodicidadFacturacion : BasicObject
    {
        private string fCodigo;
        private int fMeses;
        private string fNombre;
        private int fOrden;

        public PeriodicidadFacturacion(Session session)
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

        public int Meses
        {
            get { return fMeses; }
            set { SetPropertyValue<int>("Meses", ref fMeses, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}