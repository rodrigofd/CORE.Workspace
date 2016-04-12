using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Impuestos;
using FDIT.Core.Personas;

namespace FDIT.Core.Gestion
{
  [ Persistent( @"gestion.ComprobanteTipo" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Tipos de comprobantes" ) ]
  public class ComprobanteTipo : BasicObject
  {
    private Categoria fCategoriaDeIvaPredet;
    private string fClase;
    private string fCodigo;
    private int fCopiasImpresion;
    private DebitoCredito fDebitoCredito;
    private string fDescripcionImpresion;
    private bool fDiscriminaImpuestos;
    private bool fGestionFinanciera;

    private IdentificacionTipo fIdentificacionTipoPredet;
    private bool fIncluyeIva;
    private bool fIncluyePercepciones;
    private bool fLibroIvaCompras;
    private bool fLibroIvaVentas;
    private ComprobanteTipoModulo fModulo;
    private string fNombre;
    private bool fActivo;
    private bool fAutomatico;
    private int fOrden;
    private ComprobanteModeloItem fComprobanteModeloItemPrincipal;
    private bool fStockSalida;
    private bool fStockEntrada;
    private bool fValorizado;

    public ComprobanteTipo( Session session ) : base( session )
    {
    }

    [ VisibleInListView( false ) ]
    [VisibleInDetailView( false )]
    [VisibleInLookupListView( false )]
    //[ PersistentAlias( "ISNULL('(' + Codigo + ') ' + Nombre, '****')" ) ] 
    [PersistentAlias("ISNULL(Codigo, Nombre)")] 
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [Index( 0 )]
    [VisibleInLookupListView( true )]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 200 ) ]
    [Index( 1 )]
    [VisibleInLookupListView( true )]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Size( 1 ) ]
    public string Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    public ComprobanteTipoModulo Modulo
    {
      get { return fModulo; }
      set { SetPropertyValue( "Modulo", ref fModulo, value ); }
    }

    public DebitoCredito DebitoCredito
    {
      get { return fDebitoCredito; }
      set { SetPropertyValue( "DebitoCredito", ref fDebitoCredito, value ); }
    }

    public bool DiscriminaImpuestos
    {
      get { return fDiscriminaImpuestos; }
      set { SetPropertyValue( "DiscriminaImpuestos", ref fDiscriminaImpuestos, value ); }
    }

    public bool Automatico
    {
      get { return fAutomatico; }
      set { SetPropertyValue( "Automatico", ref fAutomatico, value ); }
    }

    public int Orden
    {
      get { return fOrden; }
      set { SetPropertyValue<int>( "Orden", ref fOrden, value ); }
    }

    public bool LibroIvaCompras
    {
      get { return fLibroIvaCompras; }
      set { SetPropertyValue( "LibroIvaCompras", ref fLibroIvaCompras, value ); }
    }

    public bool LibroIvaVentas
    {
      get { return fLibroIvaVentas; }
      set { SetPropertyValue( "LibroIvaVentas", ref fLibroIvaVentas, value ); }
    }

    public int CopiasImpresion
    {
      get { return fCopiasImpresion; }
      set { SetPropertyValue< int >( "CopiasImpresion", ref fCopiasImpresion, value ); }
    }

    public Categoria CategoriaDeIvaPredet
    {
      get { return fCategoriaDeIvaPredet; }
      set { SetPropertyValue( "CategoriaDeIvaPredet", ref fCategoriaDeIvaPredet, value ); }
    }

    public IdentificacionTipo IdentificacionTipoPredet
    {
      get { return fIdentificacionTipoPredet; }
      set { SetPropertyValue( "IdentificacionTipoPredet", ref fIdentificacionTipoPredet, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    public bool IncluyeIva
    {
      get { return fIncluyeIva; }
      set { SetPropertyValue( "IncluyeIva", ref fIncluyeIva, value ); }
    }

    public bool IncluyePercepciones
    {
      get { return fIncluyePercepciones; }
      set { SetPropertyValue( "IncluyePercepciones", ref fIncluyePercepciones, value ); }
    }

    public bool GestionFinanciera
    {
      get { return fGestionFinanciera; }
      set { SetPropertyValue( "GestionFinanciera", ref fGestionFinanciera, value ); }
    }

    public bool StockEntrada
    {
      get { return fStockEntrada; }
      set { SetPropertyValue( "Entrada", ref fStockEntrada, value ); }
    }

    public bool StockSalida
    {
      get { return fStockSalida; }
      set { SetPropertyValue( "Salida", ref fStockSalida, value ); }
    }

    public bool Valorizado
    {
      get { return fValorizado; }
      set { SetPropertyValue( "Valorizado", ref fValorizado, value ); }
    }

    public string DescripcionImpresion
    {
      get { return fDescripcionImpresion; }
      set { SetPropertyValue( "DescripcionImpresion", ref fDescripcionImpresion, value ); }
    }

    [ Aggregated ]
    [ Association( @"ComprobantesTiposCategoriasReferencesComprobantesTipos", typeof( ComprobanteTipoCategoria ) ) ]
    public XPCollection< ComprobanteTipoCategoria > CategoriasValidas
    {
      get { return GetCollection< ComprobanteTipoCategoria >( "CategoriasValidas" ); }
    }

    [ Aggregated ]
    [ Association( @"ComprobantesTiposCategoriasReferencesIdentificacionesTipos" ) ]
    public XPCollection< ComprobantesTipoIdentificacionTipo > IdentificacionesTiposValidos
    {
      get { return GetCollection< ComprobantesTipoIdentificacionTipo >( "IdentificacionesTiposValidos" ); }
    }

    [ Association ]
    public XPCollection< Talonario > Talonarios
    {
      get { return GetCollection< Talonario >( "Talonarios" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Modulo = ComprobanteTipoModulo.Gestion;
      DebitoCredito = DebitoCredito.Debito;
    }
  }
}
