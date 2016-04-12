using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Editors;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace FDIT.Core.Seguros
{
    public enum RolClase
    {
        Aseguradora = 1,
        Tomador = 2,
        Asegurado = 3,
        Intermediario = 4,
        Oficial = 5,
        Productor = 6,
        PersonaRelacionada = 7
    }

    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Rol en seguros")]
    [Persistent("seguros.Rol")]
    public class Rol : BasicObject
    {
        private RolClase fClase;
        private string fCodigo;
        private string fIconoUrl;
        private bool fLlevaComisionCobranza;
        private bool fLlevaComisionPrima;
        private bool fLlevaParticipacion;
        private string fNombre;
        private int fOrden;
        private string fTipoRolPersona;

        [Browsable(false)] private Dictionary<string, string> fTiposRolesPersonas;

        public Rol(Session session)
            : base(session)
        {
        }

        [Size(50)]
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

        public RolClase Clase
        {
            get { return fClase; }
            set { SetPropertyValue("Clase", ref fClase, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        public bool LlevaParticipacion
        {
            get { return fLlevaParticipacion; }
            set { SetPropertyValue("LlevaParticipacion", ref fLlevaParticipacion, value); }
        }

        public bool LlevaComisionPrima
        {
            get { return fLlevaComisionPrima; }
            set { SetPropertyValue("LlevaComisionPrima", ref fLlevaComisionPrima, value); }
        }

        public bool LlevaComisionCobranza
        {
            get { return fLlevaComisionCobranza; }
            set { SetPropertyValue("LlevaComisionCobranza", ref fLlevaComisionCobranza, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string IconoUrl
        {
            get { return fIconoUrl; }
            set { SetPropertyValue("IconoUrl", ref fIconoUrl, value); }
        }

        [Browsable(false)]
        public Dictionary<string, string> TiposRolesPersonas
        {
            get
            {
                if (fTiposRolesPersonas == null)
                {
                    fTiposRolesPersonas = new Dictionary<string, string>();

                    //TODO: los objetos XPO no deberian referenciar la plataforma XAF; cambiar para usar los tipos XPO
                    var documentoItemType =
                        XafTypesInfo.Instance.PersistentTypes.First(info => info.Type == typeof (Personas.Rol));

                    foreach (var type in documentoItemType.Descendants.OrderBy(info => info.Name))
                    {
                        var desc = type.Name;
                        var displayNameAttr = type.FindAttribute<DisplayNameAttribute>();
                        if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                        fTiposRolesPersonas.Add(type.Type.FullName, desc);
                    }
                }
                return fTiposRolesPersonas;
            }
        }

        [ModelDefault("PropertyEditorType", "FDIT.Core.Module.Win.Editors.WinSimpleComboBoxEditor")]
        [SimpleComboBox(AllowEdit = false, AllowNullValues = true, DataSourceProperty = "TiposRolesPersonas",
            DisplayValues = false)]
        [DisplayName("Restricción de personas")]
        public string TipoRolPersona
        {
            get { return fTipoRolPersona; }
            set { SetPropertyValue("TipoRolPersona", ref fTipoRolPersona, value); }
        }
    }
}