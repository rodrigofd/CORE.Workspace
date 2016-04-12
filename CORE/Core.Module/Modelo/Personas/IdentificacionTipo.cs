using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Regionales;

namespace FDIT.Core.Personas
{
    [Persistent(@"personas.IdentificacionTipo")]
    [DefaultProperty("Descripcion")]
    [FiltroPorPais(true)]
    [System.ComponentModel.DisplayName("Tipo de identificación")]
    public class IdentificacionTipo : BasicObject
    {
        //TODO: parametrizar
        public const int CUIT = 80;
        //TODO: parametrizar
        public const int NUMERO_IIBB = 101;

        private bool fAdmiteDuplicados;
        private IdentificacionClase fClase;
        private string fCodigo;
        private string fFmt;
        private string fNombre;
        private Pais fPais;
        private bool fValida;
        private string fValidaJs;

        public IdentificacionTipo(Session session) : base(session)
        {
        }

        [VisibleInDetailView(false)]
        //[PersistentAlias( "IIf(IsNull(Codigo), '', Codigo + ' - ') + Nombre" )]
        [PersistentAlias("ISNULL(Codigo,'')")]
        public string Descripcion
        {
            get { return Convert.ToString(EvaluateAlias("Descripcion")); }
        }

        [Association(@"IdentificacionesTiposReferencesIdentificacionesClases", typeof (IdentificacionClase))]
        public IdentificacionClase Clase
        {
            get { return fClase; }
            set { SetPropertyValue("Clase", ref fClase, value); }
        }

        [Association(@"IdentificacionesTiposReferencesPaises", typeof (Pais))]
        public Pais Pais
        {
            get { return fPais; }
            set { SetPropertyValue("Pais", ref fPais, value); }
        }

        [Size(20)]
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

        public bool ValidacionActiva
        {
            get { return fValida; }
            set { SetPropertyValue("Valida", ref fValida, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string ValidacionLogica
        {
            get { return fValidaJs; }
            set { SetPropertyValue("ValidaJs", ref fValidaJs, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Formato
        {
            get { return fFmt; }
            set { SetPropertyValue("Fmt", ref fFmt, value); }
        }

        public bool AdmiteDuplicados
        {
            get { return fAdmiteDuplicados; }
            set { SetPropertyValue("AdmiteDuplicados", ref fAdmiteDuplicados, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}