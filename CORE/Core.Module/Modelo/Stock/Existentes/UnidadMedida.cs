using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ Persistent( @"stock.UnidadMedida" ) ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Unidad de medida" ) ]
  public class UnidadMedida : BasicObject
  {
    private string fCodigo;
    private string fNombre;
    private int fUnidadesFraccionamiento;

    public UnidadMedida( Session session )
      : base( session )
    {
    }

    [ Size( 50 ) ]
    [ System.ComponentModel.DisplayName( "Código" ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ System.ComponentModel.DisplayName( "Nombre" ) ]
    [ Index( 1 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ RuleValueComparison( DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 1 ) ]
    public int UnidadesFraccionamiento
    {
      get { return fUnidadesFraccionamiento; }
      set { SetPropertyValue< int >( "UnidadesFraccionamiento", ref fUnidadesFraccionamiento, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      UnidadesFraccionamiento = 1;
    }
  }
}
