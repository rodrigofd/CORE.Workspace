using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Impuestos;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.ComprobanteTipoCategoria" )]
  [System.ComponentModel.DisplayName( "Categorías por tipo de comprobante" )]
  public class ComprobanteTipoCategoria : BasicObject
  {
    private Categoria fCategoria;
    private ComprobanteTipo fComprobanteTipo;

    public ComprobanteTipoCategoria( Session session ) : base( session )
    {
    }

    [Association( @"ComprobantesTiposCategoriasReferencesComprobantesTipos" )]
    public ComprobanteTipo ComprobanteTipo
    {
      get { return fComprobanteTipo; }
      set { SetPropertyValue( "ComprobanteTipo", ref fComprobanteTipo, value ); }
    }

    public Categoria Categoria
    {
      get { return fCategoria; }
      set { SetPropertyValue( "IdCategoria", ref fCategoria, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}