using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Compras;
using FDIT.Core.Impuestos;
using FDIT.Core.Sistema;
using FDIT.Core.Stock;
using FDIT.Core.Util;

namespace FDIT.Core.Gestion
{
  [FiltroPorEmpresa("ISNULL(Comprobante) OR Comprobante.Empresa.Oid = ?")]
  [Persistent("gestion.ComprobanteItem")]
  [ System.ComponentModel.DisplayName( "Items del comprobante" ) ]
  [ RuleCriteria( "FDIT.Core.Gestion.ComprobanteItem.Apertura_Suma", 
                  DefaultContexts.Save, 
                  "Apertura.COUNT() = 0 OR Apertura.SUM(Porcentaje) = 100", 
                  CustomMessageTemplate = "La suma de porcentajes de los centros de costo debe ser 100" ) ]
  public class ComprobanteItem : BasicObject
  {
    private decimal fAlicuota;
    private Articulo fArticulo;
    private decimal fBaseImponible;
    private decimal fCantidad;
    protected Comprobante fComprobante;
    private Concepto fConcepto;
    private decimal fDescuento;
    private string fDetalle;
    private bool fGenerado;
    private decimal fImporteDescuento;
    private decimal fImporteDescuentoConImpuesto;
    private decimal fImporteImpuestos;
    private decimal fImporteTotal;
    private decimal fImporteTotalConImpuesto;
    private decimal fPrecioUnitario;
    private decimal fPrecioUnitarioConImpuesto;
    private Regimen fRegimen;
    private UnidadMedida fUnidadMedida;

    public ComprobanteItem( Session session )
      : base( session )
    {
    }

