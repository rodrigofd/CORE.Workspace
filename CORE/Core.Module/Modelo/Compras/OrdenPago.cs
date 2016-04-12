using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;

namespace FDIT.Core.Compras
{
  [ImageName( "money_in_envelope" )]
  [Persistent( @"compras.OrdenPago" )]
  [ System.ComponentModel.DisplayName( "Orden de pago" ) ]
  public class OrdenPago : Pago
  {
    public OrdenPago( Session session )
      : base( session )
    {
      ActualizarOrigDest( );
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "DestinatariosDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Proveedor" ) ]
    [ RuleRequiredField ]
    public override Persona Destinatario
    {
      get { return base.Destinatario; }
      set { base.Destinatario = value; }
    }

    internal void ActualizarOrigDest( )
    {
      //filtrar los orig/dest posibles
      OriginantesDisponibles.Criteria = CriteriaOperator.Parse( "[<Empresa>][^.Oid = Persona.Oid]" );
      DestinatariosDisponibles.Criteria = CriteriaOperator.Parse( "[<Proveedor>][^.Oid = Persona.Oid]" );
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Tipo = Identificadores.GetInstance( Session ).TipoComprobanteOrdenPago;
    }
  }
}
