using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Tipo de Título")]
    [Persistent("seguros.TituloTipo")]
    public class TituloTipo : BasicObject
    {
        private string fCodigo;
        private string fNombre;
        private int fOrden;

        public TituloTipo(Session session)
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

        [Size(100)]
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
        public XPCollection<Titulo> Titulos
        {
            get { return GetCollection<Titulo>("Titulos"); }
        }
    }
}