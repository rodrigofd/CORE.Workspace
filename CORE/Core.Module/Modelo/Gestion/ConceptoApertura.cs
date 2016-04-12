using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Gestion
{
  [ Persistent( @"gestion.ConceptoApertura" ) ]
  [ System.ComponentModel.DisplayName( "Apertura por conceptos" ) ]
  public class ConceptoApertura : BasicObject
  {
    private CentroDeCosto fCentroDeCosto;
    private Concepto fConcepto;
    private decimal fPorcentaje;

    public ConceptoApertura( Session session ) : base( session )
    {
    }

    [ Association ]
    public Concepto Concepto
    {
      get { return fConcepto; }
      set { SetPropertyValue( "Concepto", ref fConcepto, value ); }
    }

    public CentroDeCosto CentroDeCosto
    {
      get { return fCentroDeCosto; }
      set { SetPropertyValue( "CentroDeCosto", ref fCentroDeCosto, value ); }
    }

    public decimal Porcentaje
    {
      get { return fPorcentaje; }
      set { SetPropertyValue< decimal >( "Porcentaje", ref fPorcentaje, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
