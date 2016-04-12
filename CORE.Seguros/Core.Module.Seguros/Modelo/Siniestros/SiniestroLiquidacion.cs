using System;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Seguros
{
    //[DefaultProperty( "Nombre" )]
    [System.ComponentModel.DisplayName("Liquidación de siniestro")]
    [Persistent("seguros.SiniestroLiquidacion")]
    public class SiniestroLiquidacion : BasicObject
    {
        private Especie fEspecie;
        private DateTime fFecha;
        private decimal fImporteLiquidado;
        private int fItem;
        private string fNotas;
        private Siniestro fSiniestro;
        private SiniestroLiquidacionTipoMovimiento fTipoMovimiento;

        public SiniestroLiquidacion(Session session)
            : base(session)
        {
        }

        [Association]
        public Siniestro Siniestro
        {
            get { return fSiniestro; }
            set { SetPropertyValue("Siniestro", ref fSiniestro, value); }
        }

        public int Item
        {
            get { return fItem; }
            set { SetPropertyValue<int>("Item", ref fItem, value); }
        }

        public DateTime Fecha
        {
            get { return fFecha; }
            set { SetPropertyValue<DateTime>("Fecha", ref fFecha, value); }
        }

        public SiniestroLiquidacionTipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        public Especie Especie
        {
            get { return fEspecie; }
            set { SetPropertyValue("Especie", ref fEspecie, value); }
        }

        public decimal ImporteLiquidado
        {
            get { return fImporteLiquidado; }
            set { SetPropertyValue<decimal>("ImporteLiquidado", ref fImporteLiquidado, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return fNotas; }
            set { SetPropertyValue("Notas", ref fNotas, value); }
        }
    }
}