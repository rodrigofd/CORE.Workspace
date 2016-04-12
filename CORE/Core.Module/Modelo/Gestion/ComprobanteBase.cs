using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Controllers;
using FDIT.Core.Impuestos;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;
using FDIT.Core.Sistema;

namespace FDIT.Core.Gestion
{
  /// <summary>
  ///   Representa la base para cualquier comprobante de una operación de la empresa, que tiene un originante, un tipo, sector y numero secuencial
  /// </summary>
  [ Persistent("gestion.ComprobanteBase") ]
  [ DefaultProperty( "Descripcion" ) ]
  [System.ComponentModel.DisplayName( "Comprobante" )]
  public class ComprobanteBase : BasicObject, IObjetoPorEmpresa
  {
    protected string fAnuladaNotas;
    protected ComprobanteSector fComprobanteSector;
    protected Persona fDestinatario;
    protected XPCollection< Persona > fDestinatariosDisponibles;
    protected Empresa fEmpresa;
    protected ComprobanteEstado fEstado;
    protected DateTime fFecha;
    protected string fNotasSuperior;
    protected string fNotasInferior;
    protected int fNumero;
    protected Persona fOriginante;
    protected XPCollection< Persona > fOriginantesDisponibles;
    protected bool fPropio;
    protected int fSector;
    protected Talonario fTalonario;
    protected ComprobanteTipo fTipo;
    private XPCollection< ComprobanteTipo > fTiposDisponibles;
    protected DateTime? fVencimiento;

    public ComprobanteBase( Session session )
      : base( session )
    {
    }

    [ VisibleInDetailView( false ) ]
    [ PersistentAlias( "CONCAT(IIF(NOT ISNULL(Tipo),Tipo.Codigo,''),'-',PADLEFT(TOSTR(Sector),4,'0'),'-',PADLEFT(TOSTR(Numero),8,'0'))" ) ]
    [ System.ComponentModel.DisplayName( "Descripción" ) ]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [ModelDefault( "AllowEdit", "false" )]
    public ComprobanteEstado Estado
    {
      get { return fEstado; }
      set { SetPropertyValue( "Estado", ref fEstado, value ); }
    }

    /// <summary>
    ///   Indica si el comprobante se relaciona directamente con la empresa del sistema, o bien es de un tercero, pero se
    ///   ingresa por referencia
    ///   También determina (si es True) que se autogeneren cuotas, al indicar la cantidad de cuotas, que se calculen impuestos
    ///   en ventas, etc.
    /// </summary>
    [ System.ComponentModel.DisplayName( "Es propio" ) ]
    [ RuleRequiredField ]
    public bool Propio
    {
      get { return fPropio; }
      set { SetPropertyValue( "Propio", ref fPropio, value ); }
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "TiposDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Tipo" ) ]
    [ RuleRequiredField ]
    //[ ResetsProperty( "Talonario" ) ]
    public virtual ComprobanteTipo Tipo
    {
      get { return fTipo; }
      set
      {
        SetPropertyValue( "Tipo", ref fTipo, value );

        if( CanRaiseOnChanged )
        {
          if( value != null && value.Talonarios.Count == 1 )
            Talonario = value.Talonarios[0];
        }
      }
    }

    [ ImmediatePostData ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ DataSourceProperty( "Tipo.Talonarios" ) ]
    public virtual Talonario Talonario
    {
      get { return fTalonario; }
      set
      {
        SetPropertyValue( "Talonario", ref fTalonario, value );
        
        if( this.CanRaiseOnChanged )
        {
          if( value != null )
            Sector = value.Sector;
        }
      }
    }

    [ System.ComponentModel.DisplayName( "Sector" ) ]
    [ ModelDefault( "DisplayFormat", "{0:0000}" ) ]
    [ RuleRequiredField ]
    public virtual int Sector
    {
      get { return fSector; }
      set { SetPropertyValue< int >( "Sector", ref fSector, value ); }
    }

