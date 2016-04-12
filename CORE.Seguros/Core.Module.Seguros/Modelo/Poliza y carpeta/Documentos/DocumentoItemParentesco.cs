using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Parentesco entre items")]
    [Persistent("seguros.DocumentoItemParentesco")]
    public class DocumentoItemParentesco : BasicObject
    {
        private string fNombre;
        private TipoItemSubramo fSubramoTipo;

        public DocumentoItemParentesco(Session session) : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public TipoItemSubramo SubramoTipo
        {
            get { return fSubramoTipo; }
            set { SetPropertyValue("SubramoTipo", ref fSubramoTipo, value); }
        }
    }
}