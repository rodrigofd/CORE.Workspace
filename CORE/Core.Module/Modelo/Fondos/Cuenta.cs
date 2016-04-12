using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.Cuenta" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Cuentas" ) ]
  public class Cuenta : BasicObject, IObjetoPorEmpresa
  {
    private Banco fBanco;
    private CuentaClase fClase;
    private string fCodigo;
    private bool fControlaChequera;
    private Contabilidad.Cuenta fCuentaContable;
    private bool fDisponibilidad;
    private bool fEmiteCheque;

    private Empresa fEmpresa;
    private Especie fEspeciePredeterminada;
    private bool fExigeApertura;
    private Moneda fMoneda;
    private string fNombre;
    private string fNotas;
    private string fNumeroCuenta;
    private int fOrden;
    private string fSucursal;

    public Cuenta( Session session ) : base( session )
    {
    }

    public CuentaClase Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    [ Size( 20 ) ]
    [ Index( 0 ) ]
    [VisibleInLookupListView(true)]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Index( 1 )]
    [VisibleInLookupListView( true )]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Association( @"CuentasReferencesBancos" ) ]
    public Banco Banco
    {
      get { return fBanco; }
      set { SetPropertyValue( "Banco", ref fBanco, value ); }
    }

    [ Size( 10 ) ]
    public string Sucursal
    {
      get { return fSucursal; }
      set { SetPropertyValue( "Sucursal", ref fSucursal, value ); }
    }

    [ Size( 20 ) ]
    public string NumeroCuenta
    {
      get { return fNumeroCuenta; }
      set { SetPropertyValue( "NumeroCuenta", ref fNumeroCuenta, value ); }
    }

    public Moneda Moneda
    {
      get { return fMoneda; }
      set { SetPropertyValue( "Moneda", ref fMoneda, value ); }
    }

    public bool EmiteCheque
    {
      get { return fEmiteCheque; }
      set { SetPropertyValue( "EmiteCheque", ref fEmiteCheque, value ); }
    }

    public bool ExigeApertura
    {
      get { return fExigeApertura; }
      set { SetPropertyValue( "ExigeApertura", ref fExigeApertura, value ); }
    }

    public bool ControlaChequera
    {
      get { return fControlaChequera; }
      set { SetPropertyValue( "ControlaChequera", ref fControlaChequera, value ); }
    }

    public int Orden
    {
      get { return fOrden; }
      set { SetPropertyValue< int >( "Orden", ref fOrden, value ); }
    }

    public Especie EspeciePredeterminada
    {
      get { return fEspeciePredeterminada; }
      set { SetPropertyValue( "EspeciePredeterminada", ref fEspeciePredeterminada, value ); }
    }

    public bool Disponibilidad
    {
      get { return fDisponibilidad; }
      set { SetPropertyValue( "Disponibilidad", ref fDisponibilidad, value ); }
    }

    public Contabilidad.Cuenta CuentaContable
    {
      get { return fCuentaContable; }
      set { SetPropertyValue( "CuentaContable", ref fCuentaContable, value ); }
    }

    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [ Aggregated ]
    [ Association( @"CuentasChequerasReferencesCuentas", typeof( CuentaChequera ) ) ]
    public XPCollection< CuentaChequera > Chequeras
    {
      get { return GetCollection< CuentaChequera >( "Chequeras" ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Clase = CuentaClase.General;
    }
  }
}
