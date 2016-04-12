using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [ImageName("property")]
    [Persistent(@"personas.Propiedad")]
    [DefaultProperty("Nombre")]
    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Propiedad")]
    public class Propiedad : BasicObject
    {
        private string fNombre;
        private TipoPersona fTipoPersona;

        public Propiedad(Session session) : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Para tipo de persona")]
        public TipoPersona TipoPersona
        {
            get { return fTipoPersona; }
            set { SetPropertyValue("TipoPersona", ref fTipoPersona, value); }
        }

        [Association(@"PropiedadesValoresReferencesPropiedades", typeof (PropiedadValor))]
        [System.ComponentModel.DisplayName("Valores predefinidos")]
        public XPCollection<PropiedadValor> Valores => GetCollection<PropiedadValor>("Valores");

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoPersona = TipoPersona.Fisica;
            ;
        }
    }
}