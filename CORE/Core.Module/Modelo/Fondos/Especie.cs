using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Editors;
using FDIT.Core.Sistema;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace FDIT.Core.Fondos
{
    [Persistent(@"fondos.Especie")]
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Especies")]
    public class Especie : BasicObject, IObjetoPorEmpresa
    {
        private string fCodigo;
        private Empresa fEmpresa;
        private string fEspecie;
        private Image fEspecieIcono;
        private bool fExigeValor;
        private bool fExpresaMoneda;

        private EspecieInstrumento fInstrumento;
        private Moneda fMoneda;
        private int fOrden;
        private Dictionary<string, string> fTiposValoresPosibles;
        private string fTipoValor;

        public Especie(Session session) : base(session)
        {
        }

        public Moneda Moneda
        {
            get { return fMoneda; }
            set { SetPropertyValue("Moneda", ref fMoneda, value); }
        }

        [Association(@"EspeciesReferencesEspeciesInstrumentos")]
        public EspecieInstrumento Instrumento
        {
            get { return fInstrumento; }
            set { SetPropertyValue("Instrumento", ref fInstrumento, value); }
        }

        [Size(20)]
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
            get { return fEspecie; }
            set { SetPropertyValue("Nombre", ref fEspecie, value); }
        }

        public bool ExpresaMoneda
        {
            get { return fExpresaMoneda; }
            set { SetPropertyValue("ExpresaMoneda", ref fExpresaMoneda, value); }
        }

        [Browsable(false)]
        public Dictionary<string, string> TiposValoresPosibles
        {
            get
            {
                if (fTiposValoresPosibles == null)
                {
                    fTiposValoresPosibles = new Dictionary<string, string>();

                    //TODO: los objetos XPO no deberian referenciar la plataforma XAF; cambiar para usar los tipos XPO
                    var documentoItemType =
                        XafTypesInfo.Instance.PersistentTypes.First(info => info.Type == typeof (Valor));
                    var desc = documentoItemType.Name;
                    var displayNameAttr = documentoItemType.FindAttribute<DisplayNameAttribute>();
                    if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                    fTiposValoresPosibles.Add(documentoItemType.Type.FullName, desc);

                    foreach (var type in documentoItemType.Descendants.OrderBy(info => info.Name))
                    {
                        desc = type.Name;
                        displayNameAttr = type.FindAttribute<DisplayNameAttribute>();
                        if (displayNameAttr != null) desc = displayNameAttr.DisplayName;

                        fTiposValoresPosibles.Add(type.Type.FullName, desc);
                    }
                }
                return fTiposValoresPosibles;
            }
        }

        [SimpleComboBox(AllowEdit = false, AllowNullValues = true, DataSourceProperty = "TiposValoresPosibles",
            DisplayValues = false)]
        public string TipoValor
        {
            get { return fTipoValor; }
            set { SetPropertyValue("TipoValor", ref fTipoValor, value); }
        }

        public bool ExigeValor
        {
            get { return fExigeValor; }
            set { SetPropertyValue("ExigeValor", ref fExigeValor, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
        
        [Browsable(false)]
        public Empresa Empresa
        {
            get { return fEmpresa; }
            set { SetPropertyValue("Empresa", ref fEmpresa, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}