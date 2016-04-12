using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Codigo")]
    [System.ComponentModel.DisplayName("Unidad de medida para tasa")]
    [Persistent("seguros.UnidadMedidaTasa")]
    public class UnidadMedidaTasa : BasicObject
    {
        private string fCodigo;
        private decimal fCoeficiente;
        private string fNombre;
        private int fOrden;

        public UnidadMedidaTasa(Session session) : base(session)
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

        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "n4")]
        public decimal Coeficiente
        {
            get { return fCoeficiente; }
            set { SetPropertyValue<decimal>("Coeficiente", ref fCoeficiente, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}