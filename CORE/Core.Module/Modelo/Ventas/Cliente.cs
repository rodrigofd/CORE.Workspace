using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.CRM;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;
using FDIT.Core.Stock;

namespace FDIT.Core.Ventas
{
  [ ImageName( "account-balances" ) ]
  [ Persistent( @"ventas.Cliente" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Cliente" ) ]
  [ Appearance( "tratamiento_tratamiento", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.Tratamiento" ) ]
  [ Appearance( "tratamiento_apellidos_paterno", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.ApellidosPaterno" ) ]
  [ Appearance( "tratamiento_apellidos_materno", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.ApellidosMaterno" ) ]
  [ Appearance( "tratamiento_nombres", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.NombrePila" ) ]
  [ Appearance( "tratamiento_segundo_nombre", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.SegundoNombre" ) ]
  [ Appearance( "personeria_nombre", Criteria = "Persona.Tipo <> 'Juridica'", Enabled = false, TargetItems = "Persona.Nombre" ) ]
  [ Appearance( "tratamiento_nombre_fantasia", Criteria = "Persona.Tipo <> 'Juridica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.NombreFantasia" ) ]
  [ Appearance( "personeria_sexo", Criteria = "Persona.Tipo <> 'Fisica'", Visibility = ViewItemVisibility.Hide, TargetItems = "Persona.Sexo" ) ]
  public class Cliente : Rol
  {
    private bool fActivo;
    private AgrupacionCliente fAgrupacionCliente;
    private ComprobanteTipo fComprobanteTipoPredeterminado;
    private CondicionDePago fCondicionDePagoPredeterminada;
    private decimal fDescuento;
    private PersonaDireccion fDireccionEntrega;
    private Identificacion fEmailEnvioFacturacion;
    private ListaDePrecios fListaPredeterminada;
    private string fPatronEmailEntrante;

    public Cliente( Session session ) : base( session )
    {
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "Persona.Nombre" ) ]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    public ListaDePrecios ListaPredeterminada
    {
      get { return fListaPredeterminada; }
      set { SetPropertyValue( "ListaPredeterminada", ref fListaPredeterminada, value ); }
    }

    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Venta'" ) ]
    public ComprobanteTipo ComprobanteTipoPredeterminado
    {
      get { return fComprobanteTipoPredeterminado; }
      set { SetPropertyValue( "ComprobanteTipoPredeterminado", ref fComprobanteTipoPredeterminado, value ); }
    }

    [ System.ComponentModel.DisplayName( "Condición de pago predeterminada" ) ]
    public CondicionDePago CondicionDePagoPredeterminada
    {
      get { return fCondicionDePagoPredeterminada; }
      set { SetPropertyValue( "CondicionDePago", ref fCondicionDePagoPredeterminada, value ); }
    }

    [ DataSourceProperty( "Persona.Direcciones" ) ]
    public PersonaDireccion DireccionEntrega
    {
      get { return fDireccionEntrega; }
      set { SetPropertyValue( "DireccionEntrega", ref fDireccionEntrega, value ); }
    }

    [ ModelDefault( "DisplayFormat", "{0:n2}" ) ]
    [ ModelDefault( "EditMask", "n2" ) ]
    public decimal Descuento
    {
      get { return fDescuento; }
      set { SetPropertyValue< decimal >( "Descuento", ref fDescuento, value ); }
    }

    [ Association( @"ClientesReferencesAgrupacionesClientes" ) ]
    public AgrupacionCliente AgrupacionCliente
    {
      get { return fAgrupacionCliente; }
      set { SetPropertyValue( "AgrupacionCliente", ref fAgrupacionCliente, value ); }
    }

    public string PatronEmailEntrante
    {
      get { return fPatronEmailEntrante; }
      set { SetPropertyValue( "PatronEmailEntrante", ref fPatronEmailEntrante, value ); }
    }

    [ DataSourceProperty( "Persona.Identificaciones" ) ]
    public Identificacion EmailEnvioFacturacion
    {
      get { return fEmailEnvioFacturacion; }
      set { SetPropertyValue( "EmailEnvioFacturacion", ref fEmailEnvioFacturacion, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    [ System.ComponentModel.DisplayName( "Contratos" ) ]
    [ Aggregated ]
    [ Association ]
    public XPCollection< Contrato > Contratos
    {
      get { return GetCollection< Contrato >( "Contratos" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }

    protected override void OnSaving( )
    {
      base.OnSaving( );
    }
  }
}
