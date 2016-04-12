using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ DefaultProperty( "" ) ]
  [ System.ComponentModel.DisplayName( "Equivalencia" ) ]
  [ Persistent( @"stock.Equivalencia" ) ]
  public class Equivalencia : BasicObject
  {
    private Articulo fArticulo;
    private Articulo fArticuloEquivalente;
    private string fNotas;

    public Equivalencia( Session session ) : base( session )
    {
    }

    [ Association( @"EquivalenciasReferencesArticulos" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }
    
    public Articulo ArticuloEquivalente
    {
      get { return fArticuloEquivalente; }
      set { SetPropertyValue( "ArticuloEquivalente", ref fArticuloEquivalente, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }
  }
}
