using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
    [ImageName("postage-stamp-at-sign")]
    [Persistent(@"personas.PersonaDireccion")]
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Dirección de persona")]
    [Appearance("manual_ciudad", Criteria = "Not IsNull(Direccion.Ciudad)", Visibility = ViewItemVisibility.Hide,
        TargetItems = "Direccion.CiudadOtra")]
    [Appearance("manual_localidad", Criteria = "Not IsNull(Direccion.Localidad)", Visibility = ViewItemVisibility.Hide,
        TargetItems = "Direccion.LocalidadOtra")]
    [Appearance("manual_provincia", Criteria = "Not IsNull(Direccion.Provincia)", Visibility = ViewItemVisibility.Hide,
        TargetItems = "Direccion.ProvinciaOtra")]
    [Appearance("manual_pais", Criteria = "Not IsNull(Direccion.Pais)", Visibility = ViewItemVisibility.Hide,
        TargetItems = "Direccion.PaisOtro")]
    public class PersonaDireccion : BasicObject
    {
        private string fCodigo;
        private Direccion fDireccion;
        private DireccionTipo fDireccionTipo;
        private Persona fPersona;

        public PersonaDireccion(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [PersistentAlias("IIF(ISNULL(Codigo), '', Codigo + ' - ') + Direccion.DireccionCompleta")]
        public string Descripcion
        {
            get { return Convert.ToString(EvaluateAlias("Descripcion")); }
        }

        [Association("Personas-PersonasDirecciones")]
        public Persona Persona
        {
            get { return fPersona; }
            set { SetPropertyValue("Persona", ref fPersona, value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public Direccion Direccion
        {
            get { return fDireccion; }
            set { SetPropertyValue("Direccion", ref fDireccion, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        public DireccionTipo DireccionTipo
        {
            get { return fDireccionTipo; }
            set { SetPropertyValue("DireccionTipo", ref fDireccionTipo, value); }
        }

        [Size(50)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }
        
        protected override void OnDeleting()
        {
            base.OnDeleting();

            /*if ( Persona.DireccionPrimaria != null && ( Persona.DireccionPrimaria == this || Persona.DireccionPrimaria.Oid == this.Oid ) )
            {
                Persona.DireccionPrimaria = null;
            }*/
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Direccion = new Direccion(Session);
        }
    }
}