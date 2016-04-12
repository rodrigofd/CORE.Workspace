using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.Valor" ) ]
  [ System.ComponentModel.DisplayName( "Valor" ) ]
  [ ImageName( "money-coin" ) ]
  public class Valor : BasicObject, IObjetoPorEmpresa
  {
    private decimal fCambio;
    private string fConcepto;
    private Empresa fEmpresa;
    private Especie fEspecie;
    private DateTime? fFechaAcreditado;
    private DateTime fFechaAlta;
    private decimal fImporte;
    private ComprobanteItem fUltimoMovimiento;

    public Valor( Session session ) : base( session )
    {
    }

    public DateTime FechaAlta
    {
      get { return fFechaAlta; }
      set { SetPropertyValue< DateTime >( "FechaAlta", ref fFechaAlta, value ); }
    }

    public DateTime? FechaAcreditado
    {
      get { return fFechaAcreditado; }
      set { SetPropertyValue( "FechaAcreditado", ref fFechaAcreditado, value ); }
    }

    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    public decimal Cambio
    {
      get { return fCambio; }
      set { SetPropertyValue< decimal >( "Cambio", ref fCambio, value ); }
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Importe * Cambio" ) ]
    public decimal ImporteAlCambio
    {
      get { return ( decimal ) EvaluateAlias( "ImporteAlCambio" ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Concepto
    {
      get { return fConcepto; }
      set { SetPropertyValue( "Concepto", ref fConcepto, value ); }
    }

    [ ModelDefault( "AllowEdit", "false" ) ]
    public ComprobanteItem UltimoMovimiento
    {
      get { return fUltimoMovimiento; }
      set { SetPropertyValue( "UltimoMovimiento", ref fUltimoMovimiento, value ); }
    }

    [ System.ComponentModel.DisplayName( "Historial de movimientos" ) ]
    [ CollectionOperationSet( AllowAdd = false, AllowRemove = false ) ]
    [ Association ]
    public XPCollection< ComprobanteItemValor > ComprobanteItems
    {
      get { return GetCollection< ComprobanteItemValor >( "ComprobanteItems" ); }
    }

    [Browsable(false)]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      FechaAlta = DateTime.Today;
      Cambio = 1;
    }

    public virtual void CalcularEstado( )
    {
      //calcular ultimo movimiento del valor (en comprobantes de fondos)

      //ejecutamos una vista, a los vinculos del valor con comprobantes (ComprobanteItemValor), ordenando los comprobantes de manera:
      //FECHA DESC, OID CAB DESC, DEBEHABER DESC, OID ITEM DESC
      //El primero devuelto es el ultimo movimiento
      var viewUltimoMovimiento = new XPView( Session, typeof( ComprobanteItemValor ) ) { Criteria = CriteriaOperator.Parse( "Valor = ?", this ) };

      viewUltimoMovimiento.AddProperty( "ComprobanteFecha", "ComprobanteItem.Comprobante.Fecha", false, true, SortDirection.Descending );
      viewUltimoMovimiento.AddProperty( "ComprobanteId", "ComprobanteItem.Comprobante.Oid", false, true, SortDirection.Descending );
      viewUltimoMovimiento.AddProperty( "ComprobanteItemDebeHaber", "ComprobanteItem.DebeHaber", false, true, SortDirection.Descending );
      viewUltimoMovimiento.AddProperty( "ComprobanteItemId", "ComprobanteItem.Oid", false, true, SortDirection.Descending );

      UltimoMovimiento = viewUltimoMovimiento.Count == 0 ? null : Session.GetObjectByKey< ComprobanteItem >( ( int ) viewUltimoMovimiento[ 0 ][ "ComprobanteItemId" ] );
    }
  }
}