    [ ImmediatePostData ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Artículo" ) ]
    [ Persistent( @"id_articulo" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    [ System.ComponentModel.DisplayName( "Detalle" ) ]
    public string Detalle
    {
      get { return fDetalle; }
      set { SetPropertyValue( "Detalle", ref fDetalle, value ); }
    }

    [ ImmediatePostData ]
    [ RuleRequiredField ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Concepto" ) ]
    public Concepto Concepto
    {
      get { return fConcepto; }
      set
      {
        SetPropertyValue( "Concepto", ref fConcepto, value );
        if( CanRaiseOnChanged )
        {
          if( Concepto != null )
          {
            Apertura.Empty( );

            foreach(
              var nuevaApertura in
                Concepto.Apertura.Select(
                                         nuevoItem =>
                                         new ComprobanteItemApertura( Session )
                                         {
                                           CentroDeCosto = nuevoItem.CentroDeCosto,
                                           Porcentaje = nuevoItem.Porcentaje
                                         } ) )
            {
              Apertura.Add( nuevaApertura );
            }
            OnChanged( "Apertura" );

            if( Concepto.Regimen != null )
              Regimen = Concepto.Regimen;

            if( !Concepto.DetallaCantidad ) Cantidad = 1;

            ActualizarDescripcion( );
            ActualizarImporteTotal( );
          }
        }
      }
    }

    [ Appearance( "mostrar_cantidad", Visibility = ViewItemVisibility.Hide, Criteria = "Concepto.DetallaCantidad = false" ) ]
    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ ModelDefault( "DisplayFormat", "{0:n6}" ) ]
    [ ModelDefault( "EditMask", "n6" ) ]
    public decimal Cantidad
    {
      get { return fCantidad; }
      set
      {
        SetPropertyValue< decimal >( "Cantidad", ref fCantidad, value );

        if( CanRaiseOnChanged )
        {
          actualizarImporteDescuento( );
          ActualizarImporteTotal( );
        }
      }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Precio unitario" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n6}" ) ]
    [ ModelDefault( "EditMask", "n6" ) ]
    public decimal PrecioUnitario
    {
      get { return fPrecioUnitario; }
      set
      {
        SetPropertyValue< decimal >( "PrecioUnitario", ref fPrecioUnitario, value );

        if( CanRaiseOnChanged )
        {
          actualizarImporteDescuento( );
          ActualizarImporteTotal( );
        }
      }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ System.ComponentModel.DisplayName( "Precio unitario con imp." ) ]
    [ ModelDefault( "DisplayFormat", "{0:n6}" ) ]
    [ ModelDefault( "EditMask", "n6" ) ]
    public decimal PrecioUnitarioConImpuesto
    {
      get { return fPrecioUnitarioConImpuesto; }
      set { SetPropertyValue< decimal >( "PrecioUnitario", ref fPrecioUnitarioConImpuesto, value ); }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Descuento ($)" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n6} %" ) ]
    public decimal ImporteDescuento
    {
      get { return fImporteDescuento; }
      set
      {
        SetPropertyValue< decimal >( "Descuento", ref fImporteDescuento, value );

        if( CanRaiseOnChanged )
        {
          var bas = PrecioUnitario * Cantidad;
          fDescuento = ( ImporteDescuento / bas ) * 100;
          OnChanged( "Descuento" );

          ActualizarImporteTotal( );
        }
      }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Descuento (%)" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n6} %" ) ]
    public decimal Descuento
    {
      get { return fDescuento; }
      set
      {
        SetPropertyValue< decimal >( "Descuento", ref fDescuento, value );

        if( CanRaiseOnChanged )
        {
          actualizarImporteDescuento( );

          ActualizarImporteTotal( );
        }
      }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ System.ComponentModel.DisplayName( "Importe Descuento con imp." ) ]
    [ ModelDefault( "DisplayFormat", "{0:n6}" ) ]
    [ ModelDefault( "EditMask", "n6" ) ]
    public decimal ImporteDescuentoConImpuesto
    {
      get { return fImporteDescuentoConImpuesto; }
      set { SetPropertyValue< decimal >( "ImporteDescuentoConImpuesto", ref fImporteDescuentoConImpuesto, value ); }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Impuestos" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n2}" ) ]
    [ ModelDefault( "EditMask", "n2" ) ]
    public decimal ImporteImpuestos
    {
      get { return fImporteImpuestos; }
      set { SetPropertyValue< decimal >( "ImporteImpuestos", ref fImporteImpuestos, value ); }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Importe total" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n2}" ) ]
    [ ModelDefault( "EditMask", "n2" ) ]
    public decimal ImporteTotal
    {
      get { return fImporteTotal; }
      set { SetPropertyValue< decimal >( "ImporteTotal", ref fImporteTotal, value ); }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Importe total (c/impuestos)" ) ]
    [ ModelDefault( "DisplayFormat", "{0:n2}" ) ]
    [ ModelDefault( "EditMask", "n2" ) ]
    public decimal ImporteTotalConImpuesto
    {
      get { return fImporteTotalConImpuesto; }
      set { SetPropertyValue< decimal >( "ImporteTotalConImpuesto", ref fImporteTotalConImpuesto, value ); }
    }

    [ Browsable( false ) ]
    public bool Generado
    {
      get { return fGenerado; }
      set { SetPropertyValue( "Generado", ref fGenerado, value ); }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ ModelDefault( "DisplayFormat", "n2" ) ]
    [ System.ComponentModel.DisplayName( "Base imponible" ) ]
    public decimal BaseImponible
    {
      get { return fBaseImponible; }
      set { SetPropertyValue< decimal >( "BaseImponible", ref fBaseImponible, value ); }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ ModelDefault( "DisplayFormat", "n2" ) ]
    [ System.ComponentModel.DisplayName( "Alícuota" ) ]
    public decimal Alicuota
    {
      get { return fAlicuota; }
      set { SetPropertyValue< decimal >( "Alicuota", ref fAlicuota, value ); }
    }

    [ VisibleInListView( false ) ]
    [ System.ComponentModel.DisplayName( "Regimen de impuestos" ) ]
    public Regimen Regimen
    {
      get { return fRegimen; }
      set { SetPropertyValue( "Regimen", ref fRegimen, value ); }
    }

    [ Association ]
    [ Aggregated ]
    [ System.ComponentModel.DisplayName( "Apertura por centro de costo" ) ]
    public XPCollection< ComprobanteItemApertura > Apertura
    {
      get { return GetCollection< ComprobanteItemApertura >( "Apertura" ); }
    }

    [VisibleInListView(false)]
    [VisibleInDetailView(false)]
    [ Association ]
    public Comprobante Comprobante
    {
      get { return fComprobante; }
      set { SetPropertyValue( "Comprobante", ref fComprobante, value ); }
    }

    [ VisibleInListView( false ) ]
    [ System.ComponentModel.DisplayName( "Importe" ) ]
    [ NonPersistent ]
    public decimal ImporteTotalVisualizar
    {
      get
      {
        var incluirImp = true;

        var tipoComp = Comprobante.Tipo;
        if( tipoComp != null )
          incluirImp = !tipoComp.DiscriminaImpuestos;

        return incluirImp ? ImporteTotalConImpuesto : ImporteTotal;
      }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public bool MostrarEnImpresion
    {
      get { return Comprobante.Tipo != null && ( Comprobante.Tipo.DiscriminaImpuestos || !( Concepto != null && Concepto.Grupo != null && Concepto.Grupo.EsIVA ) ); }
    }

    public UnidadMedida UnidadMedida
    {
      get { return fUnidadMedida; }
      set { SetPropertyValue( "UnidadMedida", ref fUnidadMedida, value ); }
    }

    private void actualizarImporteDescuento( )
    {
      var bas = PrecioUnitario * Cantidad;
      fImporteDescuento = bas * ( Descuento / 100 );
      OnChanged( "ImporteDescuento" );
    }

    public virtual void ActualizarDescripcion( )
    {
      if( Concepto != null )
        Detalle = Concepto.Nombre;

      if( Articulo != null )
        Detalle = Articulo.Codigo;
    }

    public void ActualizarImporteTotal( )
    {
      ImporteTotal = ( Cantidad * PrecioUnitario ) * ( 1 - ( Descuento / 100 ) );

      var importeTotConImp = ImporteTotal;
      var precioUnitarioConImp = PrecioUnitario;
      var importeDescuentoConImp = ImporteDescuento;
      var importeImpuestos = 0.00m;

      if( Concepto != null && Concepto.AlicuotaImpuesto != null )
      {
        var alic = Concepto.AlicuotaImpuesto.Valor;
        importeImpuestos = Math.Round( ImporteTotal * ( alic / 100 ), 2 );
        precioUnitarioConImp = Math.Round( precioUnitarioConImp * ( 1 + alic / 100 ), 2 );
        importeDescuentoConImp = Math.Round( importeDescuentoConImp * ( 1 + alic / 100 ), 2 );

        importeTotConImp = Math.Round( ( Cantidad * precioUnitarioConImp ) - importeDescuentoConImp, 2 );
      }

      ImporteImpuestos = importeImpuestos;
      PrecioUnitarioConImpuesto = precioUnitarioConImp;
      ImporteDescuentoConImpuesto = importeDescuentoConImp;
      ImporteTotalConImpuesto = importeTotConImp;
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Cantidad = 1;

      if( Comprobante != null && Comprobante.Originante != null )
      {
        var impuestoGanancias = Comprobante.Originante.DatosImpositivos.FirstOrDefault( impuesto => impuesto.Categoria.Impuesto.Oid == ( int ) Impuestos.Impuestos.Ganancias );
        if( impuestoGanancias != null && impuestoGanancias.Regimen != null )
          Regimen = impuestoGanancias.Regimen;

        var proveedor = Session.FindObject< Proveedor >( new BinaryOperator( "Persona.Oid", Comprobante.Originante.Oid ) );
        if( proveedor != null )
        {
          Concepto = proveedor.ConceptoPredeterminado;
        }
      }
    }

    protected override void OnChanged( string propertyName, object oldValue, object newValue )
    {
      base.OnChanged( propertyName, oldValue, newValue );

      if( propertyName == "ImporteTotal" && Comprobante != null )
      {
        Comprobante.OnItemsChanged( this );
      }
      else if( propertyName == "Comprobante" )
      {
        if( oldValue != null ) ( ( Comprobante ) oldValue ).OnItemsChanged( this );
        if( newValue != null ) ( ( Comprobante ) newValue ).OnItemsChanged( this );
      }
    }
  }
}
