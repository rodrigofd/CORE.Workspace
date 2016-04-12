using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoItemDetalleHistorialController : HistorialController< PolizaItemDetalle, DocumentoItemDetalle >
  {
    public DocumentoItemDetalleHistorialController( )
      : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "PolizaItem = ?", this.GetMasterObject<DocumentoItem>( ).PolizaItem ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoItemDetalle movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta ) return;

      DocumentoItemDetalle vigente;
      if( ( vigente = movimiento.PolizaItemDetalle.DetalleVigente ) == null ) return;

      movimiento.TipoTitulo = vigente.TipoTitulo;
      movimiento.Titulo = vigente.Titulo;
      movimiento.Interes = vigente.Interes;
      movimiento.Detalle = vigente.Detalle;
      movimiento.Orden = vigente.Orden;
    }
  }
}
