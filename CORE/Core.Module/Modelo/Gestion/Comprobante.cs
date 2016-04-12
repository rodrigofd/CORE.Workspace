using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using FDIT.Core.Fondos;
using FDIT.Core.Personas;
using FDIT.Core.Stock;
using FDIT.Core.Util;
using Categoria = FDIT.Core.Impuestos.Categoria;

namespace FDIT.Core.Gestion
{
  /// <summary>
  ///   Representa un comprobante de una operación comercial (de compra-venta)
  /// </summary>
  [ Persistent( "gestion.Comprobante" ) ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Comprobante" ) ]
  [ RuleCriteria( "FDIT.Core.Gestion.Comprobante.ItemsConCuotas",
    DefaultContexts.Save,
    "ImporteTotal = Cuotas.SUM(Importe)",
    CustomMessageTemplate = "La suma de cuotas no coincide con el importe total del comprobante" ) ]
  public class Comprobante : ComprobanteBase
  {
    protected string fAutorizadaCodigo;
    protected DateTime? fAutorizadaCodigoFecVto;
    protected DateTime? fAutorizadaFecha;
    protected string fAutorizadaNotas;
    protected string fAutorizadaResultado;
    protected decimal fCambio;
    protected int fCantidadCuotas;
    protected Categoria fCategoriaDeIva;
    protected bool fComprobanteRemito;
    private ConceptosIncluidos fConceptosIncluidos;
    protected CondicionDePago fCondicionDePago;
    protected Cuenta fCuenta;
    protected decimal fDescuento;
    protected string fDomicilio;
    protected DateTime fFechaContable;
    protected DateTime fFinPrestacion;
    protected string fIdentificacionNro;
    protected IdentificacionTipo fIdentificacionTipo;
    protected DateTime fInicioPrestacion;
    protected ListaDePrecios fLista;
    protected Especie fEspecie;
    protected string fNombre;
    protected string fNotasCobranza;

    public Comprobante( Session session )
      : base( session )
    {
    }

    [Size(200)]
    [ ModelDefault( "RowCount", "1" ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Size( 300 ) ]
    [ ModelDefault( "RowCount", "3" ) ]
    public string Domicilio
    {
      get { return fDomicilio; }
      set { SetPropertyValue( "Domicilio", ref fDomicilio, value ); }
    }

    [ Browsable( false ) ]
    [ NonPersistent ]
    public string DomicilioLinea1
    {
      get
      {
        if( string.IsNullOrEmpty( fDomicilio ) ) return "";
        var domicilioParts = fDomicilio.Split( '-' );
        return domicilioParts[ 0 ].Trim( );
      }
    }

    [ Browsable( false ) ]
    [ NonPersistent ]
    public string DomicilioLinea2
    {
      get
      {
        if( string.IsNullOrEmpty( fDomicilio ) ) return "";
        var domicilioParts = fDomicilio.Split( '-' );
        return domicilioParts.Length > 1 ? domicilioParts[ 1 ].Trim( ) : string.Empty;
      }
    }

    [ Browsable( false ) ]
    [ NonPersistent ]
    public string DomicilioLinea3
    {
      get
      {
        if( string.IsNullOrEmpty( fDomicilio ) ) return "";
        var domicilioParts = fDomicilio.Split( '-' );
        return domicilioParts.Length > 2 ? domicilioParts[ 2 ].Trim( ) : string.Empty;
      }
    }

    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Tipo de identificación" ) ]
    [VisibleInListView(false)]
    public IdentificacionTipo IdentificacionTipo
    {
      get { return fIdentificacionTipo; }
      set { SetPropertyValue( "IdentificacionTipo", ref fIdentificacionTipo, value ); }
    }

    [System.ComponentModel.DisplayName("Nro. de identificación")]
    [VisibleInListView(false)]
    public string IdentificacionNro
    {
      get { return fIdentificacionNro; }
      set { SetPropertyValue( "IdentificacionNro", ref fIdentificacionNro, value ); }
    }

    [ Browsable( false ) ]
    [ System.ComponentModel.DisplayName( "Nro. de identificación (fmt)" ) ]
    public string IdentificacionNroFmt
    {
      get
      {
        var identificacionTipo = IdentificacionTipo;
        if( identificacionTipo == null || string.IsNullOrEmpty( identificacionTipo.Formato ) ) return IdentificacionNro;

        return Convert.ToString( Evaluate( identificacionTipo.Formato.Replace( "{0}", "IdentificacionNro" ) ) );
      }
    }

    [VisibleInDetailView(false)]
    [PersistentAlias("IdentificacionTipo + '-' + IdentificacionNro")]
    [System.ComponentModel.DisplayName("Identificacion")]
    public string Identificacion
    {
        get { return Convert.ToString(EvaluateAlias("Identificacion")); }
    }

    [ System.ComponentModel.DisplayName( "Categoría de IVA" ) ]
    [ DataSourceCriteria( "Impuesto.Oid = 1" ) ]
    public Categoria CategoriaDeIva
    {
      get { return fCategoriaDeIva; }
      set { SetPropertyValue( "CategoriaDeIva", ref fCategoriaDeIva, value ); }
    }

    [ System.ComponentModel.DisplayName( "Condición de pago" ) ]
    public CondicionDePago CondicionDePago
    {
      get { return fCondicionDePago; }
      set { SetPropertyValue( "CondicionDePago", ref fCondicionDePago, value ); }
    }

    [ System.ComponentModel.DisplayName( "Cuenta de fondos" ) ]
    public Cuenta Cuenta
    {
      get { return fCuenta; }
      set { SetPropertyValue( "Cuenta", ref fCuenta, value ); }
    }

    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [System.ComponentModel.DisplayName( "Especie" )]
    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    [ System.ComponentModel.DisplayName( "Cambio" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n2}" ) ]
    [ ModelDefault( "EditMask", "n2" ) ]
    public decimal Cambio
    {
      get { return fCambio; }
      set { SetPropertyValue< decimal >( "Cambio", ref fCambio, value ); }
    }

    [ System.ComponentModel.DisplayName( "Lista de precios" ) ]
    public ListaDePrecios Lista
    {
      get { return fLista; }
      set { SetPropertyValue( "Lista", ref fLista, value ); }
    }

    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Cuotas" ) ]
    public int CantidadCuotas
    {
      get { return fCantidadCuotas; }
      set
      {
        SetPropertyValue< int >( "CantidadCuotas", ref fCantidadCuotas, value );
        if( CanRaiseOnChanged )
        {
          if( Propio )
            GenerarCuotas( );
        }
      }
    }

    [ System.ComponentModel.DisplayName( "Descuento" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n2} %" ) ]
    public decimal Descuento
    {
      get { return fDescuento; }
      set { SetPropertyValue< decimal >( "Descuento", ref fDescuento, value ); }
    }

    [ System.ComponentModel.DisplayName( "Inicio de prestación" ) ]
    public DateTime InicioPrestacion
    {
      get { return fInicioPrestacion; }
      set { SetPropertyValue< DateTime >( "InicioPrestacion", ref fInicioPrestacion, value ); }
    }

    [ System.ComponentModel.DisplayName( "Fin de prestación" ) ]
    public DateTime FinPrestacion
    {
      get { return fFinPrestacion; }
      set { SetPropertyValue< DateTime >( "FinPrestacion", ref fFinPrestacion, value ); }
    }

    [ System.ComponentModel.DisplayName( "Vencimiento" ) ]
    public DateTime? Vencimiento
    {
      get { return fVencimiento; }
      set { SetPropertyValue( "Vencimiento", ref fVencimiento, value ); }
    }

    [ System.ComponentModel.DisplayName( "Período contable" ) ]
    public DateTime FechaContable
    {
      get { return fFechaContable; }
      set { SetPropertyValue< DateTime >( "FechaContable", ref fFechaContable, value ); }
    }

    [ System.ComponentModel.DisplayName( "Notas de cobranza" ) ]
    [ Size( SizeAttribute.Unlimited ) ]
    public string NotasCobranza
    {
      get { return fNotasCobranza; }
      set { SetPropertyValue( "NotasCobranza", ref fNotasCobranza, value ); }
    }

    public string AutorizadaCodigo
    {
      get { return fAutorizadaCodigo; }
      set { SetPropertyValue( "AutorizadaCodigo", ref fAutorizadaCodigo, value ); }
    }

    public DateTime? AutorizadaCodigoFecVto
    {
      get { return fAutorizadaCodigoFecVto; }
      set { SetPropertyValue( "AutorizadaCodigoFecVto", ref fAutorizadaCodigoFecVto, value ); }
    }

    public DateTime? AutorizadaFecha
    {
      get { return fAutorizadaFecha; }
      set { SetPropertyValue( "AutorizadaFecha", ref fAutorizadaFecha, value ); }
    }

    public string AutorizadaNotas
    {
      get { return fAutorizadaNotas; }
      set { SetPropertyValue( "AutorizadaNotas", ref fAutorizadaNotas, value ); }
    }

    public string AutorizadaResultado
    {
      get { return fAutorizadaResultado; }
      set { SetPropertyValue( "AutorizadaResultado", ref fAutorizadaResultado, value ); }
    }

    [ PersistentAlias( "Items.Sum(ImporteTotal)" ) ]
    public decimal ImporteTotal
    {
      get { return Convert.ToDecimal( EvaluateAlias( "ImporteTotal" ) ); }
    }

    public ConceptosIncluidos ConceptosIncluidos
    {
      get { return fConceptosIncluidos; }
      set { SetPropertyValue( "ConceptosIncluidos", ref fConceptosIncluidos, value ); }
    }

    [ ImmediatePostData ]
    [ Association ]
    [ Aggregated ]
    public virtual XPCollection< ComprobanteItem > Items
    {
      get
      {
        var comprobanteItems = GetCollection< ComprobanteItem >( "Items" );
        return comprobanteItems;
      }
    }

    //TODO: ver si usar redundancia (bajar valor) para el saldo de la cuota
    [PersistentAlias("IsNull(Cuotas.Sum(Saldo),0)")]
    public decimal Saldo
    {
        get { return Convert.ToDecimal(EvaluateAlias("Saldo")); } 
    }

    [Association, Aggregated, CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
    public XPCollection< ComprobanteCuota > Cuotas
    {
      get { return GetCollection< ComprobanteCuota >( "Cuotas" ); }
    }

    [ Browsable( false ) ]
    public bool TienePagosAplicados
    {
      get { return Cuotas.Any( cuota => cuota.Aplicaciones.Count > 0 ); }
    }

    public virtual void OnItemsChanged( ComprobanteItem item )
    {
      if( CanRaiseOnChanged )
      {
        if( Propio )
          GenerarCuotas( );
      }
    }

    internal virtual void CopiarDatosPersona( Persona persona )
    {
      var direccion = persona.DireccionPrimaria;
      if( direccion != null )
        Domicilio = direccion.Direccion.DireccionCompleta;

      Nombre = persona.Nombre;

      if( Tipo != null )
      {
        var posiblesIdent = Tipo.IdentificacionesTiposValidos;
        foreach( var identificacion in persona.Identificaciones.Where(
                                                                      identificacion => posiblesIdent.Any( posibleIdent => posibleIdent.IdentificacionTipo.Oid == identificacion.Tipo.Oid ) ) )
        {
          IdentificacionTipo = identificacion.Tipo;
          IdentificacionNro = identificacion.Valor;
        }

        var posiblesCondicionesIva = Tipo.CategoriasValidas;
        foreach( var impuesto in persona.DatosImpositivos.Where( impuesto => impuesto.Categoria.Impuesto.Oid == ( int ) Impuestos.Impuestos.IVA ) )
        {
          foreach( var posibleCondicionesIva in posiblesCondicionesIva )
          {
            if( posibleCondicionesIva.Categoria.Oid == impuesto.Categoria.Oid )
            {
              CategoriaDeIva = impuesto.Categoria;
              break;
            }
          }
        }
      }
    }

    public void GenerarCuotas( )
    {
      var importeTotalItems = ImporteTotal;

      Cuotas.Empty( );

      var primerVto = Vencimiento ?? Fecha;

      for( var c = 0; c < CantidadCuotas; c++ )
      {
        Cuotas.Add( new ComprobanteCuota( Session )
                    {
                      Numero = c + 1,
                      Importe = 0,
                      Fecha = primerVto.AddMonths( c )
                    } );
      }

      if( Cuotas.Count == 0 ) return;

      for( var c = 0; c < Cuotas.Count; c++ )
      {
        var cuota = Cuotas[ c ];
        cuota.Importe = c == 0 ? importeTotalItems : 0;
      }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Especie = Identificadores.GetInstance( Session ).EspeciePredeterminada;

      Fecha = DateTime.Today;
      CantidadCuotas = 1;
      Cambio = 1;
      ConceptosIncluidos = ConceptosIncluidos.Productos;
    }
  }
}
