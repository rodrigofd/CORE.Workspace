using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Clase Propiedad" ) ]
  [Persistent( @"stock.ClasePropiedad" )]
  public class ClasePropiedad : BasicObject
  {
    private Clase fClase;
    private Propiedad fPropiedad;
    private int fLongitudEnCodigo;
    private int fPosicionEnCodigo;
    private bool fValorEnCodigo;

    public ClasePropiedad( Session session ) : base( session )
    {
    }

    [ Association( @"Clases_por_PropiedadesReferencesClases" ) ]
    public Clase Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    [ Association( @"Clases_por_PropiedadesReferencesPropiedades" ) ]
    public Propiedad Propiedad
    {
      get { return fPropiedad; }
      set { SetPropertyValue( "Propiedad", ref fPropiedad, value ); }
    }

    public bool ValorEnCodigo
    {
      get { return fValorEnCodigo; }
      set { SetPropertyValue( "ValorEnCodigo", ref fValorEnCodigo, value ); }
    }

    public int PosicionEnCodigo
    {
      get { return fPosicionEnCodigo; }
      set { SetPropertyValue< int >( "PosicionEnCodigo", ref fPosicionEnCodigo, value ); }
    }

    public int LongitudEnCodigo
    {
      get { return fLongitudEnCodigo; }
      set { SetPropertyValue< int >( "LongitudEnCodigo", ref fLongitudEnCodigo, value ); }
    }
  }
}
