using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Interés asegurable")]
    [Persistent("seguros.Interes")]
    public class Interes : BasicObject
    {
        private string fCodigo;
        private bool fIva;
        private decimal fIva1Tasa;
        private string fNombre;
        private int fOrden;
        private Ramo fRamo;
        private Subramo fSubramo;

        public Interes(Session session)
            : base(session)
        {
        }

        [Association]
        public Ramo Ramo
        {
            get { return fRamo; }
            set { SetPropertyValue("Ramo", ref fRamo, value); }
        }

        [Association]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Subramo Subramo
        {
            get { return fSubramo; }
            set { SetPropertyValue("Subramo", ref fSubramo, value); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Size(200)]
        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public bool Iva
        {
            get { return fIva; }
            set { SetPropertyValue("Iva", ref fIva, value); }
        }

        public decimal Iva1Tasa
        {
            get { return fIva1Tasa; }
            set { SetPropertyValue<decimal>("Iva1Tasa", ref fIva1Tasa, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}