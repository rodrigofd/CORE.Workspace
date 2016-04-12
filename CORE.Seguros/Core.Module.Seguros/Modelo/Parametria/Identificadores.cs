using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Gestion;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [System.ComponentModel.DisplayName("Preferencias de Seguros")]
    [Persistent("seguros.Identificadores")]
    public class Identificadores : IdentificadoresBase<Identificadores>
    {
        private bool fCoaseguroProrrateoImportes;
        private DocumentoEstado fDocumentoEstadoFacturacion;
        private DocumentoEstado fDocumentoEstadoInicial;
        private DocumentoTipo fDocumentoTipoEndoso;
        private DocumentoTipo fDocumentoTipoEndosoRefaArt;
        private DocumentoTipo fDocumentoTipoEndosoRefaCaucion;
        private DocumentoTipo fDocumentoTipoEndosoRefaVO;
        private DocumentoTipo fDocumentoTipoRenovacion;
        private int fEndososDuplicaIntervinientes;
        private int fEndososDuplicaItems;
        private int fEndososDuplicaItemsIntervinientes;
        private decimal fImpuestos1Tasa;
        private decimal fIva1Tasa;
        private Ramo fRamoART;
        private Ramo fRamoCaucion;
        private Ramo fRamoVehiculos;
        private Ramo fRamoVO;
        private int fRenovacionesDuplicaIntervinientes;
        private int fRenovacionesDuplicaItems;
        private int fRenovacionesDuplicaItemsIntervinientes;
        private Rol fRolAsegurado;
        private Rol fRolAseguradora;
        private Rol fRolBeneficiario;
        private Rol fRolContacto;
        private Rol fRolFacturar_a;
        private Rol fRolIntermediario;
        private Rol fRolOrganizador;
        private Rol fRolProductor;
        private Rol fRolTomador;
        private Talonario fTalonarioDocumentos;
        private ComprobanteTipo fTipoComprobanteFcDocumento;
        private ComprobanteTipo fTipoComprobanteLiqDocumento;
        private ComprobanteTipo fTipoComprobanteNcDocumento;
        private VentaTipo fVentaTipoEndoso;
        private VentaTipo fVentaTipoRenovacion;

        public Identificadores(Session session) : base(session)
        {
        }

        public VentaTipo VentaTipoRenovacion
        {
            get { return fVentaTipoRenovacion; }
            set { SetPropertyValue("VentaTipoRenovacion", ref fVentaTipoRenovacion, value); }
        }

        public VentaTipo VentaTipoEndoso
        {
            get { return fVentaTipoEndoso; }
            set { SetPropertyValue("VentaTipoEndoso", ref fVentaTipoEndoso, value); }
        }

        public DocumentoTipo DocumentoTipoRenovacion
        {
            get { return fDocumentoTipoRenovacion; }
            set { SetPropertyValue("DocumentoTipoRenovacion", ref fDocumentoTipoRenovacion, value); }
        }

        public DocumentoTipo DocumentoTipoEndoso
        {
            get { return fDocumentoTipoEndoso; }
            set { SetPropertyValue("DocumentoTipoEndoso", ref fDocumentoTipoEndoso, value); }
        }

        public DocumentoTipo DocumentoTipoEndosoRefaArt
        {
            get { return fDocumentoTipoEndosoRefaArt; }
            set { SetPropertyValue("DocumentoTipoEndosoRefaArt", ref fDocumentoTipoEndosoRefaArt, value); }
        }

        public DocumentoTipo DocumentoTipoEndosoRefaCaucion
        {
            get { return fDocumentoTipoEndosoRefaCaucion; }
            set { SetPropertyValue("DocumentoTipoEndosoRefaCaucion", ref fDocumentoTipoEndosoRefaCaucion, value); }
        }

        public DocumentoTipo DocumentoTipoEndosoRefaVO
        {
            get { return fDocumentoTipoEndosoRefaVO; }
            set { SetPropertyValue("DocumentoTipoEndosoRefaVO", ref fDocumentoTipoEndosoRefaVO, value); }
        }

        public Rol RolOrganizador
        {
            get { return fRolOrganizador; }
            set { SetPropertyValue("RolOrganizador", ref fRolOrganizador, value); }
        }

        public Rol RolAseguradora
        {
            get { return fRolAseguradora; }
            set { SetPropertyValue("RolAseguradora", ref fRolAseguradora, value); }
        }

        public Rol RolProductor
        {
            get { return fRolProductor; }
            set { SetPropertyValue("RolProductor", ref fRolProductor, value); }
        }

        public Rol RolFacturarA
        {
            get { return fRolFacturar_a; }
            set { SetPropertyValue("RolFacturarA", ref fRolFacturar_a, value); }
        }

        public Rol RolTomador
        {
            get { return fRolTomador; }
            set { SetPropertyValue("RolTomador", ref fRolTomador, value); }
        }

        public Rol RolAsegurado
        {
            get { return fRolAsegurado; }
            set { SetPropertyValue("RolAsegurado", ref fRolAsegurado, value); }
        }

        public Rol RolBeneficiario
        {
            get { return fRolBeneficiario; }
            set { SetPropertyValue("RolBeneficiario", ref fRolBeneficiario, value); }
        }

        public Rol RolIntermediario
        {
            get { return fRolIntermediario; }
            set { SetPropertyValue("RolIntermediario", ref fRolIntermediario, value); }
        }

        public Rol RolContacto
        {
            get { return fRolContacto; }
            set { SetPropertyValue("RolContacto", ref fRolContacto, value); }
        }

        public Ramo RamoART
        {
            get { return fRamoART; }
            set { SetPropertyValue("RamoART", ref fRamoART, value); }
        }

        public Ramo RamoCaucion
        {
            get { return fRamoCaucion; }
            set { SetPropertyValue("RamoCaucion", ref fRamoCaucion, value); }
        }

        public Ramo RamoVehiculos
        {
            get { return fRamoVehiculos; }
            set { SetPropertyValue("RamoVehiculos", ref fRamoVehiculos, value); }
        }

        public Ramo RamoVO
        {
            get { return fRamoVO; }
            set { SetPropertyValue("RamoVO", ref fRamoVO, value); }
        }

        public decimal Iva1Tasa
        {
            get { return fIva1Tasa; }
            set { SetPropertyValue<decimal>("Iva1Tasa", ref fIva1Tasa, value); }
        }

        public decimal Impuestos1Tasa
        {
            get { return fImpuestos1Tasa; }
            set { SetPropertyValue<decimal>("Impuestos1Tasa", ref fImpuestos1Tasa, value); }
        }

        public int EndososDuplicaItems
        {
            get { return fEndososDuplicaItems; }
            set { SetPropertyValue<int>("EndososDuplicaItems", ref fEndososDuplicaItems, value); }
        }

        public int EndososDuplicaItemsIntervinientes
        {
            get { return fEndososDuplicaItemsIntervinientes; }
            set
            {
                SetPropertyValue<int>("EndososDuplicaItemsIntervinientes", ref fEndososDuplicaItemsIntervinientes, value);
            }
        }

        public int EndososDuplicaIntervinientes
        {
            get { return fEndososDuplicaIntervinientes; }
            set { SetPropertyValue<int>("EndososDuplicaIntervinientes", ref fEndososDuplicaIntervinientes, value); }
        }

        public int RenovacionesDuplicaItems
        {
            get { return fRenovacionesDuplicaItems; }
            set { SetPropertyValue<int>("RenovacionesDuplicaItems", ref fRenovacionesDuplicaItems, value); }
        }

        public int RenovacionesDuplicaItemsIntervinientes
        {
            get { return fRenovacionesDuplicaItemsIntervinientes; }
            set
            {
                SetPropertyValue<int>("RenovacionesDuplicaItemsIntervinientes",
                    ref fRenovacionesDuplicaItemsIntervinientes, value);
            }
        }

        public int RenovacionesDuplicaIntervinientes
        {
            get { return fRenovacionesDuplicaIntervinientes; }
            set
            {
                SetPropertyValue<int>("RenovacionesDuplicaIntervinientes", ref fRenovacionesDuplicaIntervinientes, value);
            }
        }

        public DocumentoEstado DocumentoEstadoInicial
        {
            get { return fDocumentoEstadoInicial; }
            set { SetPropertyValue("DocumentoEstadoInicial", ref fDocumentoEstadoInicial, value); }
        }

        public DocumentoEstado DocumentoEstadoFacturacion
        {
            get { return fDocumentoEstadoFacturacion; }
            set { SetPropertyValue("DocumentoEstadoFacturacion", ref fDocumentoEstadoFacturacion, value); }
        }

        public ComprobanteTipo TipoComprobanteFcDocumento
        {
            get { return fTipoComprobanteFcDocumento; }
            set { SetPropertyValue("TipoComprobanteFcDocumento", ref fTipoComprobanteFcDocumento, value); }
        }

        public ComprobanteTipo TipoComprobanteNcDocumento
        {
            get { return fTipoComprobanteNcDocumento; }
            set { SetPropertyValue("TipoComprobanteNcDocumento", ref fTipoComprobanteNcDocumento, value); }
        }

        public ComprobanteTipo TipoComprobanteLiqDocumento
        {
            get { return fTipoComprobanteLiqDocumento; }
            set { SetPropertyValue("TipoComprobanteLiqDocumento", ref fTipoComprobanteLiqDocumento, value); }
        }

        public bool CoaseguroProrrateoImportes
        {
            get { return fCoaseguroProrrateoImportes; }
            set { SetPropertyValue("CoaseguroProrrateoImportes", ref fCoaseguroProrrateoImportes, value); }
        }

        public Talonario TalonarioDocumentos
        {
            get { return fTalonarioDocumentos; }
            set { SetPropertyValue("TalonarioDocumentos", ref fTalonarioDocumentos, value); }
        }
    }
}