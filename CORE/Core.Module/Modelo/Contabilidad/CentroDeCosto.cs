using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Contabilidad
{
  [ Persistent( @"contabilidad.CentroDeCosto" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Centros de costo" ) ]
  public class CentroDeCosto : BasicObject
  {
    private bool fActivo;
    private string fCentroDeCosto;
    private string fCodigo;

    public CentroDeCosto( Session session ) : base( session )
    {
    }

    [ Size( 5 ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Index( 1 ) ]
    public string Nombre
    {
      get { return fCentroDeCosto; }
      set { SetPropertyValue( "Nombre", ref fCentroDeCosto, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
