using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Codigo")]
    [System.ComponentModel.DisplayName("Lista de precios de planes")]
    [Persistent("seguros.ListaDePrecios")]
    public class ListaDePrecios : BasicObject
    {
        private string fCodigo;

        private string fNombre;

        public ListaDePrecios(Session session)
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

        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }
    }
}