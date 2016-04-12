using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Importe de documento")]
    [Persistent("seguros.DocumentoItemImporte")]
    public class DocumentoItemImporte : DocumentoImporteBase
    {
        private DocumentoItem fDocumentoItem;

        public DocumentoItemImporte(Session session) : base(session)
        {
        }

        [Association]
        public DocumentoItem DocumentoItem
        {
            get { return fDocumentoItem; }
            set { SetPropertyValue("DocumentoItem", ref fDocumentoItem, value); }
        }

        protected override IObjetoConImportes Padre
        {
            get { return DocumentoItem; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (DocumentoItem != null && DocumentoItem.Documento != null)
                DocumentoItem.Documento.ActualizarImportesSegunItems();
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();

            if (DocumentoItem != null && DocumentoItem.Documento != null)
                DocumentoItem.Documento.ActualizarImportesSegunItems();
        }
    }
}