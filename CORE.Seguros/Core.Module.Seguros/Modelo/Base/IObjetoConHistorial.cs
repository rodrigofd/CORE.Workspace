using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    public interface IObjetoConHistorial<TMovimientoHistorial>
    {
        XPCollection<TMovimientoHistorial> Historial { get; }

        TMovimientoHistorial Vigente { get; set; }

        void Delete();
    }
}