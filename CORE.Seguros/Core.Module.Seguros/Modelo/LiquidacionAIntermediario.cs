using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Personas;
using FDIT.Core.Sistema;
using Comprobante = FDIT.Core.Gestion.Comprobante;

namespace FDIT.Core.Seguros
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Seguros" ) ]
  [ System.ComponentModel.DisplayName( "Liquidación a intermediario" ) ]
  [ Persistent( "seguros.LiquidacionAIntermediario" ) ]
  public class LiquidacionAIntermediario : BasicObject, IObjetoPorEmpresa
  {
    private decimal fALiquidar;
    private Comprobante fComprobante;
    private Empresa fEmpresa;
    private Especie fEspecie;
    private DateTime fFecha;
    private DateTime fFechaCerrada;
    private decimal fGastos;
    private Persona fIntermediario;
    private decimal fIva;
    private string fNotas;
    private int fNumero;
    private decimal fRendido;

    public LiquidacionAIntermediario( Session session )
      : base( session )
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

    public Persona Intermediario
    {
      get { return fIntermediario; }
      set { SetPropertyValue( "Intermediario", ref fIntermediario, value ); }
    }

    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    public decimal Gastos
    {
      get { return fGastos; }
      set { SetPropertyValue< decimal >( "Gastos", ref fGastos, value ); }
    }

    public decimal Iva
    {
      get { return fIva; }
      set { SetPropertyValue< decimal >( "Iva", ref fIva, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    public DateTime FechaCerrada
    {
      get { return fFechaCerrada; }
      set { SetPropertyValue< DateTime >( "FechaCerrada", ref fFechaCerrada, value ); }
    }

    public decimal ALiquidar
    {
      get { return fALiquidar; }
      set { SetPropertyValue< decimal >( "ALiquidar", ref fALiquidar, value ); }
    }

    public decimal Rendido
    {
      get { return fRendido; }
      set { SetPropertyValue< decimal >( "Rendido", ref fRendido, value ); }
    }

    [ Aggregated ]
    [ Association ]
    public XPCollection< LiquidacionAIntermediarioItem > Items
    {
      get { return GetCollection< LiquidacionAIntermediarioItem >( "LiquidacionesAIntermediariosItems" ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }
  }
}
