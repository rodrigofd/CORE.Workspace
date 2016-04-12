using System;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;

namespace FDIT.Core.Seguros
{
  //[DefaultProperty( "Nombre" )]
  [ System.ComponentModel.DisplayName( "Item de rendición de aseguradora" ) ]
  [ Persistent( "seguros.RendicionDeAseguradoraItem" ) ]
  public class RendicionDeAseguradoraItem : BasicObject
  {
    private decimal fCambio;
    private decimal fCambioComision;
    private decimal fComisionCobranzaAplicado;
    private decimal fComisionCobranzaCobrada;
    private decimal fComisionCobranzaOrganizador;
    private decimal fComisionCobranzaProductor;
    private decimal fComisionCobranzaProductorRendidoTasa;
    private decimal fComisionCobranzaTotal;
    private decimal fComisionOrganizadorTotal;
    private decimal fComisionPrimaAplicado;
    private decimal fComisionPrimaCobrada;
    private decimal fComisionPrimaOrganizador;
    private decimal fComisionPrimaProductor;
    private decimal fComisionPrimaProductorRendidoTasa;
    private decimal fComisionPrimaTotal;
    private decimal fComisionProductorTotal;
    private decimal fComisionTotal;
    private ComprobanteCuota fComprobanteCuota;
    private Especie fEspecie;
    private Especie fEspecieComision;
    private DateTime fFechaCobroAseguradora;
    private decimal fImporte;
    private decimal fImporteAplicado;
    private string fNotas;
    private PagoAplicacion fPagoAplicacion;
    private RendicionDeAseguradora fRendicionDeAseguradora;

    public RendicionDeAseguradoraItem( Session session )
      : base( session )
    {
    }

    [ Association ]
    public RendicionDeAseguradora RendicionDeAseguradora
    {
      get { return fRendicionDeAseguradora; }
      set { SetPropertyValue( "RendicionDeAseguradora", ref fRendicionDeAseguradora, value ); }
    }

    public ComprobanteCuota ComprobanteCuota
    {
      get { return fComprobanteCuota; }
      set { SetPropertyValue( "ComprobanteCuota", ref fComprobanteCuota, value ); }
    }

    public PagoAplicacion PagoAplicacion
    {
      get { return fPagoAplicacion; }
      set { SetPropertyValue( "PagoAplicacion", ref fPagoAplicacion, value ); }
    }

    public DateTime FechaCobroAseguradora
    {
      get { return fFechaCobroAseguradora; }
      set { SetPropertyValue< DateTime >( "FechaCobroAseguradora", ref fFechaCobroAseguradora, value ); }
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

    public decimal ImporteAplicado
    {
      get { return fImporteAplicado; }
      set { SetPropertyValue< decimal >( "ImporteAplicado", ref fImporteAplicado, value ); }
    }

    public decimal ComisionPrimaAplicado
    {
      get { return fComisionPrimaAplicado; }
      set { SetPropertyValue< decimal >( "ComisionPrimaAplicado", ref fComisionPrimaAplicado, value ); }
    }

    public decimal ComisionCobranzaAplicado
    {
      get { return fComisionCobranzaAplicado; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaAplicado", ref fComisionCobranzaAplicado, value ); }
    }

    public Especie EspecieComision
    {
      get { return fEspecieComision; }
      set { SetPropertyValue( "EspecieComision", ref fEspecieComision, value ); }
    }

    public decimal CambioComision
    {
      get { return fCambioComision; }
      set { SetPropertyValue< decimal >( "CambioComision", ref fCambioComision, value ); }
    }

    public decimal ComisionPrimaOrganizador
    {
      get { return fComisionPrimaOrganizador; }
      set { SetPropertyValue< decimal >( "ComisionPrimaOrganizador", ref fComisionPrimaOrganizador, value ); }
    }

    public decimal ComisionPrimaProductor
    {
      get { return fComisionPrimaProductor; }
      set { SetPropertyValue< decimal >( "ComisionPrimaProductor", ref fComisionPrimaProductor, value ); }
    }

    public decimal ComisionCobranzaOrganizador
    {
      get { return fComisionCobranzaOrganizador; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaOrganizador", ref fComisionCobranzaOrganizador, value ); }
    }

    public decimal ComisionCobranzaProductor
    {
      get { return fComisionCobranzaProductor; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaProductor", ref fComisionCobranzaProductor, value ); }
    }

    public decimal ComisionOrganizadorTotal
    {
      get { return fComisionOrganizadorTotal; }
      set { SetPropertyValue< decimal >( "ComisionOrganizadorTotal", ref fComisionOrganizadorTotal, value ); }
    }

    public decimal ComisionProductorTotal
    {
      get { return fComisionProductorTotal; }
      set { SetPropertyValue< decimal >( "ComisionProductorTotal", ref fComisionProductorTotal, value ); }
    }

    public decimal ComisionPrimaTotal
    {
      get { return fComisionPrimaTotal; }
      set { SetPropertyValue< decimal >( "ComisionPrimaTotal", ref fComisionPrimaTotal, value ); }
    }

    public decimal ComisionCobranzaTotal
    {
      get { return fComisionCobranzaTotal; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaTotal", ref fComisionCobranzaTotal, value ); }
    }

    public decimal ComisionTotal
    {
      get { return fComisionTotal; }
      set { SetPropertyValue< decimal >( "ComisionTotal", ref fComisionTotal, value ); }
    }

    public decimal ComisionPrimaProductorRendidoTasa
    {
      get { return fComisionPrimaProductorRendidoTasa; }
      set { SetPropertyValue< decimal >( "ComisionPrimaProductorRendidoTasa", ref fComisionPrimaProductorRendidoTasa, value ); }
    }

    public decimal ComisionCobranzaProductorRendidoTasa
    {
      get { return fComisionCobranzaProductorRendidoTasa; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaProductorRendidoTasa", ref fComisionCobranzaProductorRendidoTasa, value ); }
    }

    public decimal ComisionPrimaCobrada
    {
      get { return fComisionPrimaCobrada; }
      set { SetPropertyValue< decimal >( "ComisionPrimaCobrada", ref fComisionPrimaCobrada, value ); }
    }

    public decimal ComisionCobranzaCobrada
    {
      get { return fComisionCobranzaCobrada; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaCobrada", ref fComisionCobranzaCobrada, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [ Association ]
    [ Aggregated ]
    public XPCollection< LiquidacionAIntermediarioItem > Items
    {
      get { return GetCollection< LiquidacionAIntermediarioItem >( "Items" ); }
    }
  }
}
