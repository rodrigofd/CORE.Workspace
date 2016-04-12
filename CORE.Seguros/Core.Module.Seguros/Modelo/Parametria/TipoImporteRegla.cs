using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [Persistent("seguros.TipoImporteRegla")]
    [DefaultClassOptions]
    public class TipoImporteRegla : BasicObject
    {
        private bool fPredeterminado;
        private Ramo fRamo;
        private Subramo fSubramo;
        private TipoImporte fTipoImporte;

        public TipoImporteRegla(Session session) : base(session)
        {
        }

        [Association]
        public TipoImporte TipoImporte
        {
            get { return fTipoImporte; }
            set { SetPropertyValue("TipoImporte", ref fTipoImporte, value); }
        }

        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Ramo Ramo
        {
            get { return fRamo; }
            set { SetPropertyValue("Ramo", ref fRamo, value); }
        }

        [DataSourceProperty("Ramo.Subramos")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Subramo Subramo
        {
            get { return fSubramo; }
            set { SetPropertyValue("Subramo", ref fSubramo, value); }
        }

        public bool Predeterminado
        {
            get { return fPredeterminado; }
            set { SetPropertyValue("Predeterminado", ref fPredeterminado, value); }
        }
    }
}