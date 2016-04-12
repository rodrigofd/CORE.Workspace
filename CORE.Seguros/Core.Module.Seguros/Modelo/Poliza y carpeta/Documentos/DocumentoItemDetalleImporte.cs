using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Importe de documento")]
    [Persistent("seguros.DocumentoItemDetalleImporte")]
    public class DocumentoItemDetalleImporte : DocumentoImporteBase
    {
        private DocumentoItemDetalle fDocumentoItemDetalle;

        public DocumentoItemDetalleImporte(Session session) : base(session)
        {
        }

        [Association]
        public DocumentoItemDetalle DocumentoItemDetalle
        {
            get { return fDocumentoItemDetalle; }
            set { SetPropertyValue("DocumentoItemDetalle", ref fDocumentoItemDetalle, value); }
        }

        protected override IObjetoConImportes Padre
        {
            get { return DocumentoItemDetalle; }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (DocumentoItemDetalle != null && DocumentoItemDetalle.DocumentoItem != null)
                DocumentoItemDetalle.DocumentoItem.ActualizarImportesSegunDetalles();
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();

            if (DocumentoItemDetalle != null && DocumentoItemDetalle.DocumentoItem != null)
                DocumentoItemDetalle.DocumentoItem.ActualizarImportesSegunDetalles();
        }
    }
}