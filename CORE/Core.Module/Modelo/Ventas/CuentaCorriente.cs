using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;

namespace FDIT.Core.Ventas
{
  [ NonPersistent ]
  [ ModelDefault( "AllowEdit", "false" ) ]
  public class CuentaCorriente : XPLiteObject
  {
    private Persona fCliente;
    private DateTime? fFechaDesde;
    private DateTime? fFechaHasta;

    private List< CuentaCorrienteItem > fItems;

    public CuentaCorriente( Session Session ) : base( Session )
    {
    }

    [ ImmediatePostData ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ DataSourceProperty( "DestinatariosDisponibles" ) ]
    public Persona Cliente
    {
      get { return fCliente; }
      set { SetPropertyValue( "Cliente", ref fCliente, value ); }
    }

    [ Browsable( false ) ]
    public XPCollection< Persona > DestinatariosDisponibles
    {
      get { return new XPCollection< Persona >( Session, CriteriaOperator.Parse( "[<Cliente>][^.Oid = Persona.Oid]" ) ); }
    }

    [ ImmediatePostData ]
    public DateTime? FechaDesde
    {
      get { return fFechaDesde; }
      set { SetPropertyValue( "FechaDesde", ref fFechaDesde, value ); }
    }

    [ ImmediatePostData ]
    public DateTime? FechaHasta
    {
      get { return fFechaHasta; }
      set { SetPropertyValue( "FechaHasta", ref fFechaHasta, value ); }
    }

    [ CollectionOperationSet( AllowAdd = false, AllowRemove = false ) ]
    public List< CuentaCorrienteItem > Items
    {
      get { return fItems ?? ( fItems = new List< CuentaCorrienteItem >( ) ); }
    }

    [ Action ]
    public void Filtrar( )
    {
      Items.Clear( );

      if( FechaDesde == null || FechaHasta == null ) return;

      var fechaSaldoInicial = FechaDesde.Value.AddDays( -1 );

      var viewSaldoInicial = new XPView( Session, typeof( CuentaCorrienteItem ) );
      viewSaldoInicial.AddProperty( "Saldo" );
      viewSaldoInicial.AddProperty( "Oid" );
      viewSaldoInicial.Criteria = CriteriaOperator.Parse( "Destinatario = ? AND Fecha <= ?", Cliente, fechaSaldoInicial );
      viewSaldoInicial.Sorting.Add( new SortProperty( CriteriaOperator.Parse( "Oid" ), SortingDirection.Descending ) );

      var saldoInicial = viewSaldoInicial.Count > 0 ? ( decimal ) viewSaldoInicial[ 0 ][ "Saldo" ] : 0;

      var itemSaldoInicial = new CuentaCorrienteItem( Session ) { Destinatario = Cliente, Fecha = fechaSaldoInicial, Debitos = 0, Creditos = 0, Saldo = saldoInicial };
      Items.Add( itemSaldoInicial );

      var itemsPeriodo = new XPCollection< CuentaCorrienteItem >( Session ) { Criteria = CriteriaOperator.Parse( "Destinatario = ? AND Fecha >= ? AND Fecha <= ?", Cliente, FechaDesde, FechaHasta ) };
      itemsPeriodo.Reload( );

      foreach( var item in itemsPeriodo )
        Items.Add( item );

      OnChanged( "Items" );
    }
  }

  [ Persistent( "ventas.vCuentaCorriente" ) ]
  public class CuentaCorrienteItem : XPLiteObject
  {
    public CuentaCorrienteItem( Session Session )
      : base( Session )
    {
    }

    [ Key ]
    public int Oid{ get; set; }

    public ComprobanteBase Comprobante{ get; set; }
    public Persona Destinatario{ get; set; }
    public DateTime Fecha{ get; set; }
    public decimal Debitos{ get; set; }
    public decimal Creditos{ get; set; }
    public decimal Saldo{ get; set; }
  }
}
