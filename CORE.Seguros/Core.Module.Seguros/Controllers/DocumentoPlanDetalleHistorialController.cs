using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoPlanDetalleHistorialController : HistorialController< PolizaPlanDetalle, DocumentoPlanDetalle >
  {
    public DocumentoPlanDetalleHistorialController( )
      : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "PolizaPlan = ?", this.GetMasterObject<DocumentoPlan>( ).PolizaPlan ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoPlanDetalle movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta ) return;

      DocumentoPlanDetalle vigente;
      if( ( vigente = movimiento.PolizaPlanDetalle.DetalleVigente ) == null ) return;

      movimiento.TipoTitulo = vigente.TipoTitulo;
      movimiento.Titulo = vigente.Titulo;
      movimiento.Interes = vigente.Interes;
      movimiento.Detalle = vigente.Detalle;
      movimiento.Orden = vigente.Orden;
    }
  }
}
