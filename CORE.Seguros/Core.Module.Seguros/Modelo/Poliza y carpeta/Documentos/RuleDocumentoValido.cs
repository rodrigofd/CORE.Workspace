using DevExpress.Persistent.Validation;

namespace FDIT.Core.Seguros
{
    [CodeRule]
    public class RuleDocumentoValido : RuleBase<Documento>
    {
        public RuleDocumentoValido() : base("", "Save")
        {
        }

        public RuleDocumentoValido(IRuleBaseProperties properties) : base(properties)
        {
        }

        protected override bool IsValidInternal(Documento target, out string errorMessageTemplate)
        {
            if (target.Tipo != null && target.Tipo.Clase == DocumentoClase.Poliza)
                return DocumentoIntervinienteBase.ValidateIntervinientes(target.Intervinientes, out errorMessageTemplate);
            //TODO: para los documentos sucesivos (endosos), por ahora se ignora la validacion de intervinientes (en particular la suma de partic. 100%). Repensar
            errorMessageTemplate = null;
            return true;
        }
    }
}