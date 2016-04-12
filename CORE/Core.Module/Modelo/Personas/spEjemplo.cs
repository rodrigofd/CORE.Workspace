using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using FDIT.Core.StoredProcedures;

namespace FDIT.Core.Personas
{
  [ NonPersistent ]
  public class spEjemploParams : IConsultaStoredProcedure
  {
    public DateTime? FechaDesde{ get; set; }
    public DateTime? FechaHasta{ get; set; }

    [ Browsable( false ) ]
    public OperandValue[ ] Parametros
    {
      get { return new[ ] { new OperandValue( FechaDesde ), new OperandValue( FechaHasta ) }; }
    }

    [ Browsable( false ) ]
    public string NombreStoredProcedure
    {
      get { return "personas.spEjemplo"; }
    }

    [ Browsable( false ) ]
    public Type ClaseResultados
    {
      get { return typeof( spEjemploResult ); }
    }
  }

  [ NonPersistent ]
  [ ConsultaStoredProcedure( typeof( spEjemploParams ) ) ]
  public class spEjemploResult
  {
    public int OID{ get; set; }
    public int Grupo{ get; set; }
    public int Tipo{ get; set; }
    public string Tratamiento{ get; set; }
    public string ApellidosPaterno{ get; set; }
    public string ApellidosMaterno{ get; set; }
    public string NombrePila{ get; set; }
    public string SegundoNombre{ get; set; }
    public string Nombre{ get; set; }
    public string NombreFantasia{ get; set; }
    public string NombreCompletoAlias{ get; set; }
    public DateTime NacimientoFecha{ get; set; }
    public DateTime AniversarioFecha{ get; set; }
    public int Edad{ get; set; }
    public int NacimientoPais{ get; set; }
    public int Sexo{ get; set; }
    public DateTime FallecimientoFecha{ get; set; }
    public int FallecimientoPais{ get; set; }
    public string Notas{ get; set; }
    public int DireccionPrimaria{ get; set; }
    public byte[ ] Imagen{ get; set; }
    public byte[ ] ImagenFirma{ get; set; }
    public byte[ ] ImagenImp{ get; set; }
    public string CodigoEWS{ get; set; }
    public DateTime ModifFechaEWS{ get; set; }
  }
}
