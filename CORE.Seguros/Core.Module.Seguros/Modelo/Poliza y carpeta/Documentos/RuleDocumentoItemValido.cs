using DevExpress.Persistent.Validation;

namespace FDIT.Core.Seguros
{
    [CodeRule]
    public class RuleDocumentoItemValido : RuleBase<DocumentoItem>
    {
        public RuleDocumentoItemValido() : base("", "Save")
        {
        }

        public RuleDocumentoItemValido(IRuleBaseProperties properties) : base(properties)
        {
        }

        protected override bool IsValidInternal(DocumentoItem target, out string errorMessageTemplate)
        {
            if (target.Documento != null && target.Documento.Tipo != null &&
                target.Documento.Tipo.Clase == DocumentoClase.Poliza)
                return DocumentoIntervinienteBase.ValidateIntervinientes(target.Intervinientes, out errorMessageTemplate);
            //TODO: para los items de documentos sucesivos (endosos), por ahora se ignora la validacion de intervinientes (en particular la suma de partic. 100%). Repensar
            errorMessageTemplate = null;
            return true;
        }
    }
}