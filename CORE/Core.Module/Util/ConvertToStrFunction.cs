using System;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace FDIT.Core.Util
{
    public class ConvertToStrFunction : ICustomFunctionOperatorFormattable
    {
        string ICustomFunctionOperatorFormattable.Format(Type providerType, params string[] operands)
        {
            if (providerType == typeof (MSSqlConnectionProvider))
                return $"convert(nvarchar(max), {operands[0]}, {operands[1]})";
            throw new NotSupportedException(string.Concat("Proveedor de base de datos no soportado: ", providerType.Name));
        }

        // Evaluates the function on the client. 
        object ICustomFunctionOperator.Evaluate(params object[] operands)
        {
            return ConvertToStr(operands[0], Convert.ToString(operands[1]));
        }

        string ICustomFunctionOperator.Name => "ConvertToStr";

        Type ICustomFunctionOperator.ResultType(params Type[] operands)
        {
            return typeof (string);
        }

        public static string ConvertToStr(object obj, string fmt)
        {
            if (fmt == "102" || fmt == "103")
                fmt = "d";

            return ((IFormattable) obj)?.ToString(fmt, null) ?? "";
        }
    }
}