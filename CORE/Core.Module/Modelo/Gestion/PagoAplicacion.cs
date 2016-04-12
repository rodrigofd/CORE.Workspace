using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Gestion
{
  [ ImageName( "receipt_stamp" ) ]
  [ Persistent( @"gestion.PagoAplicacion" ) ]
  [ System.ComponentModel.DisplayName( "Aplicación de comprobante" ) ]
  [ RuleCriteria( "FDIT.Core.Gestion.PagoAplicacion.AplicadoSaldo",
    DefaultContexts.Save, "ISNULL(Cuota) OR Cuota.Saldo >= 0", CustomMessageTemplate = "No puede aplicar un importe superior al importe total de la cuota." ) ]
  public class PagoAplicacion : BasicObject
  {
    private decimal fCambio;
    private ComprobanteCuota fCuota;
    private Especie fEspecie;
    private decimal fImporte;
    private string fNotas;
    private Pago fPago;

    public PagoAplicacion( Session session ) : base( session )
    {
    }

    [ Browsable( false ) ]
    [ Association ]
    public Pago Pago
    {
      get { return fPago; }
      set { SetPropertyValue( "Pago", ref fPago, value ); }
    }

    [ Association ]
    [ RuleRequiredField ]
    public ComprobanteCuota Cuota
    {
      get { return fCuota; }
      set { SetPropertyValue( "Cuota", ref fCuota, value ); }
    }

    [ System.ComponentModel.DisplayName( "Especie pagada" ) ]
    [ ImmediatePostData ]
    [ RuleRequiredField ]
    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    [ ImmediatePostData ]
    [ RuleRequiredField ]
    public decimal Importe
    {
      get { return fImporte; }
      set { SetPropertyValue< decimal >( "Importe", ref fImporte, value ); }
    }

    [ System.ComponentModel.DisplayName( "Relación mon. pagada/mon. cuota" ) ]
    [ ImmediatePostData ]
    [ RuleRequiredField ]
    public decimal Cambio
    {
      get { return fCambio; }
      set { SetPropertyValue< decimal >( "Cambio", ref fCambio, value ); }
    }

    [ PersistentAlias( "IIF(Cambio=0,Importe,Importe/Cambio)" ) ]
    public decimal ImporteAplicado
    {
      get { return ( decimal ) EvaluateAlias( "ImporteAplicado" ); }
    }

    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
