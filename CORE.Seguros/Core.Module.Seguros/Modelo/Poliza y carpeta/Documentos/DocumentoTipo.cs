using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Tipo de documento")]
    [Persistent("seguros.DocumentoTipo")]
    public class DocumentoTipo : BasicObject
    {
        private DocumentoClase fClase;
        private string fCodigo;
        private bool fGeneracionDirecta;
        private string fNombre;

        private int fOrden;
        private DocumentoSubclase fSubclase;

        public DocumentoTipo(Session session)
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

        [Size(50)]
        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public DocumentoClase Clase
        {
            get { return fClase; }
            set { SetPropertyValue("Clase", ref fClase, value); }
        }

        public DocumentoSubclase Subclase
        {
            get { return fSubclase; }
            set { SetPropertyValue("Subclase", ref fSubclase, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        public bool GeneracionDirecta
        {
            get { return fGeneracionDirecta; }
            set { SetPropertyValue("GeneracionDirecta", ref fGeneracionDirecta, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Clase = DocumentoClase.Endoso;
            Subclase = DocumentoSubclase.Modificacion;
        }
    }
}