using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Regionales
{
  [Persistent( @"regionales.PaisIdioma" )]
  [DefaultClassOptions]
  [DefaultProperty("Codigo")]
  [System.ComponentModel.DisplayName( "Idioma del país" )]
  public class PaisIdioma : BasicObject
  {
    private Idioma fIdioma;
    private int fOrden;
    private Pais fPais;

    public PaisIdioma( Session session ) : base( session )
    {
    }

    [PersistentAlias( "IIF(ISNULL(Idioma),'',Idioma.Codigo1) + '-' + IIF(ISNULL(Pais),'',Pais.Codigo)" )]
    public string Codigo
    {
      get { return ( string ) EvaluateAlias( "Codigo" ); }
    }

    [Association( @"PaisesIdiomasReferencesPaises" )]
    public Pais Pais
    {
      get { return fPais; }
      set { SetPropertyValue( "Pais", ref fPais, value ); }
    }

    public Idioma Idioma
    {
      get { return fIdioma; }
      set { SetPropertyValue( "Idioma", ref fIdioma, value ); }
    }

    public int Orden
    {
      get { return fOrden; }
      set { SetPropertyValue< int >( "Orden", ref fOrden, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}