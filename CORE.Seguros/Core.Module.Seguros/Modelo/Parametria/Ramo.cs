using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    public enum ClaseRamo
    {
        Patrimonial = 1,
        Personal = 2
    }

    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Ramo de seguros")]
    [Persistent("seguros.Ramo")]
    public class Ramo : BasicObject
    {
        private ClaseRamo fClase;
        private string fCodigo;
        private string fNombre;
        private int fOrden;

        public Ramo(Session session) : base(session)
        {
        }

        public ClaseRamo Clase
        {
            get { return fClase; }
            set { SetPropertyValue("Clase", ref fClase, value); }
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

        [Association]
        [Aggregated]
        public XPCollection<Subramo> Subramos
        {
            get { return GetCollection<Subramo>("Subramos"); }
        }

        [Association]
        public XPCollection<Titulo> Titulos
        {
            get { return GetCollection<Titulo>("Titulos"); }
        }

        [Association]
        public XPCollection<DocumentoItemClase> DocumentosItemsClases
        {
            get { return GetCollection<DocumentoItemClase>("DocumentosItemsClases"); }
        }

        [Association]
        public XPCollection<Interes> Intereses
        {
            get { return GetCollection<Interes>("Intereses"); }
        }
    }
}