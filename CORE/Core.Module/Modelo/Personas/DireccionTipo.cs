using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [Persistent(@"personas.DireccionTipo")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Tipo de dirección")]
    public class DireccionTipo : BasicObject
    {
        private string fCodigo;
        private string fDireccionTipo;

        public DireccionTipo(Session session) : base(session)
        {
        }

        [Size(10)]
        [System.ComponentModel.DisplayName("Código")]
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
            get { return fDireccionTipo; }
            set { SetPropertyValue("Nombre", ref fDireccionTipo, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}