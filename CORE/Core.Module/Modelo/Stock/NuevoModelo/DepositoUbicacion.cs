using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Deposito Ubicación" ) ]
  [ Persistent( @"stock.DepositoUbicacion" ) ]
  public class DepositoUbicacion : BasicObject
  {
    private string fCodigo;
    private Deposito fDeposito;
    private string fNombre;

    public DepositoUbicacion( Session session ) : base( session )
    {
    }

    [ Association( @"Depositos_UbicacionesReferencesDepositos" ) ]
    public Deposito Deposito
    {
      get { return fDeposito; }
      set { SetPropertyValue( "Deposito", ref fDeposito, value ); }
    }

    [ Size( 10 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 50 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    //TODO: coleccion con movimientos
  }
}
