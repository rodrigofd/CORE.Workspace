using System;
using DevExpress.Data.Filtering;

namespace FDIT.Core.StoredProcedures
{
    public interface IConsultaStoredProcedure
    {
        OperandValue[] Parametros { get; }

        string NombreStoredProcedure { get; }
        Type ClaseResultados { get; }
    }
}