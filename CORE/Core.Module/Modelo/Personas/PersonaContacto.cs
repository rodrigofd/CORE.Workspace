using System.ComponentModel;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [Persistent(@"personas.PersonaContacto")]
    [System.ComponentModel.DisplayName("Contacto de persona")]
    public class PersonaContacto : BasicObject
    {
        private ContactoTipo fContactoTipo;
        private string fNombre;
        private int fOrden;
        private Persona fPersona;
        private string fUbicacion;

        public PersonaContacto(Session session) : base(session)
        {
        }

        [Browsable(false)]
        [Association(@"PersonasContactosReferencesPersonas")]
        public Persona Persona
        {
            get { return fPersona; }
            set { SetPropertyValue("Persona", ref fPersona, value); }
        }

        [System.ComponentModel.DisplayName("Tipo")]
        public ContactoTipo ContactoTipo
        {
            get { return fContactoTipo; }
            set { SetPropertyValue("ContactoTipo", ref fContactoTipo, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        [System.ComponentModel.DisplayName("Ubicación")]
        [Size(SizeAttribute.Unlimited)]
        public string Ubicacion
        {
            get { return fUbicacion; }
            set { SetPropertyValue("Ubicacion", ref fUbicacion, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}