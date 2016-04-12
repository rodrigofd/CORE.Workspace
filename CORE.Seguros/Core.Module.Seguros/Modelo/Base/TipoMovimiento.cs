using DevExpress.Persistent.Base;

namespace FDIT.Core.Seguros
{
    public enum TipoMovimiento
    {
        [ImageName("ui-tooltip--plus")] Alta = 1,
        [ImageName("ui-tooltip--minus")] Baja = 2,
        [ImageName("ui-tooltip--pencil")] Modificacion = 3,
    }
}