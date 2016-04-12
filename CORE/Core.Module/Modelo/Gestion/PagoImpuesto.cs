using DevExpress.Xpo;
using FDIT.Core.Impuestos;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.PagoImpuesto" )]
  [System.ComponentModel.DisplayName( "Impuestos del pago" )]
  public class PagoImpuesto : BasicObject
  {
    private PagoAplicacion fAplicacion;
    private decimal fAlicuota;
    private decimal fBaseImponible;
    private decimal fImporte;
    private decimal fImporteFijo;
    private Impuesto fImpuesto;
    private Pago fPago;
    private Regimen fRegimen;

    public PagoImpuesto( Session session ) : base( session )
    {
    }

    [Association]
    public Pago Pago
    {
      get { return fPago; }
      set { SetPropertyValue( "Pago", ref fPago, value ); }
    }

    public Impuesto Impuesto
    {
      get { return fImpuesto; }
      set { SetPropertyValue( "Impuesto", ref fImpuesto, value ); }
    }

    public PagoAplicacion Aplicacion
    {
      get { return fAplicacion; }
      set { SetPropertyValue( "Aplicacion", ref fAplicacion, value ); }
    }

    public Regimen Regimen
    {
      get { return fRegimen; }
      set { SetPropertyValue( "Regimen", ref fRegimen, value ); }
    }

    public decimal BaseImponible
    {
      get { return fBaseImponible; }
      set { SetPropertyValue< decimal >( "BaseImponible", ref fBaseImponible, value ); }
    }

    public decimal Alicuota
    {
      get { return fAlicuota; }
      set { SetPropertyValue< decimal >( "Alicuota", ref fAlicuota, value ); }
    }

    public decimal ImporteFijo
    {
      get { return fImporteFijo; }
      set { SetPropertyValue< decimal >( "ImporteFijo", ref fImporteFijo, value ); }
    }

    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}