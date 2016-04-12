using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoIntervinienteItemHistorialController : HistorialController< PolizaIntervinienteItem, DocumentoIntervinienteItem >
  {
    public DocumentoIntervinienteItemHistorialController( )
      : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "PolizaItem = ?", this.GetMasterObject<DocumentoItem>( ).PolizaItem ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoIntervinienteItem movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta ) return;

      DocumentoIntervinienteItem vigente;
      if( ( vigente = movimiento.PolizaIntervinienteItem.IntervinienteItemVigente ) == null ) return;

      movimiento.Rol = vigente.Rol;
      movimiento.Principal = vigente.Principal;
      movimiento.Participacion = vigente.Participacion;
      movimiento.Interviniente = vigente.Interviniente;
      movimiento.Direccion = vigente.Direccion;
      movimiento.CategoriaImpuestos = vigente.CategoriaImpuestos;
      movimiento.Contacto = vigente.Contacto;
      movimiento.ComisionPrima = vigente.ComisionPrima;
      movimiento.ComisionCobranza = vigente.ComisionCobranza;
    }
  }
}
