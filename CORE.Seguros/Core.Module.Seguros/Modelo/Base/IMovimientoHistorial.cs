using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;

namespace FDIT.Core.Seguros
{
    /// <summary>
    ///     Define un objeto que constituye un movimiento, que forma parte de un 'historial' de cierto objeto
    /// </summary>
    /// <typeparam name="TObjetoConHistorial">El tipo del objeto maestro, del cual se define un historial</typeparam>
    /// <typeparam name="TMovimientoHistorial"></typeparam>
    public interface IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial> : ISessionProvider
        where TObjetoConHistorial : IObjetoConHistorial<TMovimientoHistorial>
        where TMovimientoHistorial : IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial>
    {
        /// <summary>
        ///     Propiedad que determina la dirección del movimiento (Alta,Baja,Modificacion)
        /// </summary>
        [Index(0)]
        TipoMovimiento TipoMovimiento { get; set; }

        bool IsDeleted { get; }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        XPCollection<TObjetoConHistorial> PadreObjetoConHistorial { get; }

        /// <summary>
        ///     Propiedad referencia al objeto del cual se define un historial
        /// </summary>
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        TObjetoConHistorial ObjetoConHistorial { get; set; }

        void Delete();
    }
}