using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;
using FDIT.Core.Ventas;

namespace FDIT.Core.Controllers.Ventas
{
  public class ReciboAplicacionController : PagoAplicacionController
  {
    protected override void CreateActions( )
    {
      base.CreateActions( );

      PagoAplicarCuotaAction.Id = "ReciboAplicarCuotaAction";
    }

    protected override void SetupCollectionSource( CollectionSource collectionSource )
    {
      collectionSource.Criteria.Add( "CuotasDelCliente", CriteriaOperator.Parse( "Comprobante.Destinatario = ?", this.GetMasterObject<Pago>( ).Destinatario ) );
      collectionSource.Criteria.Add( "ComprobantesVenta", CriteriaOperator.Parse( "[<FDIT.Core.Ventas.Comprobante>][^.Comprobante.Oid = Oid]" ) );
    }

    protected override void SetupShowViewParameters( SimpleActionExecuteEventArgs e, CollectionSource collectionSource )
    {
      e.ShowViewParameters.CreatedView = Application.CreateListView( "FDIT.Core.Gestion.ComprobanteCuota_LookupListView", collectionSource, false );
    }

    protected override void OnViewChanging( View view )
    {
      base.OnViewChanging( view );

      Active[ "EsPagoTipo" ] = view is ListView && this.GetMasterType( ( ListView ) view ) == typeof( Recibo );
    }
  }
}