using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Impuestos
{
  [Persistent( @"impuestos.RetencionEscala" )]
  [DefaultClassOptions]
  [System.ComponentModel.DisplayName( "Escala de retenciones" )]
  public class RetencionEscala : BasicObject
  {
    private Impuesto fImpuesto;
    private decimal fImporte;
    private decimal fImporteMaximo;
    private decimal fImporteMinimo;
    private decimal fMontoExcedente;
    private decimal fPorcentajeExcedente;

    public RetencionEscala( Session session ) : base( session )
    {
    }

    [Association( @"RetencionesEscalaReferencesImpuestos" )]
    public Impuesto Impuesto
    {
      get { return fImpuesto; }
      set { SetPropertyValue( "Impuesto", ref fImpuesto, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal ImporteMinimo
    {
      get { return fImporteMinimo; }
      set { SetPropertyValue< decimal >( "ImporteMinimo", ref fImporteMinimo, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal ImporteMaximo
    {
      get { return fImporteMaximo; }
      set { SetPropertyValue< decimal >( "ImporteMaximo", ref fImporteMaximo, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal PorcentajeExcedente
    {
      get { return fPorcentajeExcedente; }
      set { SetPropertyValue< decimal >( "PorcentajeExcedente", ref fPorcentajeExcedente, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal MontoExcedente
    {
      get { return fMontoExcedente; }
      set { SetPropertyValue< decimal >( "MontoExcedente", ref fMontoExcedente, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}