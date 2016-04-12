using System;
using System.ComponentModel;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Fabricaci�n de modelo de veh�culo")]
    [Persistent("seguros.VehiculoFabricacion")]
    public class VehiculoFabricacion : BasicObject
    {
        private int fA�o;
        private bool fCeroKm;
        private int fCodigoExterno;
        private string fDescripcion;
        private Especie fEspecie;
        private VehiculoModelo fModelo;
        private decimal fPrecio;
        private DateTime fVigencia;

        public VehiculoFabricacion(Session session)
            : base(session)
        {
        }

        [Association]
        public VehiculoModelo Modelo
        {
            get { return fModelo; }
            set { SetPropertyValue("Modelo", ref fModelo, value); }
        }

        public int A�o
        {
            get { return fA�o; }
            set { SetPropertyValue<int>("A�o", ref fA�o, value); }
        }

        public bool CeroKm
        {
            get { return fCeroKm; }
            set { SetPropertyValue("CeroKm", ref fCeroKm, value); }
        }

        public Especie Especie
        {
            get { return fEspecie; }
            set { SetPropertyValue("Especie", ref fEspecie, value); }
        }

        public decimal Precio
        {
            get { return fPrecio; }
            set { SetPropertyValue<decimal>("Precio", ref fPrecio, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Descripcion
        {
            get { return fDescripcion; }
            set { SetPropertyValue("Descripcion", ref fDescripcion, value); }
        }

        public DateTime VigenciaHasta
        {
            get { return fVigencia; }
            set { SetPropertyValue<DateTime>("Vigencia", ref fVigencia, value); }
        }

        public int CodigoExterno
        {
            get { return fCodigoExterno; }
            set { SetPropertyValue<int>("CodigoExterno", ref fCodigoExterno, value); }
        }
    }
}