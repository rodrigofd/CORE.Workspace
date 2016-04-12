using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Cotización")]
    [Persistent("seguros.Cotizacion")]
    public class Cotizacion : Documento
    {
        private Carpeta _carpeta;

        public Cotizacion(Session session)
            : base(session)
        {
        }

        [Association]
        public Carpeta Carpeta
        {
            get { return _carpeta; }
            set { SetPropertyValue("Carpeta", ref _carpeta, value); }
        }
    }
}