using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Contabilidad;
using FDIT.Core.Gestion;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Artículo" ) ]
  [ Persistent( @"stock.Articulo" ) ]
  public class Articulo : BasicObject
  {
    private string fAlias;
    private string fCodigo;
    private string fCodigoBarras;
    private string fCodigoLegal;
    private bool fCompra;
    private UnidadMedida fComprasUnidadMedida;
    private decimal fComprasUnidadMedidaEquivalencia;
    private Concepto fConcepto;
    private string fDescripcion;
    private string fDescripcionImpresion;
    private DateTime fFechaDeAlta;
    private Categoria fCategoria;
    private CentroDeCosto fCentroCostoCompras;
    private CentroDeCosto fCentroCostoVentas;
    private Cuenta fCuentaContableCompras;
    private Cuenta fCuentaContableVentas;

    private Clase fClase;
    private int fSiapClasificacionCompra;
    private int fSiapClasificacionVenta;
    private UnidadMedida fUnidadMedidaStock;
    private bool fActivo;
    private bool fLlevaPartida;
    private bool fLlevaSerie;
    private bool fLlevaStock;
    private MarcaComercial fMarcaComercial;
    private decimal fCantidadMaxima;
    private decimal fCantidadMinima;
    private string fNombre;
    private string fNotas;
    private decimal fPuntoPedido;
    private bool fStockNegativo;
    private Temporada fTemporada;
    private bool fVenta;
    private UnidadMedida fVentasUnidadMedida;
    private decimal fVentasUnidadMedidaEquivalecia;
    private bool fVentasFacturaPorImporte;
    private int fVentasIdIibbActividad;
    private bool fVentasStockNegativo;

    public Articulo( Session session ) : base( session )
    {
    }

    [ Association( @"ArticulosReferencesClases" ) ]
    public Clase Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    [ Association( @"ArticulosReferencesCategorias" ) ]
    public Categoria Categoria
    {
      get { return fCategoria; }
      set { SetPropertyValue( "Categoria", ref fCategoria, value ); }
    }

    [ Size( 50 ) ]
    [ System.ComponentModel.DisplayName( "Código" ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 50 ) ]
    public string CodigoLegal
    {
      get { return fCodigoLegal; }
      set { SetPropertyValue( "CodigoLegal", ref fCodigoLegal, value ); }
    }

    [ System.ComponentModel.DisplayName( "Nombre" ) ]
    [ Index( 1 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ System.ComponentModel.DisplayName( "Concepto asociado" ) ]
    public Concepto Concepto
    {
      get { return fConcepto; }
      set { SetPropertyValue( "Concepto", ref fConcepto, value ); }
    }

    [ System.ComponentModel.DisplayName( "Temporada" ) ]
    public Temporada Temporada
    {
      get { return fTemporada; }
      set { SetPropertyValue( "Temporada", ref fTemporada, value ); }
    }

    [ System.ComponentModel.DisplayName( "Marca comercial" ) ]
    public MarcaComercial MarcaComercial
    {
      get { return fMarcaComercial; }
      set { SetPropertyValue( "MarcaComercial", ref fMarcaComercial, value ); }
    }

    [ Size( 50 ) ]
    public string Alias
    {
      get { return fAlias; }
      set { SetPropertyValue( "Alias", ref fAlias, value ); }
    }

    [ Size( 50 ) ]
    public string CodigoBarras
    {
      get { return fCodigoBarras; }
      set { SetPropertyValue( "CodigoBarras", ref fCodigoBarras, value ); }
    }

    public DateTime FechaDeAlta
    {
      get { return fFechaDeAlta; }
      set { SetPropertyValue< DateTime >( "FechaDeAlta", ref fFechaDeAlta, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Descripcion
    {
      get { return fDescripcion; }
      set { SetPropertyValue( "Descripcion", ref fDescripcion, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string DescripcionImpresion
    {
      get { return fDescripcionImpresion; }
      set { SetPropertyValue( "DescripcionImpresion", ref fDescripcionImpresion, value ); }
    }

    public bool LlevaStock
    {
      get { return fLlevaStock; }
      set { SetPropertyValue( "LlevaStock", ref fLlevaStock, value ); }
    }

    public bool StockNegativo
    {
      get { return fStockNegativo; }
      set { SetPropertyValue( "StockNegativo", ref fStockNegativo, value ); }
    }

    public bool LlevaPartida
    {
      get { return fLlevaPartida; }
      set { SetPropertyValue( "LlevaPartida", ref fLlevaPartida, value ); }
    }

    public bool LlevaSerie
    {
      get { return fLlevaSerie; }
      set { SetPropertyValue( "LlevaSerie", ref fLlevaSerie, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    public bool Compra
    {
      get { return fCompra; }
      set { SetPropertyValue( "Compra", ref fCompra, value ); }
    }

    public bool Venta
    {
      get { return fVenta; }
      set { SetPropertyValue( "Venta", ref fVenta, value ); }
    }

    public decimal CantidadMaxima
    {
      get { return fCantidadMaxima; }
      set { SetPropertyValue<decimal>( "CantidadMaxima", ref fCantidadMaxima, value ); }
    }

    public decimal CantidadMinima
    {
      get { return fCantidadMinima; }
      set { SetPropertyValue<decimal>( "CantidadMinima", ref fCantidadMinima, value ); }
    }

    public decimal PuntoPedido
    {
      get { return fPuntoPedido; }
      set { SetPropertyValue< decimal >( "PuntoPedido", ref fPuntoPedido, value ); }
    }

    public UnidadMedida UnidadMedidaStock
    {
      get { return fUnidadMedidaStock; }
      set { SetPropertyValue( "UnidadMedidaStock", ref fUnidadMedidaStock, value ); }
    }

    public UnidadMedida ComprasUnidadMedida
    {
      get { return fComprasUnidadMedida; }
      set { SetPropertyValue( "ComprasUnidadMedida", ref fComprasUnidadMedida, value ); }
    }

    public decimal ComprasUnidadMedidaEquivalencia
    {
      get { return fComprasUnidadMedidaEquivalencia; }
      set { SetPropertyValue<decimal>( "ComprasUnidadMedidaEquivalencia", ref fComprasUnidadMedidaEquivalencia, value ); }
    }

    public UnidadMedida VentasUnidadMedida
    {
      get { return fVentasUnidadMedida; }
      set { SetPropertyValue( "VentasUnidadMedida", ref fVentasUnidadMedida, value ); }
    }

    public decimal VentasUnidadMedidaEquivalecia
    {
      get { return fVentasUnidadMedidaEquivalecia; }
      set { SetPropertyValue<decimal>( "VentasUnidadMedidaEquivalecia", ref fVentasUnidadMedidaEquivalecia, value ); }
    }

    public int VentasIdIibbActividad
    {
      get { return fVentasIdIibbActividad; }
      set { SetPropertyValue< int >( "VentasIdIibbActividad", ref fVentasIdIibbActividad, value ); }
    }

    public bool VentasStockNegativo
    {
      get { return fVentasStockNegativo; }
      set { SetPropertyValue( "VentasStockNegativo", ref fVentasStockNegativo, value ); }
    }

    public bool VentasFacturaPorImporte
    {
      get { return fVentasFacturaPorImporte; }
      set { SetPropertyValue( "VentasFacturaPorImporte", ref fVentasFacturaPorImporte, value ); }
    }

    public Cuenta CuentaContableCompras
    {
      get { return fCuentaContableCompras; }
      set { SetPropertyValue("CuentaContableCompras", ref fCuentaContableCompras, value ); }
    }

    public CentroDeCosto CentroCostoCompras
    {
      get { return fCentroCostoCompras; }
      set { SetPropertyValue( "CentroCostoCompras", ref fCentroCostoCompras, value ); }
    }

    public Cuenta CuentaContableVentas
    {
      get { return fCuentaContableVentas; }
      set { SetPropertyValue( "CuentaContableVentas", ref fCuentaContableVentas, value ); }
    }

    public CentroDeCosto CentroCostoVentas
    {
      get { return fCentroCostoVentas; }
      set { SetPropertyValue( "CentroCostoVentas", ref fCentroCostoVentas, value ); }
    }

    public int SiapClasificacionCompra
    {
      get { return fSiapClasificacionCompra; }
      set { SetPropertyValue<int>( "SiapClasificacionCompra", ref fSiapClasificacionCompra, value ); }
    }

    public int SiapClasificacionVenta
    {
      get { return fSiapClasificacionVenta; }
      set { SetPropertyValue<int>( "SiapClasificacionVenta", ref fSiapClasificacionVenta, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [Association]
    [Aggregated]
    [System.ComponentModel.DisplayName( "Precios por lista" )]
    public XPCollection<Precio> Precios
    {
      get { return GetCollection<Precio>( @"Precios" ); }
    }

    //[Association]
    //[Aggregated]
    //[System.ComponentModel.DisplayName( "Existencias" )]
    //public XPCollection<Existencia> Existencias
    //{
    //  get { return GetCollection<Existencia>( @"Existencias" ); }
    //}

    [Aggregated]
    [ Association( @"Articulos_por_ProveedoresReferencesArticulos", typeof( ArticuloPorProveedor ) ) ]
    public XPCollection< ArticuloPorProveedor > ArticuloPorProveedores
    {
      get { return GetCollection<ArticuloPorProveedor>( "ArticuloPorProveedores" ); }
    }

    [Aggregated]
    [ Association( @"Articulos_PropiedadesReferencesArticulos", typeof( ArticuloPropiedad ) ) ]
    public XPCollection< ArticuloPropiedad > Propiedades
    {
      get { return GetCollection<ArticuloPropiedad>( "Propiedades" ); }
    }

    [ Association( @"Articulos_SeriesReferencesArticulos", typeof( ArticuloSerie ) ) ]
    public XPCollection< ArticuloSerie > Series
    {
      get { return GetCollection<ArticuloSerie>( "Series" ); }
    }

    //TODO: compaginar ambos lados de la equivalencia: o sea que esta coleccion devuelva mis articulos equivalentes, y tambien aquellos de los cuales YO soy equivalente
    [ Association( @"EquivalenciasReferencesArticulos", typeof( Equivalencia ) ) ]
    public XPCollection< Equivalencia > Equivalencias
    {
      get { return GetCollection< Equivalencia >( "Equivalencias" ); }
    }

    //TODO: compaginar ambos lados de la equivalencia: o sea que esta coleccion devuelva mis articulos equivalentes, y tambien aquellos de los cuales YO soy equivalente
    [ Association( @"InsumosReferencesArticulos", typeof( Insumo ) ) ]
    public XPCollection< Insumo > Insumos
    {
      get { return GetCollection< Insumo >( "Insumos" ); }
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();

        Activo = true;
        LlevaStock = true;
        Venta = true;
        Compra = true;

    }
  }
}
