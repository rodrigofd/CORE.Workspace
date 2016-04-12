#region

using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;
using FDIT.Core.Stock;

#endregion

namespace FDIT.Core.Compras
{
  [ ImageName( "truck" ) ]
  [ Persistent( @"compras.Proveedor" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Proveedor" ) ]
  public class Proveedor : Rol
  {
    private bool fActivo;
    private ComprobanteTipo fComprobanteTipoPredeterminado;
    private Concepto fConceptoPredeterminado;
    private CondicionDePago fCondicionDePagoPredeterminada;
    private ListaDePrecios fListaPredeterminada;

    public Proveedor( Session session )
      : base( session )
    {
    }

    [ System.ComponentModel.DisplayName( "Lista de precios predeterminada" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public ListaDePrecios ListaPredeterminada
    {
      get { return fListaPredeterminada; }
      set { SetPropertyValue( "ListaPredeterminada", ref fListaPredeterminada, value ); }
    }

    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Compra'" ) ]
    [ System.ComponentModel.DisplayName( "Tipo de comprobante predeterminado" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public ComprobanteTipo ComprobanteTipoPredeterminado
    {
      get { return fComprobanteTipoPredeterminado; }
      set { SetPropertyValue( "ComprobanteTipo", ref fComprobanteTipoPredeterminado, value ); }
    }

    [ System.ComponentModel.DisplayName( "Condición de pago predeterminada" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public CondicionDePago CondicionDePagoPredeterminada
    {
      get { return fCondicionDePagoPredeterminada; }
      set { SetPropertyValue( "CondicionDePago", ref fCondicionDePagoPredeterminada, value ); }
    }

    [ System.ComponentModel.DisplayName( "Concepto de fact. predeterminado" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public Concepto ConceptoPredeterminado
    {
      get { return fConceptoPredeterminado; }
      set { SetPropertyValue( "Concepto", ref fConceptoPredeterminado, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    [ Aggregated ]
    [ Association( @"ProveedoresEmpresasReferencesProveedores", typeof( ProveedorEmpresa ) ) ]
    public XPCollection< ProveedorEmpresa > ProveedorPorEmpresas
    {
      get { return GetCollection< ProveedorEmpresa >( "ProveedorPorEmpresas" ); }
    }

    [ Association( @"ArticuloProveedorReferencesProveedores" ) ]
    public XPCollection< ArticuloProveedor > ArticulosPorProveedor
    {
      get { return GetCollection< ArticuloProveedor >( "ArticulosPorProveedor" ); }
    }
    
    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
