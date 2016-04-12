using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    /*public enum DocumentoEstado
  {
    Error = 0,
    Anulada = 1,
    Cancelada = 2,
    Vencida = 3,
    Vigente = 4,
    Aceptada = 5,
    Rechazada = 6,
    PendienteDeValidar = 7,
    PendDeEnvio = 8,
    PendienteAcuseDeRecibo = 9,
    PendienteDeConfirmacion = 10,
    PendienteDeEmision = 11,
    PendienteDeRegularizar = 12,
    PendienteDeDespacho = 13,
    PendienteDeAceptacion = 14,
  }*/

    [Persistent("seguros.DocumentoEstado")]
    [DefaultProperty("Nombre")]
    [DefaultClassOptions]
    public class DocumentoEstado : BasicObject
    {
        private bool fAccionConfirma;
        private bool fAccionGeneraComprobante;
        private bool fConfirmada;
        private bool fInicial;
        private string fNombre;

        public DocumentoEstado(Session session) : base(session)
        {
        }

        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue("Nombre", ref fNombre, value); }
        }

        /// <summary>
        ///     Indica si este es el estado inicial de nuevos documentos
        /// </summary>
        public bool Inicial
        {
            get { return fInicial; }
            set { SetPropertyValue("Inicial", ref fInicial, value); }
        }

        /// <summary>
        ///     Indica si al 'entrar' a este estado, implica realizar la logica de generación de comprobante de venta (entra en
        ///     circuito de gestión administrativa)
        ///     La generación del comprobante puede ser anterior a la confirmación de la póliza (v. FFernandez Rep. Dominicana)
        /// </summary>
        public bool AccionGeneraComprobante
        {
            get { return fAccionGeneraComprobante; }
            set { SetPropertyValue("GeneraComprobante", ref fAccionGeneraComprobante, value); }
        }

        /// <summary>
        ///     Indica si al 'entrar' a este estado, implica realizar la logica de confirmación del documento
        /// </summary>
        public bool AccionConfirma
        {
            get { return fAccionConfirma; }
            set { SetPropertyValue("Confirmada", ref fAccionConfirma, value); }
        }

        /// <summary>
        ///     Indica si los documentos en este estado, se consideran confirmados (vigentes/válidos)
        /// </summary>
        public bool Confirmada
        {
            get { return fConfirmada; }
            set { SetPropertyValue("Confirmada", ref fConfirmada, value); }
        }
    }
}