using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Marca de vehículo")]
    [Persistent("seguros.VehiculoMarca")]
    public class VehiculoMarca : BasicObject
    {
        private string fCodigo;
        private string fNombre;

        public VehiculoMarca(Session session)
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

        [Aggregated]
        [Association]
        public XPCollection<VehiculoModelo> Modelos
        {
            get { return GetCollection<VehiculoModelo>("Modelos"); }
        }
    }
}