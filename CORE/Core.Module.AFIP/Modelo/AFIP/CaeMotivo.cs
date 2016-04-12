using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.CaeMotivo" ) ]
  [ DefaultClassOptions ]
  [DefaultProperty("Nombre")]
  [ System.ComponentModel.DisplayName( "CaeMotivos" ) ]
  public class CaeMotivo : BasicObject
  {
    private string fCaeMotivo;
    private int fCodigo;

    public CaeMotivo( Session session ) : base( session )
    {
    }

    [ Size( 10 ) ]
    public int Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue< int >( "Codigo", ref fCodigo, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Nombre
    {
      get { return fCaeMotivo; }
      set { SetPropertyValue( "Nombre", ref fCaeMotivo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
