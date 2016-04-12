using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [ImageName("cards_bind_address")]
    [Persistent(@"personas.Identificacion")]
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Identificación")]
    public class Identificacion : BasicObject
    {
        private DateTime fDesde;
        private DateTime fHasta;
        private string fIdentificacion;
        private string fNotas;
        private Persona fPersona;
        private IdentificacionTipo fTipo;

        public Identificacion(Session session) : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("concat(Tipo.Codigo, ' - ' , Valor)")]
        public string Descripcion => Convert.ToString(EvaluateAlias("Descripcion"));

        [Association(@"IdentificacionesReferencesPersonas")]
        public Persona Persona
        {
            get { return fPersona; }
            set { SetPropertyValue("Persona", ref fPersona, value); }
        }

        [System.ComponentModel.DisplayName("Tipo")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public IdentificacionTipo Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        [Size(200)]
        [System.ComponentModel.DisplayName("Identificación")]
        public string Valor
        {
            get { return fIdentificacion; }
            set { SetPropertyValue("Valor", ref fIdentificacion, value); }
        }

        public DateTime Desde
        {
            get { return fDesde; }
            set { SetPropertyValue<DateTime>("Desde", ref fDesde, value); }
        }

        public DateTime Hasta
        {
            get { return fHasta; }
            set { SetPropertyValue<DateTime>("Hasta", ref fHasta, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return fNotas; }
            set { SetPropertyValue("Notas", ref fNotas, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}