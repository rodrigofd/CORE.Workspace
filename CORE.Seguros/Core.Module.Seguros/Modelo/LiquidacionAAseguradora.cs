using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Sistema;
using Comprobante = FDIT.Core.Gestion.Comprobante;

namespace FDIT.Core.Seguros
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Seguros" ) ]
  [ System.ComponentModel.DisplayName( "Liquidación a aseguradora" ) ]
  [ Persistent( "seguros.LiquidacionAAseguradora" ) ]
  public class LiquidacionAAseguradora : BasicObject, IObjetoPorEmpresa
  {
    private Aseguradora fAseguradora;
    private Comprobante fComprobante;
    private Empresa fEmpresa;
    private Especie fEspecieCobrada;
    private Especie fEspecieDocumento;
    private DateTime fFecha;
    private decimal fIva1Tasa;
    private string fLiquidacion;
    private string fNotas;
    private int fNumero;
    private decimal fRetencion1Tasa;
    private decimal fRetencion2Tasa;
    private decimal fRetencion3Tasa;
    private decimal fRetencion4Tasa;
    private TipoLiquidacion fTipo;

    public LiquidacionAAseguradora( Session session ) : base( session )
    {
    }

    public Comprobante Comprobante
    {
      get { return fComprobante; }
      set { SetPropertyValue( "Comprobante", ref fComprobante, value ); }
    }

    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue< int >( "Numero", ref fNumero, value ); }
    }

    public DateTime Fecha
    {
      get { return fFecha; }
      set { SetPropertyValue< DateTime >( "Fecha", ref fFecha, value ); }
    }

    public Aseguradora Aseguradora
    {
      get { return fAseguradora; }
      set { SetPropertyValue( "Aseguradora", ref fAseguradora, value ); }
    }

    public TipoLiquidacion Tipo
    {
      get { return fTipo; }
      set { SetPropertyValue( "Tipo", ref fTipo, value ); }
    }

    public Especie EspecieDocumento
    {
      get { return fEspecieDocumento; }
      set { SetPropertyValue( "EspecieDocumento", ref fEspecieDocumento, value ); }
    }

    public Especie EspecieCobrada
    {
      get { return fEspecieCobrada; }
      set { SetPropertyValue( "EspecieCobrada", ref fEspecieCobrada, value ); }
    }

    public decimal Iva1Tasa
    {
      get { return fIva1Tasa; }
      set { SetPropertyValue< decimal >( "Iva1Tasa", ref fIva1Tasa, value ); }
    }

    public decimal Retencion1Tasa
    {
      get { return fRetencion1Tasa; }
      set { SetPropertyValue< decimal >( "Retencion1Tasa", ref fRetencion1Tasa, value ); }
    }

    public decimal Retencion2Tasa
    {
      get { return fRetencion2Tasa; }
      set { SetPropertyValue< decimal >( "Retencion2Tasa", ref fRetencion2Tasa, value ); }
    }

    public decimal Retencion3Tasa
    {
      get { return fRetencion3Tasa; }
      set { SetPropertyValue< decimal >( "Retencion3Tasa", ref fRetencion3Tasa, value ); }
    }

    public decimal Retencion4Tasa
    {
      get { return fRetencion4Tasa; }
      set { SetPropertyValue< decimal >( "Retencion4Tasa", ref fRetencion4Tasa, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Liquidacion
    {
      get { return fLiquidacion; }
      set { SetPropertyValue( "Liquidacion", ref fLiquidacion, value ); }
    }

    [ Association ]
    public XPCollection< LiquidacionAAseguradoraItem > Items
    {
      get { return GetCollection< LiquidacionAAseguradoraItem >( "Items" ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }
  }
}
