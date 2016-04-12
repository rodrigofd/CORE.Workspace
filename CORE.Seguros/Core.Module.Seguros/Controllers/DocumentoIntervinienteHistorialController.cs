using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
  public class DocumentoIntervinienteHistorialController : HistorialController< PolizaInterviniente, DocumentoInterviniente >
  {
    public DocumentoIntervinienteHistorialController( )
      : base( )
    {
    }

    protected override CriteriaOperator PopupCriteria
    {
      get { return CriteriaOperator.Parse( "Poliza = ?", this.GetMasterObject< Documento >( ).Poliza ); }
    }

    protected override void InicializarValores( IObjectSpace objectSpace, DocumentoInterviniente movimiento )
    {
      if( movimiento.TipoMovimiento == TipoMovimiento.Alta ) return;

      DocumentoInterviniente vigente;
      if( ( vigente = movimiento.PolizaInterviniente.IntervinienteVigente ) == null ) return;

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
