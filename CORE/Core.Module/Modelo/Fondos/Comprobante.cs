using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Seguridad;
using FDIT.Core.Util;

namespace FDIT.Core.Fondos
{
  [ ImageName( "comp-fondos" ) ]
  [ Persistent( @"fondos.Comprobante" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Comprobante de fondos" ) ]
  [ RuleCriteria( "FDIT.Core.Fondos.Comprobante.MinimoUnItem", DefaultContexts.Save, "Items.Count() > 0", "Debe ingresar al menos un item." ) ]
  [ Appearance( "FDIT.Core.Fondos.Comprobante.EditableSiPendiente", "Estado <> 'Pendiente'", Enabled = false, TargetItems = "*" ) ]
  [ RuleCriteria( "FDIT.Core.Fondos.Comprobante.NoEliminarSiConfirmado", DefaultContexts.Delete, "Estado = 'Pendiente'", "No puede eliminar un comprobante ya confirmado o anulado." ) ]
  public class Comprobante : ComprobanteBase
  {
    private Comprobante fComprobanteReversion;
    private string fConcepto;

    private XPCollection< ComprobanteTipo > fTiposDisponibles;

    public Comprobante( Session session ) : base( session )
    {
    }

    protected override XPCollection< ComprobanteTipo > TiposDisponibles
    {
      get
      {
        fTiposDisponibles = base.TiposDisponibles;
        fTiposDisponibles.Criteria = CriteriaOperator.Parse( "Modulo = 'Fondos'" );
        return fTiposDisponibles;
      }
    }

    [ RuleRequiredField ]
    public override Talonario Talonario
    {
      get { return base.Talonario; }
      set { base.Talonario = value; }
    }

    [ Size( 200 ) ]
    [ ModelDefault( "RowCount", "1" ) ]
    [ RuleRequiredField ]
    public string Concepto
    {
      get { return fConcepto; }
      set { SetPropertyValue( "Concepto", ref fConcepto, value ); }
    }

    [ ModelDefault( "AllowEdit", "false" ) ]
    public Comprobante ComprobanteReversion
    {
      get { return fComprobanteReversion; }
      set { SetPropertyValue( "ComprobanteReversion", ref fComprobanteReversion, value ); }
    }

    [ Aggregated ]
    [ Association ]
    public XPCollection< ComprobanteItem > Items
    {
      get { return GetCollection< ComprobanteItem >( "Items" ); }
    }

    public override ComprobanteTipo Tipo
    {
      get { return base.Tipo; }
      set
      {
        base.Tipo = value;

        if( CanRaiseOnChanged )
          ProponerModelo( );
      }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Fecha = DateTime.Today;
      Originante = CoreAppLogonParameters.Instance.EmpresaActual( Session ).Persona;
    }

    public void ProponerModelo( )
    {
      var modelo = Session.FindObject< ComprobanteModelo >( CriteriaOperator.Parse( "Empresa = ? AND Tipo = ?", Empresa, Tipo ) );
      if( modelo == null ) return;

      Items.Empty( );

      foreach( var nuevoItem in modelo.Items.Select( modeloItem => new ComprobanteItem( Session )
                                                                   {
                                                                     Cuenta = modeloItem.Cuenta,
                                                                     DebeHaber = modeloItem.DebeHaber,
                                                                     Importe = 0,
                                                                     Cambio = 1,
                                                                     Especie = modeloItem.Especie
                                                                   } ) )
        Items.Add( nuevoItem );

      OnChanged( "Items" );
    }
  }
}
