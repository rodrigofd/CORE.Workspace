using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [ImageName("car-red")]
    [System.ComponentModel.DisplayName("Item de documento (vehiculo)")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class DocumentoItemVehiculo : DocumentoItem
    {
        private int fCodigoBaseDatos;
        private bool fVehiculo0Km;
        private int fVehiculoAno;
        private string fVehiculoCaracteristicas;
        private string fVehiculoChasis;
        private VehiculoColor fVehiculoColor;
        private VehiculoMarca fVehiculoMarca;
        private string fVehiculoMarcaOtra;
        private VehiculoModelo fVehiculoModelo;
        private string fVehiculoModeloOtro;
        private string fVehiculoMotor;
        private string fVehiculoPatente;
        private VehiculoTipo fVehiculoTipo;
        private VehiculoUso fVehiculoUso;

        public DocumentoItemVehiculo(Session session)
            : base(session)
        {
        }

        public VehiculoTipo VehiculoTipo
        {
            get { return fVehiculoTipo; }
            set { SetPropertyValue("VehiculoTipo", ref fVehiculoTipo, value); }
        }

        public VehiculoMarca VehiculoMarca
        {
            get { return fVehiculoMarca; }
            set { SetPropertyValue("VehiculoMarca", ref fVehiculoMarca, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        public string VehiculoMarcaOtra
        {
            get { return fVehiculoMarcaOtra; }
            set { SetPropertyValue("VehiculoMarcaOtra", ref fVehiculoMarcaOtra, value); }
        }

        [DataSourceProperty("VehiculoMarca.Modelos")]
        public VehiculoModelo VehiculoModelo
        {
            get { return fVehiculoModelo; }
            set { SetPropertyValue("VehiculoModelo", ref fVehiculoModelo, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        public string VehiculoModeloOtro
        {
            get { return fVehiculoModeloOtro; }
            set { SetPropertyValue("VehiculoModeloOtro", ref fVehiculoModeloOtro, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string VehiculoCaracteristicas
        {
            get { return fVehiculoCaracteristicas; }
            set { SetPropertyValue("VehiculoCaracteristicas", ref fVehiculoCaracteristicas, value); }
        }

        public int VehiculoAño
        {
            get { return fVehiculoAno; }
            set { SetPropertyValue<int>("VehiculoAno", ref fVehiculoAno, value); }
        }

        public bool Vehiculo0Km
        {
            get { return fVehiculo0Km; }
            set { SetPropertyValue("Vehiculo0Km", ref fVehiculo0Km, value); }
        }

        [Size(30)]
        [ModelDefault("RowCount", "1")]
        public string VehiculoPatente
        {
            get { return fVehiculoPatente; }
            set { SetPropertyValue("VehiculoPatente", ref fVehiculoPatente, value); }
        }

        [Size(150)]
        [ModelDefault("RowCount", "1")]
        public string VehiculoMotor
        {
            get { return fVehiculoMotor; }
            set { SetPropertyValue("VehiculoMotor", ref fVehiculoMotor, value); }
        }

        [Size(150)]
        [ModelDefault("RowCount", "1")]
        public string VehiculoChasis
        {
            get { return fVehiculoChasis; }
            set { SetPropertyValue("VehiculoChasis", ref fVehiculoChasis, value); }
        }

        public VehiculoColor VehiculoColor
        {
            get { return fVehiculoColor; }
            set { SetPropertyValue("VehiculoColor", ref fVehiculoColor, value); }
        }

        public VehiculoUso VehiculoUso
        {
            get { return fVehiculoUso; }
            set { SetPropertyValue("VehiculoUso", ref fVehiculoUso, value); }
        }

        public int CodigoBaseDatos
        {
            get { return fCodigoBaseDatos; }
            set { SetPropertyValue<int>("CodigoBaseDatos", ref fCodigoBaseDatos, value); }
        }
    }
}