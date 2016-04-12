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
  //[DefaultProperty( "Nombre" )]
  [ System.ComponentModel.DisplayName( "Rendición de aseguradora" ) ]
  [ Persistent( "seguros.RendicionDeAseguradora" ) ]
  public class RendicionDeAseguradora : BasicObject, IObjetoPorEmpresa
  {
    private decimal fAjustes;
    private Aseguradora fAseguradora;
    private decimal fCambioPropuesto;
    private Comprobante fComprobante;
    private Empresa fEmpresa;
    private Especie fEspecie;
    private DateTime fFecha;
    private DateTime fFechaCerrada;
    private DateTime fFechaRendicion;
    private string fNotas;
    private int fNumero;
    private string fNumeroCompleto;
    private DateTime fPeriodoDesde;
    private DateTime fPeriodoHasta;
    private decimal fSaldoFinal;
    private decimal fSaldoInicio;

    public RendicionDeAseguradora( Session session )
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

    [ Size( 4000 ) ]
    public string NumeroCompleto
    {
      get { return fNumeroCompleto; }
      set { SetPropertyValue( "NumeroCompleto", ref fNumeroCompleto, value ); }
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

    public DateTime FechaRendicion
    {
      get { return fFechaRendicion; }
      set { SetPropertyValue< DateTime >( "FechaRendicion", ref fFechaRendicion, value ); }
    }

    public DateTime PeriodoDesde
    {
      get { return fPeriodoDesde; }
      set { SetPropertyValue< DateTime >( "PeriodoDesde", ref fPeriodoDesde, value ); }
    }

    public DateTime PeriodoHasta
    {
      get { return fPeriodoHasta; }
      set { SetPropertyValue< DateTime >( "PeriodoHasta", ref fPeriodoHasta, value ); }
    }

    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    public decimal CambioPropuesto
    {
      get { return fCambioPropuesto; }
      set { SetPropertyValue< decimal >( "CambioPropuesto", ref fCambioPropuesto, value ); }
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

    public decimal SaldoInicio
    {
      get { return fSaldoInicio; }
      set { SetPropertyValue< decimal >( "SaldoInicio", ref fSaldoInicio, value ); }
    }

    public decimal Ajustes
    {
      get { return fAjustes; }
      set { SetPropertyValue< decimal >( "Ajustes", ref fAjustes, value ); }
    }

    public decimal SaldoFinal
    {
      get { return fSaldoFinal; }
      set { SetPropertyValue< decimal >( "SaldoFinal", ref fSaldoFinal, value ); }
    }

    [ Aggregated ]
    [ Association ]
    public XPCollection< RendicionDeAseguradoraItem > Items
    {
      get { return GetCollection< RendicionDeAseguradoraItem >( "Items" ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }
  }
}
