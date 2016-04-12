using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Tipo de aplicación cuota")]
    [Persistent("seguros.DocumentoCuotaAplicacionTipo")]
    public class DocumentoCuotaAplicacionTipo : BasicObject
    {
        private string fCodigo;
        private string fNombre;

        public DocumentoCuotaAplicacionTipo(Session session) : base(session)
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