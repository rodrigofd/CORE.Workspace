using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Modelo de vehículo")]
    [Persistent("seguros.VehiculoModelo")]
    public class VehiculoModelo : BasicObject
    {
        private bool fAbs;
        private bool fAirbag;
        private bool fAireAcondicionado;
        private string fAlimentacion;
        private string fCabina;
        private string fCaja;
        private string fCodigo;
        private int fCodigoExterno;
        private string fCombustion;
        private string fDireccion;
        private bool fEsCarga;
        private int fHp;
        private bool fImportado;
        private VehiculoMarca fMarca;
        private string fMotor;
        private string fNombre;
        private int fPeso;
        private int fPuertas;
        private decimal fTarifaTramo1;
        private decimal fTarifaTramo2;
        private decimal fTarifaTramo3;
        private string fTipo;
        private string fTraccion;
        private string fVehiculoGrupo;
        private int fVelocidadMax;

        public VehiculoModelo(Session session)
            : base(session)
        {
        }

        [Association]
        public VehiculoMarca Marca
        {
            get { return fMarca; }
            set { SetPropertyValue("Marca", ref fMarca, value); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Size(50)]
        public string VehiculoGrupo
        {
            get { return fVehiculoGrupo; }
            set { SetPropertyValue("VehiculoGrupo", ref fVehiculoGrupo, value); }
        }

        [Size(50)]
        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [Size(3)]
        public string Combustion
        {
            get { return fCombustion; }
            set { SetPropertyValue("Combustion", ref fCombustion, value); }
        }

        [Size(3)]
        public string Alimentacion
        {
            get { return fAlimentacion; }
            set { SetPropertyValue("Alimentacion", ref fAlimentacion, value); }
        }

        public string Motor
        {
            get { return fMotor; }
            set { SetPropertyValue("Motor", ref fMotor, value); }
        }

        public int Puertas
        {
            get { return fPuertas; }
            set { SetPropertyValue("Puertas", ref fPuertas, value); }
        }

        [Size(3)]
        public string Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        [Size(4)]
        public string Cabina
        {
            get { return fCabina; }
            set { SetPropertyValue("Cabina", ref fCabina, value); }
        }

        public bool EsCarga
        {
            get { return fEsCarga; }
            set { SetPropertyValue("EsCarga", ref fEsCarga, value); }
        }

        public int Peso
        {
            get { return fPeso; }
            set { SetPropertyValue<int>("Peso", ref fPeso, value); }
        }

        public int VelocidadMax
        {
            get { return fVelocidadMax; }
            set { SetPropertyValue<int>("VelocidadMax", ref fVelocidadMax, value); }
        }

        public int Hp
        {
            get { return fHp; }
            set { SetPropertyValue<int>("Hp", ref fHp, value); }
        }

        [Size(3)]
        public string Direccion
        {
            get { return fDireccion; }
            set { SetPropertyValue("Direccion", ref fDireccion, value); }
        }

        public bool AireAcondicionado
        {
            get { return fAireAcondicionado; }
            set { SetPropertyValue("AireAcondicionado", ref fAireAcondicionado, value); }
        }

        [Size(3)]
        public string Traccion
        {
            get { return fTraccion; }
            set { SetPropertyValue("Traccion", ref fTraccion, value); }
        }

        public bool Importado
        {
            get { return fImportado; }
            set { SetPropertyValue("Importado", ref fImportado, value); }
        }

        [Size(3)]
        public string Caja
        {
            get { return fCaja; }
            set { SetPropertyValue("Caja", ref fCaja, value); }
        }

        public bool Abs
        {
            get { return fAbs; }
            set { SetPropertyValue("Abs", ref fAbs, value); }
        }

        public bool Airbag
        {
            get { return fAirbag; }
            set { SetPropertyValue("Airbag", ref fAirbag, value); }
        }

        public int CodigoExterno
        {
            get { return fCodigoExterno; }
            set { SetPropertyValue<int>("CodigoExterno", ref fCodigoExterno, value); }
        }

        public decimal TarifaTramo1
        {
            get { return fTarifaTramo1; }
            set { SetPropertyValue<decimal>("TarifaTramo1", ref fTarifaTramo1, value); }
        }

        public decimal TarifaTramo2
        {
            get { return fTarifaTramo2; }
            set { SetPropertyValue<decimal>("TarifaTramo2", ref fTarifaTramo2, value); }
        }

        public decimal TarifaTramo3
        {
            get { return fTarifaTramo3; }
            set { SetPropertyValue<decimal>("TarifaTramo3", ref fTarifaTramo3, value); }
        }

        [Aggregated]
        [Association]
        public XPCollection<VehiculoFabricacion> Fabricaciones
        {
            get { return GetCollection<VehiculoFabricacion>("Fabricaciones"); }
        }
    }
}