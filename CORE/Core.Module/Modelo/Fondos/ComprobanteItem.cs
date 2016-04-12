using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.ComprobanteItem" ) ]
  [ System.ComponentModel.DisplayName( "Item de comprobante" ) ]
  [ RuleCriteria( "ValidaciondeFondosCompItemApertura", DefaultContexts.Save, "Apertura.SUM(Porcentaje) = 100 OR NOT Cuenta.ExigeApertura",
    "La apertura por centro de costo debe sumar 100% para la cuenta {Cuenta}." ) ]
  [ RuleCriteria( "ValidaciondeFondosCompItemValores", DefaultContexts.Save, "Valores.SUM(Valor.ImporteAlCambio) = ImporteAlCambio OR NOT Especie.ExigeValor",
    "Debe ingresar o asociar los valores afectados en este movimiento." ) ]
  [ Appearance( "FDIT.Core.Fondos.ComprobanteItem.NoEditableSiAutogenerado", "Autogenerado = TRUE OR (NOT ISNULL(Comprobante) AND Comprobante.Estado <> 'Pendiente' )", Enabled = false, TargetItems = "*" ) ]
  [ DefaultProperty( "Descripcion" ) ]
  public class ComprobanteItem : BasicObject
  {
    private bool fAutogenerado;
    private decimal fCambio;
    private Comprobante fComprobante;
    private Cuenta fCuenta;
    private DebeHaber fDebeHaber;
    private Especie fEspecie;
    private decimal fImporte;

    public ComprobanteItem( Session session )
      : base( session )
    {
    }
 
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "IIF(NOT ISNULL(Comprobante),CONCAT(Comprobante.Descripcion,' - '),'') + IIF(NOT ISNULL(Cuenta),CONCAT(Cuenta.Nombre,' (',DebeHaber,')'),'')" ) ]
    public string Descripcion
    {
      get { return ( string ) EvaluateAlias( "Descripcion" ); }
    }

    [ Association ]
    public Comprobante Comprobante
    {
      get { return fComprobante; }
      set { SetPropertyValue( "Comprobante", ref fComprobante, value ); }
    }

    [ ImmediatePostData ]
    [ Appearance( "FCI_Cuenta_DeshabilitadoSiHayValores", "Valores.COUNT() > 0", Enabled = false ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ RuleRequiredField ]
    public Cuenta Cuenta
    {
      get { return fCuenta; }
      set
      {
        SetPropertyValue( "Cuenta", ref fCuenta, value );
        if( CanRaiseOnChanged )
        {
          if( Cuenta != null && Cuenta.EspeciePredeterminada != null )
          {
            Especie = Cuenta.EspeciePredeterminada;
          }
        }
      }
    }

    [ RuleRequiredField ]
    [ ImmediatePostData ]
    public DebeHaber DebeHaber
    {
      get { return fDebeHaber; }
      set { SetPropertyValue( "DebeHaber", ref fDebeHaber, value ); }
    }

    [ Appearance( "FCI_Importe_HabilitadoSegunEspecie", "Especie != NULL AND Especie.ExigeValor", Enabled = false ) ]
    [ RuleRequiredField ]
    [ ImmediatePostData ]
    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    [ Appearance( "FCI_Especie_DeshabilitadoSiHayValores", "Valores.COUNT() > 0", Enabled = false ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ ImmediatePostData ]
    [ RuleRequiredField ]
    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    [ Appearance( "FCI_Cambio_HabilitadoSegunEspecie", "Especie != NULL AND Especie.ExigeValor", Enabled = false ) ]
    [ RuleRequiredField ]
    [ ImmediatePostData ]
    [ ModelDefault( "DisplayFormat", "n5" ) ]
    public decimal Cambio
    {
      get { return fCambio; }
      set { SetPropertyValue< decimal >( "Cambio", ref fCambio, value ); }
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * IIF(DebeHaber='Debe',1,0)" ) ]
    public decimal Debe
    {
      get { return ( decimal ) EvaluateAlias( "Debe" ); }
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * IIF(DebeHaber='Haber',1,0)" ) ]
    public decimal Haber
    {
      get { return ( decimal ) EvaluateAlias( "Haber" ); }
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * IIF(DebeHaber='Debe',1,0) * Cambio" ) ]
    public decimal DebeAlCambio
    {
      get { return ( decimal ) EvaluateAlias( "DebeAlCambio" ); }
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * IIF(DebeHaber='Haber',1,0) * Cambio" ) ]
    public decimal HaberAlCambio
    {
      get { return ( decimal ) EvaluateAlias( "Haber" ); }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * Cambio" ) ]
    public decimal ImporteAlCambio
    {
      get { return ( decimal ) EvaluateAlias( "ImporteAlCambio" ); }
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    public bool Autogenerado
    {
      get { return fAutogenerado; }
      set { SetPropertyValue( "Autogenerado", ref fAutogenerado, value ); }
    }

    [ Association ]
    [ Aggregated ]
    public XPCollection< ComprobanteItemValor > Valores
    {
      get { return GetCollection< ComprobanteItemValor >( "Valores" ); }
    }

    [ Association ]
    [ Aggregated ]
    [ System.ComponentModel.DisplayName( "Apertura por centro de costo" ) ]
    public XPCollection< ComprobanteItemApertura > Apertura
    {
      get { return GetCollection< ComprobanteItemApertura >( "Apertura" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Cambio = 1;
      DebeHaber = DebeHaber.Debe;
    }

    protected override XPCollection< T > CreateCollection< T >( XPMemberInfo property )
    {
      //interceptar la creación de colección Valores; para actuar ante cualquier A/B/M de los mismos (totalizadores)
      var collection = base.CreateCollection< T >( property );

      if( property.Name == "Valores" )
        collection.ListChanged += Valores_ListChanged;

      return collection;
    }

    private void Valores_ListChanged( object sender, ListChangedEventArgs e )
    {
      //Ante cualquier cambio en la colección de valores, sumarizar el importe al cambio, y tomar un tipo de cambio ponderado (para este item)
      var importeTotalValores = Evaluate( "Valores.SUM(Valor.Importe)" );
      Importe = importeTotalValores != null ? ( decimal ) importeTotalValores : 0;

      var importeAlCambioTotalValores = Evaluate( "Valores.SUM(Valor.ImporteAlCambio)" );
      if( importeAlCambioTotalValores != null && ( decimal ) importeAlCambioTotalValores != 0 )
        Cambio = ( decimal ) importeAlCambioTotalValores / Importe;
    }

    protected override void OnSaving( )
    {
      base.OnSaving( );
    }
  }
}
