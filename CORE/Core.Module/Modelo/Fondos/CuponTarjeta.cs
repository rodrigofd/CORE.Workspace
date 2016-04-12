using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.CuponTarjeta" ) ]
  [ System.ComponentModel.DisplayName( "Cupón Tarjeta" ) ]
  [ ImageName( "credit-card" ) ]
  public class CuponTarjeta : Valor
  {
    private int fCantidadCuotas;
    private string fCupon;
    private string fLote;
    private Tarjeta fTarjeta;

    public CuponTarjeta( Session Session )
      : base( Session )
    {
    }

    public int CantidadCuotas
    {
      get { return fCantidadCuotas; }
      set { SetPropertyValue< int >( "CantidadCuotas", ref fCantidadCuotas, value ); }
    }

    public string Cupon
    {
      get { return fCupon; }
      set { SetPropertyValue( "Cupon", ref fCupon, value ); }
    }

    public string Lote
    {
      get { return fLote; }
      set { SetPropertyValue( "Lote", ref fLote, value ); }
    }

    [ Aggregated ]
    [ ExpandObjectMembers( ExpandObjectMembers.Always ) ]
    public Tarjeta Tarjeta
    {
      get { return fTarjeta; }
      set { SetPropertyValue( "Tarjeta", ref fTarjeta, value ); }
    }
  }
}