    [ System.ComponentModel.DisplayName( "Número" ) ]
    [ ModelDefault( "DisplayFormat", "{0:00000000}" ) ]
    [ RuleRequiredField ]
    public virtual int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue< int >( "Numero", ref fNumero, value ); }
    }

    [ImmediatePostData]
    [ System.ComponentModel.DisplayName( "Fecha" ) ]
    [ RuleRequiredField ]
    public virtual DateTime Fecha
    {
      get { return fFecha; }
      set { SetPropertyValue< DateTime >( "Fecha", ref fFecha, value ); }
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "OriginantesDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Originante" ) ]
    [ RuleRequiredField ]
    public virtual Persona Originante
    {
      get { return fOriginante; }
      set { SetPropertyValue( "Originante", ref fOriginante, value ); }
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "DestinatariosDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Destinatario" ) ]
    public virtual Persona Destinatario
    {
      get { return fDestinatario; }
      set { SetPropertyValue( "Destinatario", ref fDestinatario, value ); }
    }

    [ System.ComponentModel.DisplayName( "Notas (1)" ) ]
    [ Size( SizeAttribute.Unlimited ) ]
    public string NotasSuperior
    {
      get { return fNotasSuperior; }
      set { SetPropertyValue( "NotasSuperior", ref fNotasSuperior, value ); }
    }

    [ System.ComponentModel.DisplayName( "Notas (2)" ) ]
    [ Size( SizeAttribute.Unlimited ) ]
    public string NotasInferior
    {
      get { return fNotasInferior; }
      set { SetPropertyValue( "NotasInferior", ref fNotasInferior, value ); }
    }

    public string AnuladaNotas
    {
      get { return fAnuladaNotas; }
      set { SetPropertyValue( "AnuladaNotas", ref fAnuladaNotas, value ); }
    }

    [ Browsable( false ) ]
    protected virtual XPCollection< ComprobanteTipo > TiposDisponibles
    {
      get { return fTiposDisponibles ?? ( fTiposDisponibles = new XPCollection< ComprobanteTipo >( Session ) ); }
    }

    [ Browsable( false ) ]
    protected virtual XPCollection< Persona > OriginantesDisponibles
    {
      get { return fOriginantesDisponibles ?? ( fOriginantesDisponibles = new XPCollection< Persona >( Session ) ); }
    }

    [ Browsable( false ) ]
    protected virtual XPCollection< Persona > DestinatariosDisponibles
    {
      get { return fDestinatariosDisponibles ?? ( fDestinatariosDisponibles = new XPCollection< Persona >( Session ) ); }
    }

    //[Browsable( false )]
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public Categoria OriginanteCategoriaIva
    {
      get { return Originante == null ? null : ( from imp in Originante.DatosImpositivos where imp.Categoria.Impuesto == Impuestos.Impuestos.IVA select imp.Categoria ).FirstOrDefault( ); }
    }

    //[Browsable( false )]
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public DateTime? OriginanteInicioActIva
    {
      get { return Originante == null ? ( DateTime? ) null : ( from imp in Originante.DatosImpositivos where imp.Categoria.Impuesto == Impuestos.Impuestos.IVA select imp.Desde ).FirstOrDefault( ); }
    }

    //[Browsable( false )]
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public Categoria OriginanteCategoriaIIBB
    {
      get { return Originante == null ? null : ( from imp in Originante.DatosImpositivos where imp.Categoria.Impuesto == Impuestos.Impuestos.IngresosBrutosProvBuenosAires select imp.Categoria ).FirstOrDefault( ); }
    }

    //[Browsable( false )]
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public Identificacion OriginanteIdentificacionIva
    {
      get { return Originante == null ? null : ( from imp in Originante.Identificaciones where imp.Tipo.Oid == IdentificacionTipo.CUIT select imp ).FirstOrDefault( ); }
    }

    //[Browsable( false )]
    [ VisibleInListView( false ) ]
    [ VisibleInDetailView( false ) ]
    [ NonPersistent ]
    public Identificacion OriginanteIdentificacionIIBB
    {
      get { return Originante == null ? null : ( from imp in Originante.Identificaciones where imp.Tipo.Oid == IdentificacionTipo.NUMERO_IIBB select imp ).FirstOrDefault( ); }
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

      Estado = ComprobanteEstado.Pendiente;

      Propio = true;
      Empresa = CoreAppLogonParameters.Instance.EmpresaActual( Session );
    }

    public void NumerarAutomatico( )
    {
      if( Talonario == null )
        throw new UserFriendlyException( "No se encuentra un talonario configurado para esta empresa, tipo y sector de comprobante" );

      var proxNumero = Talonario.UltimoNumero + 1;
      
      var compExiste =
        Session.FindObject( this.ClassInfo, CriteriaOperator.Parse( "Talonario.Oid = ? AND Numero = ? AND Oid <> ?", Talonario.Oid, proxNumero, Oid ) );

      if( compExiste != null )
        throw new UserFriendlyException( "El próximo número que indicó el talonario, ya se encuentra utilizado por otro comprobante. Por favor revise los valores del talonario" );

      Numero = proxNumero;
    }
  }
}
