using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Contabilidad
{
  [ Persistent( "contabilidad.CuentaRubro" ) ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Rubro de cuenta contable" ) ]
  [ DefaultClassOptions ]
  public class CuentaRubro : BasicObject
  {
    private bool fActiva;
    private CuentaClase fClase;
    private string fCodigo;
    private Empresa fEmpresa;
    private string fNombre;

    public CuentaRubro( Session session ) : base( session )
    {
    }

    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ VisibleInLookupListView( false ) ]
    [ PersistentAlias( "ISNULL(Codigo, '') + ' - ' + Nombre" ) ]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [ Size( 50 ) ]
    [ Index( 0 ) ]
    [ VisibleInLookupListView( true ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    [ VisibleInListView( true ) ]
    [ VisibleInLookupListView( true ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Descripcion", ref fNombre, value ); }
    }

    public CuentaClase Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    public bool Activa
    {
      get { return fActiva; }
      set { SetPropertyValue( "Activa", ref fActiva, value ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }
  }
}
