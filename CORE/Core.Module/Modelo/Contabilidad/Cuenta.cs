using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Contabilidad
{
  [ Persistent( @"contabilidad.Cuenta" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Cuenta contable" ) ]
  public class Cuenta : BasicObject, IObjetoPorEmpresa, ITreeNode
  {
    private bool fActiva;
    private string fCodigo;
    private Cuenta fCuentaPadre;
    private CuentaRubro fCuentaRubro;
    private Empresa fEmpresa;
    private string fNombre;

    public Cuenta( Session session ) : base( session )
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
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Association ]
    public Cuenta CuentaPadre
    {
      get { return fCuentaPadre; }
      set { SetPropertyValue( "CuentaPadre", ref fCuentaPadre, value ); }
    }

    [ Association ]
    public XPCollection< Cuenta > CuentasHijas
    {
      get { return GetCollection< Cuenta >( "CuentasHijas" ); }
    }

    public CuentaRubro CuentaRubro
    {
      get { return fCuentaRubro; }
      set { SetPropertyValue( "CuentaRubro", ref fCuentaRubro, value ); }
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

    public string Name
    {
      get { return Descripcion; }
    }

    [Browsable(false)]
    public ITreeNode Parent
    {
      get { return CuentaPadre; }
    }

    [Browsable( false )]
    public IBindingList Children
    {
      get { return CuentasHijas; }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
