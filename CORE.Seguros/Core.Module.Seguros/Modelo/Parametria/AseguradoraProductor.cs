using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Productor por aseguradora")]
    [Persistent("seguros.AseguradoraProductor")]
    public class AseguradoraProductor : BasicObject
    {
        private Aseguradora fAseguradora;
        private string fCodigo;
        private DateTime fDesde;
        private DateTime fHasta;
        private Productor fProductor;

        public AseguradoraProductor(Session session)
            : base(session)
        {
        }

        [Association]
        public Aseguradora Aseguradora
        {
            get { return fAseguradora; }
            set { SetPropertyValue("Aseguradora", ref fAseguradora, value); }
        }

        public Productor Productor
        {
            get { return fProductor; }
            set { SetPropertyValue("Productor", ref fProductor, value); }
        }

        [Size(50)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        public DateTime Desde
        {
            get { return fDesde; }
            set { SetPropertyValue<DateTime>("Desde", ref fDesde, value); }
        }

        public DateTime Hasta
        {
            get { return fHasta; }
            set { SetPropertyValue<DateTime>("Hasta", ref fHasta, value); }
        }
    }
}