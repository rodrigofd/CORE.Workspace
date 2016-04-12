using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Personas;
using FDIT.Core.Regionales;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.Banco" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ FiltroPorPais( true ) ]
  [ System.ComponentModel.DisplayName( "Banco" ) ]
  public class Banco : Rol
  {
    private string fCodigo;
    private int fOrden;

    public Banco( Session session ) : base( session )
    {
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Persona.Nombre" ) ]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [ Size( 10 ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    public int Orden
    {
      get { return fOrden; }
      set { SetPropertyValue< int >( "Orden", ref fOrden, value ); }
    }

    [Association(@"CuentasReferencesBancos", typeof(Cuenta))]
    public XPCollection< Cuenta > CuentasBancarias
    {
      get { return GetCollection< Cuenta >( "CuentasBancarias" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
