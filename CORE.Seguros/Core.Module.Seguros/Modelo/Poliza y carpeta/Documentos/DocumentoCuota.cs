using System;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Cuota de documento")]
    [Persistent("seguros.DocumentoCuota")]
    public class DocumentoCuota : BasicObject
    {
        private ComprobanteCuota fComprobanteCuota;
        private Documento fDocumento;
        private DocumentoCuotaEstado fEstado;
        private DateTime fFecha;
        private decimal fImporte;
        private int fNumero;

        public DocumentoCuota(Session session)
            : base(session)
        {
        }

        [Association]
        public Documento Documento
        {
            get { return fDocumento; }
            set { SetPropertyValue("Documento", ref fDocumento, value); }
        }

        [RuleRequiredField]
        public int Numero
        {
            get { return fNumero; }
            set { SetPropertyValue<int>("Numero", ref fNumero, value); }
        }

        [RuleRequiredField]
        public DateTime Fecha
        {
            get { return fFecha; }
            set { SetPropertyValue<DateTime>("Fecha", ref fFecha, value); }
        }

        [RuleRequiredField]
        public decimal Importe
        {
            get { return fImporte; }
            set { SetPropertyValue<decimal>("Importe", ref fImporte, value); }
        }

        [RuleRequiredField]
        public DocumentoCuotaEstado Estado
        {
            get { return fEstado; }
            set { SetPropertyValue("Estado", ref fEstado, value); }
        }

        public ComprobanteCuota ComprobanteCuota
        {
            get { return fComprobanteCuota; }
            set { SetPropertyValue("ComprobanteCuota", ref fComprobanteCuota, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (CanRaiseOnChanged)
            {
                //Si se setea la referencia de documento (padre) a null, marcar modificado el valor anterior (perdió una cuota)
                if (oldValue != null && oldValue.GetType() == typeof (Documento))
                    GetParentObject((Documento) oldValue).OnCuotasModificadas();

                //Cualquier modificación en la cuota, marca el documento padre como modificado
                if (Documento != null)
                    GetParentObject(Documento).OnCuotasModificadas();
            }
        }
    }
}