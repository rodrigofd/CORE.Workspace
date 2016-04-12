using System;

namespace FDIT.Core.StoredProcedures
{
  public class ConsultaStoredProcedureAttribute : Attribute
  {
    public Type ConsultaStoredProcedure;

    public ConsultaStoredProcedureAttribute( Type consultaStoredProcedure )
    {
      ConsultaStoredProcedure = consultaStoredProcedure;
    }
  }
}