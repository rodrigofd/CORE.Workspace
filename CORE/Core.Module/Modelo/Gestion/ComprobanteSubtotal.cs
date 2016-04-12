using System.ComponentModel;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  [NonPersistent]
  [System.ComponentModel.DisplayName( "Subtotales de factura" )]
  public class ComprobanteSubtotal : BasicObject
  {
    private decimal fAlicuota;
    private decimal fBaseImponible;
    private decimal fTotal;

    public ComprobanteSubtotal( Session session )
      : base( session )
    {
    }
    
    public decimal Alicuota
    {
      get { return fAlicuota; }
      set { SetPropertyValue( "Alicuota", ref fAlicuota, value ); }
    }

    public decimal BaseImponible
    {
      get { return fBaseImponible; }
      set { SetPropertyValue( "BaseImponible", ref fBaseImponible, value ); }
    }

    public decimal Total
    {
      get { return fTotal; }
      set { SetPropertyValue( "Total", ref fTotal, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}