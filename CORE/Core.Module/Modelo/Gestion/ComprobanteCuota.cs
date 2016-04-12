using System;
using System.ComponentModel;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Gestion
{
  [ FiltroPorEmpresa( "ISNULL(Comprobante) OR Comprobante.Empresa.Oid = ?" ) ]
  [ Persistent( "gestion.ComprobanteCuota" ) ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Cuotas" ) ]
  public class ComprobanteCuota : BasicObject
  {
    private Comprobante fComprobante;
    private DateTime fFecha;
    private decimal fImporte;
    private int fNumero;

    public ComprobanteCuota( Session session ) : base( session )
    {
    }

    [ PersistentAlias( "IIF(NOT ISNULL(Comprobante),CONCAT(Comprobante.Descripcion,' (C/',TOSTR(Numero),')'),'')" ) ]
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

    [ RuleRequiredField ]
    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue< int >( "Numero", ref fNumero, value ); }
    }

    [ RuleRequiredField ]
    public DateTime Fecha
    {
      get { return fFecha; }
      set { SetPropertyValue< DateTime >( "Fecha", ref fFecha, value ); }
    }

    [ RuleRequiredField ]
    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    //TODO: ver si usar redundancia (bajar valor) para el saldo de la cuota
    [ PersistentAlias( "Importe - IsNull(Aplicaciones[Pago.Estado='Confirmado'].Sum(ImporteAplicado),0)" ) ]
    public decimal Saldo
    {
      get { return ( decimal ) ( EvaluateAlias( "Saldo" ) ); }
    }

    [ Association ]
    public XPCollection< PagoAplicacion > Aplicaciones
    {
      get { return GetCollection< PagoAplicacion >( "Aplicaciones" ); }
    }
  }
}
