#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Regionales;
using FDIT.Core.Seguridad;

#endregion

namespace FDIT.Core.Personas
{
    [DefaultProperty("Nombre")]
    [Persistent(@"personas.Persona")]
    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Persona")]
    [FiltroPorPais]
    //<Appearance("BoldRule", AppearanceItemType := "ViewItem", FontStyle:=FontStyle.Bold, FontColor:="Yellow", TargetItems:="Name", Context:="DetailView")>
    //[Appearance("BoldRule", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Price>50", Context = "ListView", BackColor = "Red", FontColor = "Maroon", Priority = 2)]
    //[Appearance("DetailViewBoldRule", AppearanceItemType = "ViewItem", TargetItems = "*", FontStyle = FontStyle.Bold, Context = "DetailView", Priority = 0)]
    [Appearance("RedTipoRule", AppearanceItemType = "ViewItem", TargetItems = "Nombre", Criteria = "Tipo = 'Virtual'",
        BackColor = "Red", Context = "ListView")]
    public class Persona : BasicObject
    {
        private DateTime? fAniversarioFecha;
        private string fApellidosMaterno;
        private string fApellidosPaterno;
        private PersonaDireccion fDireccionPrimaria;
        private int? fEdad;
        private DateTime fFallecimientoFecha;
        private Pais fFallecimientoPais;
        private DateTime? fNacimientoFecha;
        private Pais fNacimientoPais;
        private string fNombre;
        private string fNombreCompletoAlias;
        private string fNombreFantasia;
        private string fNombrePila;
        private string fNotas;
        private XPCollection<Relacion> fRelaciones;
        private BindingList<Rol> fRoles;
        private string fSegundoNombre;
        private Sexo? fSexo;
        private TipoPersona fTipo;
        private string fTratamiento;

        public Persona(Session session)
            : base(session)
        {
        }

