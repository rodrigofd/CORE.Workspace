using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Regionales;
using FDIT.Core.Seguridad;
using Rol = FDIT.Core.Personas.Rol;

namespace FDIT.Core.Sistema
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    [Persistent(@"sistema.Empresa")]
    [System.ComponentModel.DisplayName("Empresa")]
    public class Empresa : Rol
    {
        private string fColorFondo;
        private PaisIdioma fCulturaPredeterminada;
        private Idioma fIdiomaPredeterminado;
        private DateTime fLicenciaDesde;
        private DateTime fLicenciaHasta;
        private int fMaxAccesosErroneos;
        private string fPieInformesGral;
        private TipoLicencia fTipoLicencia;

        public Empresa(Session session) : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Persona.Nombre")]
        public string Descripcion => Convert.ToString(EvaluateAlias("Descripcion"));

        public TipoLicencia TipoLicencia
        {
            get { return fTipoLicencia; }
            set { SetPropertyValue("TipoLicencia", ref fTipoLicencia, value); }
        }

        public DateTime LicenciaDesde
        {
            get { return fLicenciaDesde; }
            set { SetPropertyValue<DateTime>("Inicio", ref fLicenciaDesde, value); }
        }

        public DateTime LicenciaHasta
        {
            get { return fLicenciaHasta; }
            set { SetPropertyValue<DateTime>("Fin", ref fLicenciaHasta, value); }
        }

        public int MaxAccesosErroneos
        {
            get { return fMaxAccesosErroneos; }
            set { SetPropertyValue<int>("MaxAccesosErroneos", ref fMaxAccesosErroneos, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string PieInformesGral
        {
            get { return fPieInformesGral; }
            set { SetPropertyValue("PieInformesGral", ref fPieInformesGral, value); }
        }

        [Delayed(true)]
        [ValueConverter(typeof (ImageValueConverter))]
        [System.ComponentModel.DisplayName("Imagen Logo")]
        public Image ImagenLogo
        {
            get { return GetDelayedPropertyValue<Image>("ImagenLogo"); }
            set { SetDelayedPropertyValue("ImagenLogo", value); }
        }

        public Idioma IdiomaPredeterminado
        {
            get { return fIdiomaPredeterminado; }
            set { SetPropertyValue("IdiomaPredeterminado", ref fIdiomaPredeterminado, value); }
        }

        public PaisIdioma CulturaPredeterminada
        {
            get { return fCulturaPredeterminada; }
            set { SetPropertyValue("CulturaPredeterminada", ref fCulturaPredeterminada, value); }
        }

        public string ColorFondo
        {
            get { return fColorFondo; }
            set { SetPropertyValue("ColorFondo", ref fColorFondo, value); }
        }

        [Association(@"seguridad.UsuarioEmpresa", typeof (Usuario), UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Usuario> Usuarios => GetCollection<Usuario>("Usuarios");

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            TipoLicencia = TipoLicencia.Trial;
        }

        protected override void OnSaved()
        {
            base.OnSaved();
        }
    }
}