using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Importe de documento")]
    [Persistent("seguros.DocumentoImporte")]
    public class DocumentoImporte : DocumentoImporteBase
    {
        private Documento fDocumento;

        public DocumentoImporte(Session session) : base(session)
        {
        }

        [Association]
        public Documento Documento
        {
            get { return fDocumento; }
            set { SetPropertyValue("Documento", ref fDocumento, value); }
        }

        protected override IObjetoConImportes Padre
        {
            get { return Documento; }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (CanRaiseOnChanged)
            {
                //Si se setea la referencia de documento (padre) a null, marcar modificado el valor anterior (perdió un importe)
                if (oldValue != null && oldValue.GetType() == typeof (Documento))
                    GetParentObject((Documento) oldValue).OnImporteModificado();

                //Cualquier modificación en el importe, marca el documento padre como modificado
                if (Documento != null)
                    GetParentObject(Documento).OnImporteModificado();
            }
        }
    }
}