        [VisibleInListView(true)]
        [VisibleInDetailView(false)]
        [Index(0)]
        public override Image Icono
        {
            get
            {
                var descriptor = new EnumDescriptor(typeof (TipoPersona));
                var imageInfo = descriptor.GetImageInfo(Tipo);
                return imageInfo.Image;
            }
        }

        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Tipo")]
        public TipoPersona Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        [Appearance("tratamiento_tratamiento", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [Size(20)]
        [System.ComponentModel.DisplayName("Tratamiento")]
        public string Tratamiento
        {
            get { return fTratamiento; }
            set { SetPropertyValue("Tratamiento", ref fTratamiento, value); }
        }

        [Appearance("tratamiento_apellidos_paterno", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Apellidos paternos")]
        public string ApellidosPaterno
        {
            get { return fApellidosPaterno; }
            set
            {
                SetPropertyValue("ApellidosPaterno", ref fApellidosPaterno, value);

                if (CanRaiseOnChanged)
                    ActualizarNombre();
            }
        }

        [Appearance("tratamiento_apellidos_materno", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Apellidos maternos")]
        public string ApellidosMaterno
        {
            get { return fApellidosMaterno; }
            set
            {
                SetPropertyValue("ApellidosMaterno", ref fApellidosMaterno, value);

                if (CanRaiseOnChanged)
                {
                    ActualizarNombre();
                }
            }
        }

        [Appearance("tratamiento_nombres", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Nombre")]
        public string NombrePila
        {
            get { return fNombrePila; }
            set
            {
                SetPropertyValue("NombrePila", ref fNombrePila, value);

                if (CanRaiseOnChanged)
                {
                    ActualizarNombre();
                }
            }
        }

        [Appearance("tratamiento_segundo_nombre", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Segundo Nombre")]
        public string SegundoNombre
        {
            get { return fSegundoNombre; }
            set
            {
                SetPropertyValue("SegundoNombre", ref fSegundoNombre, value);

                if (CanRaiseOnChanged)
                {
                    ActualizarNombre();
                }
            }
        }

        [Appearance("personeria_nombre", "Tipo <> 'Juridica'", Enabled = false)]
        [RuleRequiredField]
        [System.ComponentModel.DisplayName("Nombre")]
        public string Nombre
        {
            get { return fNombre; }
            set
            {
                SetPropertyValue("Nombre", ref fNombre, value);

                if (CanRaiseOnChanged)
                {
                    ActualizarNombreCompletoAlias();
                }
            }
        }

        [Appearance("tratamiento_nombre_fantasia", "Tipo <> 'Juridica'", Visibility = ViewItemVisibility.Hide)]
        [System.ComponentModel.DisplayName("Nombre fantasía/apodo")]
        public string NombreFantasia
        {
            get { return fNombreFantasia; }
            set
            {
                SetPropertyValue("NombreFantasia", ref fNombreFantasia, value);

                if (CanRaiseOnChanged)
                {
                    ActualizarNombreCompletoAlias();
                }
            }
        }

        [Browsable(false)]
        [System.ComponentModel.DisplayName("Nombre completo (alias)")]
        public string NombreCompletoAlias
        {
            get { return fNombreCompletoAlias; }
            set { SetPropertyValue("NombreCompletoAlias", ref fNombreCompletoAlias, value); }
        }

        [System.ComponentModel.DisplayName("Fecha de nacimiento")]
        public DateTime? NacimientoFecha
        {
            get { return fNacimientoFecha; }
            set
            {
                SetPropertyValue("NacimientoFecha", ref fNacimientoFecha, value);

                if (CanRaiseOnChanged)
                {
                    var edad = 0;
                    if (value.HasValue)
                    {
                        var now = DateTime.Today;
                        edad = now.Year - value.Value.Year;
                        if (value > now.AddYears(-edad)) edad--;
                    }
                    Edad = edad;
                }
            }
        }

        [System.ComponentModel.DisplayName("Fecha de aniversario")]
        public DateTime? AniversarioFecha
        {
            get { return fAniversarioFecha; }
            set { SetPropertyValue("AniversarioFecha", ref fAniversarioFecha, value); }
        }

        [Appearance("personeria_edad", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ReadOnly(true)]
        [Browsable(false)]
        [System.ComponentModel.DisplayName("Edad")]
        public int? Edad
        {
            get { return fEdad; }
            set { SetPropertyValue("Edad", ref fEdad, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        [System.ComponentModel.DisplayName("País de nacimiento")]
        public Pais NacimientoPais
        {
            get { return fNacimientoPais; }
            set { SetPropertyValue("NacimientoPais", ref fNacimientoPais, value); }
        }

        [Appearance("personeria_sexo", "Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide)]
        [ImmediatePostData]
        [System.ComponentModel.DisplayName("Sexo")]
        public Sexo? Sexo
        {
            get { return fSexo; }
            set { SetPropertyValue("Sexo", ref fSexo, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        [System.ComponentModel.DisplayName("Fecha de fallecimiento")]
        public DateTime FallecimientoFecha
        {
            get { return fFallecimientoFecha; }
            set { SetPropertyValue<DateTime>("FallecimientoFecha", ref fFallecimientoFecha, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        [System.ComponentModel.DisplayName("País de fallecimiento")]
        public Pais FallecimientoPais
        {
            get { return fFallecimientoPais; }
            set { SetPropertyValue("FallecimientoPais", ref fFallecimientoPais, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return fNotas; }
            set { SetPropertyValue("Notas", ref fNotas, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceProperty("Direcciones")]
        [System.ComponentModel.DisplayName("Dirección primaria")]
        public PersonaDireccion DireccionPrimaria
        {
            get { return fDireccionPrimaria; }
            set { SetPropertyValue("DireccionPrimaria", ref fDireccionPrimaria, value); }
        }

        [Delayed(true)]
        [ValueConverter(typeof (ImageValueConverter))]
        [System.ComponentModel.DisplayName("Imagen")]
        public Image Imagen
        {
            get { return GetDelayedPropertyValue<Image>("Imagen"); }
            set { SetDelayedPropertyValue("Imagen", value); }
        }

        [Delayed(true)]
        [ValueConverter(typeof (ImageValueConverter))]
        [System.ComponentModel.DisplayName("Imagen (firma)")]
        public Image ImagenFirma
        {
            get { return GetDelayedPropertyValue<Image>("ImagenFirma"); }
            set { SetDelayedPropertyValue("ImagenFirma", value); }
        }

        [Delayed(true)]
        [ValueConverter(typeof (ImageValueConverter))]
        [System.ComponentModel.DisplayName("Imagen (impresiones)")]
        public Image ImagenImp
        {
            get { return GetDelayedPropertyValue<Image>("ImagenImp"); }
            set { SetDelayedPropertyValue("ImagenImp", value); }
        }

        [Aggregated]
        [Association("Personas-PersonasDirecciones")]
        [System.ComponentModel.DisplayName("Direcciones")]
        public XPCollection<PersonaDireccion> Direcciones
        {
            get { return GetCollection<PersonaDireccion>("Direcciones"); }
        }

        [Aggregated]
        [Association(@"IdentificacionesReferencesPersonas", typeof (Identificacion))]
        [System.ComponentModel.DisplayName("Identificaciones")]
        public XPCollection<Identificacion> Identificaciones
        {
            get { return GetCollection<Identificacion>("Identificaciones"); }
        }

        [Aggregated]
        [Association(@"PersonasContactosReferencesPersonas", typeof (PersonaContacto))]
        [System.ComponentModel.DisplayName("Contactos")]
        public XPCollection<PersonaContacto> Contactos
        {
            get { return GetCollection<PersonaContacto>("Contactos"); }
        }

        [Aggregated]
        [Association(@"PersonasImpuestosReferencesPersonas", typeof (PersonaImpuesto))]
        [System.ComponentModel.DisplayName("Datos impositivos")]
        public XPCollection<PersonaImpuesto> DatosImpositivos
        {
            get { return GetCollection<PersonaImpuesto>("DatosImpositivos"); }
        }

        [Aggregated]
        [Association(@"PersonasPropiedadesReferencesPersonas", typeof (PersonaPropiedad))]
        [System.ComponentModel.DisplayName("Propiedades")]
        public XPCollection<PersonaPropiedad> Propiedades
        {
            get { return GetCollection<PersonaPropiedad>("Propiedades"); }
        }

        [Browsable(false)]
        [Association(@"RelacionesReferencesPersonas-Vinculante", typeof (Relacion))]
        [System.ComponentModel.DisplayName("Relaciones (vinculante)")]
        public XPCollection<Relacion> RelacionesVinculante
        {
            get { return GetCollection<Relacion>("RelacionesVinculante"); }
        }

        [Browsable(false)]
        [Association(@"RelacionesReferencesPersonas-Vinculado", typeof (Relacion))]
        [System.ComponentModel.DisplayName("Relaciones (vinculado)")]
        public XPCollection<Relacion> RelacionesVinculado
        {
            get { return GetCollection<Relacion>("RelacionesVinculado"); }
        }

        [Aggregated]
        [CollectionOperationSet(AllowAdd = false, AllowRemove = true)]
        public XPCollection<Relacion> Relaciones
        {
            get
            {
                if (fRelaciones == null)
                {
                    var criteria = GroupOperator.Combine(GroupOperatorType.Or,
                        RelacionesVinculante.GetRealFetchCriteria(), RelacionesVinculado.GetRealFetchCriteria());
                    fRelaciones = new XPCollection<Relacion>(Session, criteria);
                }
                return fRelaciones;
            }
        }

        [System.ComponentModel.DisplayName("Roles")]
        public BindingList<Rol> Roles
        {
            get
            {
                if (fRoles == null)
                {
                    fRoles = new BindingList<Rol>();

                    foreach (
                        var item in
                            Session.TypesManager.AllTypes.Where(
                                type => type.Key.BaseClass != null && type.Key.BaseClass.ClassType == typeof (Rol))
                                .Select(
                                    type =>
                                        Session.GetObjects(type.Key, new BinaryOperator("Persona.Oid", Oid), null, 0,
                                            true, false))
                                .SelectMany(roles => roles.Cast<Rol>()))
                    {
                        fRoles.Add(item);
                    }
                }

                return fRoles;
            }
        }

        public void ActualizarNombre()
        {
            if (Tipo == TipoPersona.Fisica)
            {
                var nombre = ApellidosPaterno;
                if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(ApellidosMaterno)) nombre += " ";
                nombre += ApellidosMaterno;
                if (!string.IsNullOrWhiteSpace(nombre)) nombre += ", ";
                nombre += NombrePila;
                if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(SegundoNombre)) nombre += " ";
                nombre += SegundoNombre;

                Nombre = nombre;
            }

            ActualizarNombreCompletoAlias();
        }

        private void ActualizarNombreCompletoAlias()
        {
            NombreCompletoAlias = Nombre +
                                  (string.IsNullOrWhiteSpace(NombreFantasia) ? "" : " (" + NombreFantasia + ")");
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Tipo = TipoPersona.Fisica;
            if (CoreAppLogonParameters.Instance.EmpresaActual(Session) != null)
                NacimientoPais = Identificadores.GetInstance(Session).PaisPredeterminado;
        }
    }
}