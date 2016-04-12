using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using FDIT.Core.Controllers;
using FDIT.Core.Modelo.Sistema;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoItemHistorialController : HistorialController< PolizaItem, DocumentoItem >
  {
    public DocumentoItemHistorialController( ) : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "Poliza = ?", this.GetMasterObject< Documento >( ).Poliza ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoItem movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta )
      {
        movimiento.Ubicacion = objectSpace.CreateObject< Direccion >( );
      }
      else
      {
        DocumentoItem vigente;
        if( ( vigente = movimiento.PolizaItem.ItemVigente ) == null ) return;

        movimiento.NumeroAseguradora = vigente.NumeroAseguradora;
        movimiento.MateriaAsegurada = vigente.MateriaAsegurada;
        movimiento.Plan = vigente.Plan;

        //Clonamos el objeto Direccion asociado; para que pueda hacer modificaciones sobre uno nuevo, en lugar de afectar el anterior
        var cloner = new BasicObjectCloner( true );
        movimiento.Ubicacion = ( Direccion ) cloner.CloneTo( vigente.Ubicacion, typeof( Direccion ) );
      }
    }
  }
}
