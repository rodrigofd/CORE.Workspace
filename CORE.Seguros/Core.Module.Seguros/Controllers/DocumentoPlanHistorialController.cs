using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoPlanHistorialController : HistorialController< PolizaPlan, DocumentoPlan >
  {
    public DocumentoPlanHistorialController( )
      : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "Poliza = ?", this.GetMasterObject< Documento >( ).Poliza ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoPlan movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta ) return;

      DocumentoPlan vigente;
      if( ( vigente = movimiento.PolizaPlan.PlanVigente ) == null ) return;

      movimiento.Codigo = vigente.Codigo;
      movimiento.Nombre = vigente.Nombre;
      movimiento.PlanCobertura = vigente.PlanCobertura;
      movimiento.Orden = vigente.Orden;
    }
  }
}
