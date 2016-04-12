using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.ConceptoVinculo" )]
  [System.ComponentModel.DisplayName( "Vínculos de conceptos" )]
  public class ConceptoVinculo : BasicObject
  {
    private Concepto fHijo;
    private Concepto fPadre;

    public ConceptoVinculo( Session session ) : base( session )
    {
    }

    [Association( @"ConceptosVinculosReferencesConceptos" )]
    public Concepto Padre
    {
      get { return fPadre; }
      set { SetPropertyValue( "Padre", ref fPadre, value ); }
    }

    public Concepto Hijo
    {
      get { return fHijo; }
      set { SetPropertyValue( "Hijo", ref fHijo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}