using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Fondos
{
  [ Persistent( "fondos.Tarjeta" ) ]
  [ DefaultProperty( "" ) ]
  [ System.ComponentModel.DisplayName( "Tarjeta" ) ]
  [ DefaultClassOptions ]
  public class Tarjeta : BasicObject
  {
    private string fCodigoSeguridad;
    private DateTime fFechaDesde;
    private DateTime fFechaHasta;
    private string fNumero;
    private string fTitularIdentificacion;
    private IdentificacionTipo fTitularIdentificacionTipo;
    private string fTitularNombre;

    public Tarjeta( Session session ) : base( session )
    {
    }

    public string Numero
    {
      get { return fNumero; }
      set { SetPropertyValue( "Numero", ref fNumero, value ); }
    }

    public DateTime FechaDesde
    {
      get { return fFechaDesde; }
      set { SetPropertyValue( "FechaDesde", ref fFechaDesde, value ); }
    }

    public DateTime FechaHasta
    {
      get { return fFechaHasta; }
      set { SetPropertyValue( "FechaHasta", ref fFechaHasta, value ); }
    }

    public string CodigoSeguridad
    {
      get { return fCodigoSeguridad; }
      set { SetPropertyValue( "CodigoSeguridad", ref fCodigoSeguridad, value ); }
    }

    public IdentificacionTipo TitularIdentificacionTipo
    {
      get { return fTitularIdentificacionTipo; }
      set { SetPropertyValue( "TitularIdentificacionTipo", ref fTitularIdentificacionTipo, value ); }
    }

    public string TitularIdentificacion
    {
      get { return fTitularIdentificacion; }
      set { SetPropertyValue( "TitularIdentificacion", ref fTitularIdentificacion, value ); }
    }

    public string TitularNombre
    {
      get { return fTitularNombre; }
      set { SetPropertyValue( "TitularNombre", ref fTitularNombre, value ); }
    }
  }
}