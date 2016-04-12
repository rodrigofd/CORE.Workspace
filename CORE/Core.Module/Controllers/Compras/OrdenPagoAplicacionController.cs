using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using FDIT.Core.Compras;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;

namespace FDIT.Core.Controllers.Compras
{
  public class OrdenPagoAplicacionController : PagoAplicacionController
  {
    protected override void CreateActions( )
    {
      base.CreateActions( );

      PagoAplicarCuotaAction.Id = "OrdenPagoAplicarCuotaAction";
    }

    protected override void SetupCollectionSource( CollectionSource collectionSource )
    {
      collectionSource.Criteria.Add( "CuotasDelProveedor", CriteriaOperator.Parse( "Comprobante.Originante = ?", this.GetMasterObject< Pago >( ).Destinatario ) );
      collectionSource.Criteria.Add( "ComprobantesCompra", CriteriaOperator.Parse( "[<FDIT.Core.Compras.Comprobante>][^.Comprobante.Oid = Oid]" ) );
    }

    protected override void SetupShowViewParameters( SimpleActionExecuteEventArgs e, CollectionSource collectionSource )
    {
      e.ShowViewParameters.CreatedView = Application.CreateListView( "FDIT.Core.Gestion.ComprobanteCuota_LookupListView", collectionSource, false );
    }

    protected override void OnViewChanging( View view )
    {
      base.OnViewChanging( view );

      Active[ "EsPagoTipo" ] = view is ListView && this.GetMasterType( ( ListView ) view ) == typeof( OrdenPago );
    }
  }
}
