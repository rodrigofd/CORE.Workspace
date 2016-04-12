using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Impuestos;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [NonPersistent]
    public abstract class DocumentoIntervinienteBase : BasicObject
    {
        private Categoria fCategoriaImpuestos;
        private decimal fComisionCobranza;
        private decimal fComisionPrima;
        private PersonaContacto fContacto;
        private PersonaDireccion fDireccion;
        private Identificacion fIdentificacion;
        private Persona fInterviniente;
        private XPCollection<Persona> fIntervinientesPosibles;
        private decimal fParticipacion;
        private bool fPrincipal;
        private Rol fRol;
        private TipoMovimiento fTipoMovimiento;

        public DocumentoIntervinienteBase(Session session)
            : base(session)
        {
        }

        [VisibleInListView(false)]
        [PersistentAlias("IIF(ISNULL(Rol),'',Rol.Codigo) + ' - ' + IIF(ISNULL(Interviniente),'',Interviniente.Nombre)")]
        public string Descripcion
        {
            get { return (string) EvaluateAlias("Descripcion"); }
        }

        [ImmediatePostData]
        [VisibleInListView(true)]
        public TipoMovimiento TipoMovimiento
        {
            get { return fTipoMovimiento; }
            set { SetPropertyValue("TipoMovimiento", ref fTipoMovimiento, value); }
        }

        [ImmediatePostData]
        [VisibleInListView(true)]
        public Rol Rol
        {
            get { return fRol; }
            set
            {
                SetPropertyValue("Rol", ref fRol, value);
                if (CanRaiseOnChanged)
                {
                    FilterIntervinientesPosibles();
                }
            }
        }

        [VisibleInListView(true)]
        public bool Principal
        {
            get { return fPrincipal; }
            set { SetPropertyValue("Principal", ref fPrincipal, value); }
        }

        [VisibleInListView(true)]
        [RuleRange(DefaultContexts.Save, 0, 100, CustomMessageTemplate = "El valor debe ser un entero entre 0 y 100")]
        [Appearance("ParticipacionShow", "Not Rol.LlevaParticipacion", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        [ModelDefault("DisplayFormat", "{0:n2} %")]
        [ModelDefault("EditMask", "n2")]
        public decimal Participacion
        {
            get { return fParticipacion; }
            set { SetPropertyValue<decimal>("Participacion", ref fParticipacion, value); }
        }

        [VisibleInListView(true)]
        [Appearance("ComisionPrimaShow", "NOT Rol.LlevaComisionPrima", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        [ModelDefault("DisplayFormat", "{0:n2} %")]
        [ModelDefault("EditMask", "n2")]
        public decimal ComisionPrima
        {
            get { return fComisionPrima; }
            set { SetPropertyValue<decimal>("ComisionPrima", ref fComisionPrima, value); }
        }

        [VisibleInListView(true)]
        [Appearance("ComisionCobranzaShow", "NOT Rol.LlevaComisionCobranza", Visibility = ViewItemVisibility.Hide,
            Context = "DetailView")]
        [ModelDefault("DisplayFormat", "{0:n2} %")]
        [ModelDefault("EditMask", "n2")]
        public decimal ComisionCobranza
        {
            get { return fComisionCobranza; }
            set { SetPropertyValue<decimal>("ComisionCobranza", ref fComisionCobranza, value); }
        }

        [Browsable(false)]
        protected XPCollection<Persona> IntervinientesPosibles
        {
            get { return fIntervinientesPosibles ?? (fIntervinientesPosibles = new XPCollection<Persona>(Session)); }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("IntervinientesPosibles")]
        [RuleRequiredField(TargetCriteria = "")]
        public Persona Interviniente
        {
            get { return fInterviniente; }
            set
            {
                var fOldVal = fInterviniente;
                SetPropertyValue("Interviniente", ref fInterviniente, value);

                if (CanRaiseOnChanged)
                {
                    //Si cambió la persona, reiniciar valores dependientes de la persona
                    Direccion = null;
                    Identificacion = null;
                    Contacto = null;
                    CategoriaImpuestos = null;

                    if (fInterviniente != null)
                    {
                        //Si hay una dirección primaria, predeterminarla
                        if (Interviniente.DireccionPrimaria != null)
                            Direccion = Interviniente.DireccionPrimaria;

                        //Si hay una unica dirección, identif, contacto o categoria imp., predeterminarlos
                        else if (Interviniente.Direcciones.Count == 1)
                            Direccion = Interviniente.Direcciones[0];

                        if (Interviniente.Identificaciones.Count == 1)
                            Identificacion = Interviniente.Identificaciones[0];
                        if (Interviniente.Contactos.Count == 1)
                            Contacto = Interviniente.Contactos[0];
                        if (Interviniente.DatosImpositivos.Count == 1)
                            CategoriaImpuestos = Interviniente.DatosImpositivos[0].Categoria;
                    }
                }
            }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("Interviniente.Direcciones")]
        public PersonaDireccion Direccion
        {
            get { return fDireccion; }
            set { SetPropertyValue("Direccion", ref fDireccion, value); }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("Interviniente.Identificaciones")]
        public Identificacion Identificacion
        {
            get { return fIdentificacion; }
            set { SetPropertyValue("Identificacion", ref fIdentificacion, value); }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("Interviniente.DatosImpositivos")]
        public Categoria CategoriaImpuestos
        {
            get { return fCategoriaImpuestos; }
            set { SetPropertyValue("CategoriaImpuestos", ref fCategoriaImpuestos, value); }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("Interviniente.Contactos")]
        public PersonaContacto Contacto
        {
            get { return fContacto; }
            set { SetPropertyValue("Contacto", ref fContacto, value); }
        }

        private void FilterIntervinientesPosibles()
        {
            var typeName = Rol != null ? Rol.TipoRolPersona : "";
            IntervinientesPosibles.Criteria = string.IsNullOrEmpty(typeName)
                ? null
                : CriteriaOperator.Parse("[<" + typeName + ">][^.Oid = Persona.Oid]");
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            TipoMovimiento = TipoMovimiento.Alta;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            FilterIntervinientesPosibles();
        }

        public static bool ValidateIntervinientes(IEnumerable<DocumentoIntervinienteBase> col,
            out string errorMessageTemplate)
        {
            var ld = new Dictionary<int, decimal>();

            foreach (var interviniente in col.Where(interviniente => interviniente.Rol.LlevaParticipacion))
            {
                if (!ld.ContainsKey(interviniente.Rol.Oid))
                    ld[interviniente.Rol.Oid] = 0;

                ld[interviniente.Rol.Oid] += interviniente.Participacion;
            }

            if (ld.Any(l => l.Value != 100))
            {
                errorMessageTemplate = "La participación de intervinientes debe sumar 100% para cada rol";
                return false;
            }

            errorMessageTemplate = "";
            return true;
        }
    }
}