using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.Cheque" ) ]
  [ System.ComponentModel.DisplayName( "Cheque" ) ]
  [ ImageName( "Cheque" ) ]
  //[DefaultProperty("Nombre")]
    public class Cheque : Valor
  {
    private DateTime fAnulado;
    private string fAnuladoMotivo;
    private Banco fBanco;
    private string fCodigoPostal;
    private bool fCruzado;
    private CuentaChequera fCuentaChequera;
    private ChequeEstado fEstado;
    private DateTime fFecha;
    private DateTime fFechaPagoDiferido;
    private DateTime fFechaVencimiento;
    private bool fNoALaOrden;
    private int fNumero;
    private Persona fOrden;
    private string fOrdenOtro;
    private bool fPropio;
    private string fSerie;
    private string fSucursal;

    public Cheque( Session Session )
      : base( Session )
    {
    }

    [ RuleRequiredField ]
    public bool Propio
    {
      get { return fPropio; }
      set { SetPropertyValue( "Propio", ref fPropio, value ); }
    }

    [ RuleRequiredField ]
    public DateTime Fecha
    {
      get { return fFecha; }
      set
      {
        SetPropertyValue< DateTime >( "Fecha", ref fFecha, value );
        if( CanRaiseOnChanged )
        {
          var dias = Identificadores.GetInstance( Session ).VencimientoChequeDias;
          FechaVencimiento = Fecha.AddDays( dias );
        }
      }
    }

    public DateTime FechaPagoDiferido
    {
      get { return fFechaPagoDiferido; }
      set { SetPropertyValue< DateTime >( "FechaPagoDiferido", ref fFechaPagoDiferido, value ); }
    }

    [ RuleRequiredField ]
    public DateTime FechaVencimiento
    {
      get { return fFechaVencimiento; }
      set { SetPropertyValue< DateTime >( "FechaVencimiento", ref fFechaVencimiento, value ); }
    }

    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ RuleRequiredField ]
    public Banco Banco
    {
      get { return fBanco; }
      set { SetPropertyValue( "Banco", ref fBanco, value ); }
    }

    [LookupEditorMode( LookupEditorMode.AllItems )]
    public CuentaChequera CuentaChequera
    {
      get { return fCuentaChequera; }
      set { SetPropertyValue( "CuentaChequera", ref fCuentaChequera, value ); }
    }

    [ Size( 5 ) ]
    public string Serie
    {
      get { return fSerie; }
      set { SetPropertyValue( "Serie", ref fSerie, value ); }
    }

    [ RuleRequiredField ]
    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue< int >( "Nro", ref fNumero, value ); }
    }

    [ Size( 20 ) ]
    public string Sucursal
    {
      get { return fSucursal; }
      set { SetPropertyValue( "Sucursal", ref fSucursal, value ); }
    }

    [ Size( 10 ) ]
    public string CodigoPostal
    {
      get { return fCodigoPostal; }
      set { SetPropertyValue( "CodigoPostal", ref fCodigoPostal, value ); }
    }

    public bool NoALaOrden
    {
      get { return fNoALaOrden; }
      set { SetPropertyValue( "NoALaOrden", ref fNoALaOrden, value ); }
    }

    public bool Cruzado
    {
      get { return fCruzado; }
      set { SetPropertyValue( "Cruzado", ref fCruzado, value ); }
    }

    public Persona Orden
    {
      get { return fOrden; }
      set { SetPropertyValue( "Orden", ref fOrden, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    [ ModelDefault( "RowCount", "1" ) ]
    [ Appearance( "manual_pais", Criteria = "NOT ISNULL(Orden)", Visibility = ViewItemVisibility.Hide ) ]
    public string OrdenOtro
    {
      get { return fOrdenOtro; }
      set { SetPropertyValue( "OrdenOtro", ref fOrdenOtro, value ); }
    }

    [ ModelDefault( "AllowEdit", "false" ) ]
    public ChequeEstado Estado
    {
      get { return fEstado; }
      set { SetPropertyValue( "Estado", ref fEstado, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }

    public override void CalcularEstado( )
    {
      base.CalcularEstado( );

      if( UltimoMovimiento == null ) return;

      var cuentaClase = UltimoMovimiento.Cuenta.Clase;
      var debeHaber = UltimoMovimiento.DebeHaber;

      if( debeHaber == DebeHaber.Debe && cuentaClase == CuentaClase.Cartera ) //Si ENTRO en una cuenta NO BANCO y de clase CARTERA, esta EN CARTERA
        Estado = ChequeEstado.EnCartera;
      else if( debeHaber == DebeHaber.Haber && cuentaClase == CuentaClase.Banco )
      {
        if( FechaAcreditado == null )
          Estado = ChequeEstado.PropiosPendDebito; //Si SALIO de una cuenta BANCO, SIN FECHA DE ACRED, esta PEND PAGO
        else
          Estado = ChequeEstado.PropiosDebitados; //Si SALIO de una cuenta BANCO, CON FECHA DE ACRED, esta PAGADO
      }
      else if( debeHaber == DebeHaber.Debe && cuentaClase == CuentaClase.Banco )
      {
        if( FechaAcreditado == null )
        {
          if( Propio )
            Estado = ChequeEstado.PropiosEnTransito; //Si ENTRO de una cuenta BANCO, SIN FECHA DE ACRED, PROPIO, esta EN TRANSITO (ej. cheque propio, que me volvio por rechazo)
          else
            Estado = ChequeEstado.PendAcreditación; //Si ENTRO de una cuenta BANCO, SIN FECHA DE ACRED, NO PROPIO, esta PEND COBRO
        }
        else
          Estado = ChequeEstado.Acreditados; //Si ENTRO de una cuenta BANCO, CON FECHA DE ACRED, esta COBRADO
      }
      else
        Estado = ChequeEstado.Entregados; //Casos remanentes:	Si SALIO de una cuenta NO BANCO, esta ENTREGADO /SI ENTRO en una cuenta NO BANCO - NO CARTERA
    }
  }
}
