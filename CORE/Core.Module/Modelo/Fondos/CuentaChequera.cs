using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [Persistent( @"fondos.CuentaChequera" )]
  [System.ComponentModel.DisplayName( "Chequeras" )]
  public class CuentaChequera : BasicObject
  {
    private Cuenta fCuenta;
    private int fNumeroDesde;
    private int fNumeroHasta;
    private int fUltimoNumero;
    private string fSerie;

    public CuentaChequera( Session session ) : base( session )
    {
    }

    [Association( @"CuentasChequerasReferencesCuentas" )]
    public Cuenta Cuenta
    {
      get { return fCuenta; }
      set { SetPropertyValue( "Cuenta", ref fCuenta, value ); }
    }

    public int UltimoNumero
    {
      get { return fUltimoNumero; }
      set { SetPropertyValue<int>( "UltimoNumero", ref fUltimoNumero, value ); }
    }

    [Size( 3 )]
    public string Serie
    {
      get { return fSerie; }
      set { SetPropertyValue( "Serie", ref fSerie, value ); }
    }

    public int NumeroDesde
    {
      get { return fNumeroDesde; }
      set { SetPropertyValue<int>( "NumeroDesde", ref fNumeroDesde, value ); }
    }

    public int NumeroHasta
    {
      get { return fNumeroHasta; }
      set { SetPropertyValue<int>( "NumeroHasta", ref fNumeroHasta, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}