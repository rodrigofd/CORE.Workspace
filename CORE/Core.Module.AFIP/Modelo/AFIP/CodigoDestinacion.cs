using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.CodigoDestinacion" ) ]
  [ DefaultClassOptions ]
  [DefaultProperty("Nombre")]
  [ System.ComponentModel.DisplayName( "CodigosDestinacion" ) ]
  public class CodigoDestinacion : BasicObject
  {
    private int fCodigo;
    private string fCodigoDestinacion;
    private DateTime fFechaDesde;
    private DateTime fFechaHasta;

    public CodigoDestinacion( Session session ) : base( session )
    {
    }

    [ Size( 10 ) ]
    public int Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue< int >( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 300 ) ]
    public string Nombre
    {
      get { return fCodigoDestinacion; }
      set { SetPropertyValue( "Nombre", ref fCodigoDestinacion, value ); }
    }

    public DateTime FechaDesde
    {
      get { return fFechaDesde; }
      set { SetPropertyValue< DateTime >( "FechaDesde", ref fFechaDesde, value ); }
    }

    public DateTime FechaHasta
    {
      get { return fFechaHasta; }
      set { SetPropertyValue< DateTime >( "FechaHasta", ref fFechaHasta, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
