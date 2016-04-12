using DevExpress.Xpo;
using FDIT.Core.Util;

namespace FDIT.Core.Regionales
{
  [Persistent( @"regionales.Identificadores" )]
  [System.ComponentModel.DisplayName( "Preferencias regionales" )]
  public class Identificadores : IdentificadoresBase<Identificadores>
  {
    private Idioma fIdiomaPredeterminado;
    private Pais fPaisPredeterminado;

    public Identificadores( Session session )
      : base( session )
    { }

    public Pais PaisPredeterminado
    {
      get { return fPaisPredeterminado; }
      set { SetPropertyValue( "PaisPredeterminado", ref fPaisPredeterminado, value ); }
    }

    public Idioma IdiomaPredeterminado
    {
      get { return fIdiomaPredeterminado; }
      set { SetPropertyValue("IdiomaPredeterminado", ref fIdiomaPredeterminado, value); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
