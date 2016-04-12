using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Editors;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace FDIT.Core.Seguros
{
    public enum TipoItemSubramo
    {
        General = 1,
        Automotores = 2,
        Vida = 3,
        VidaGeneral = 4
    }

    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Subramo de seguros")]
    [Persistent("seguros.Subramo")]
    public class Subramo : BasicObject
    {
        private string fCodigo;
        private string fNombre;
        private int fOrden;
        private Ramo fRamo;
        private string fTipoItem;
        private Dictionary<string, string> fTiposItemPosibles;

        public Subramo(Session session) : base(session)
        {
        }

        [Association]
        public Ramo Ramo
        {
            get { return fRamo; }
            set { SetPropertyValue("Ramo", ref fRamo, value); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Index(1)]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        [Browsable(false)]
        public Dictionary<string, string> TiposItemPosibles
        {
            get
            {
                if (fTiposItemPosibles == null)
                {
                    fTiposItemPosibles = new Dictionary<string, string>();

                    //TODO: los objetos XPO no deberian referenciar la plataforma XAF; cambiar para usar los tipos XPO
                    var documentoItemType =
                        XafTypesInfo.Instance.PersistentTypes.First(info => info.Type == typeof (DocumentoItem));
                    var desc = documentoItemType.Name;
                    var displayNameAttr = documentoItemType.FindAttribute<DisplayNameAttribute>();
                    if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                    fTiposItemPosibles.Add(documentoItemType.Type.FullName, desc);

                    foreach (var type in documentoItemType.Descendants.OrderBy(info => info.Name))
                    {
                        desc = type.Name;
                        displayNameAttr = type.FindAttribute<DisplayNameAttribute>();
                        if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                        fTiposItemPosibles.Add(type.Type.FullName, desc);
                    }
                }
                return fTiposItemPosibles;
            }
        }

        [SimpleComboBox(AllowEdit = false, AllowNullValues = true, DataSourceProperty = "TiposItemPosibles",
            DisplayValues = false)]
        public string TipoItem
        {
            get { return fTipoItem; }
            set { SetPropertyValue("TipoItem", ref fTipoItem, value); }
        }

        [Association]
        public XPCollection<Titulo> Titulos
        {
            get { return GetCollection<Titulo>("Titulos"); }
        }

        [Association]
        public XPCollection<DocumentoItemClase> DocumentosItemsClases
        {
            get { return GetCollection<DocumentoItemClase>("DocumentosItemsClases"); }
        }

        [Association]
        public XPCollection<Interes> Intereses
        {
            get { return GetCollection<Interes>("Intereses"); }
        }
    }
}