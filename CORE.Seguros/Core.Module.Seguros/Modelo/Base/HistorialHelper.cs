using System;
using System.Linq;
using DevExpress.ExpressApp;

namespace FDIT.Core.Seguros
{
    public static class HistorialHelper
    {
        public static void IniciarMovimientoHistorial<TMovimientoHistorial, TObjetoConHistorial>(
            this IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial> t)
            where TObjetoConHistorial : IObjetoConHistorial<TMovimientoHistorial>
            where TMovimientoHistorial : IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial>
        {
            var thisObj = (TMovimientoHistorial) t;
            if (!thisObj.IsDeleted && thisObj.TipoMovimiento == TipoMovimiento.Alta &&
                thisObj.ObjetoConHistorial == null)
            {
                //Crear un nuevo item a nivel de poliza, y agregar este plan de doc. como el inicial en su historia
                var nuevoObjetoHistorial =
                    (TObjetoConHistorial) Activator.CreateInstance(typeof (TObjetoConHistorial), thisObj.Session);

                nuevoObjetoHistorial.Historial.Add(thisObj);
                nuevoObjetoHistorial.Vigente = thisObj;
                thisObj.PadreObjetoConHistorial.Add(nuevoObjetoHistorial);
            }
        }

        public static void OnSavingMovimientoHistorial<TMovimientoHistorial, TObjetoConHistorial>(
            this IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial> t)
            where TObjetoConHistorial : IObjetoConHistorial<TMovimientoHistorial>
            where TMovimientoHistorial : IMovimientoHistorial<TObjetoConHistorial, TMovimientoHistorial>
        {
            var thisObj = (TMovimientoHistorial) t;
            if (thisObj.TipoMovimiento == TipoMovimiento.Alta)
            {
                //Si es tipo de movimiento ALTA, y ya esta asociado a un plan de poliza, asegurarse de que es el unico ALTA de dicho plan
                if (thisObj.ObjetoConHistorial != null)
                {
                    var cantMovAlta =
                        thisObj.ObjetoConHistorial.Historial.Count(mov => mov.TipoMovimiento == TipoMovimiento.Alta);
                    if (cantMovAlta > 1)
                    {
                        throw new UserFriendlyException("No puede marcar como Alta a un plan ya ingresado en la póliza.");
                    }
                    if (cantMovAlta == 0)
                    {
                        throw new UserFriendlyException("El primer movimiento del plan debe ser de Alta");
                    }

                    if (thisObj.IsDeleted)
                        thisObj.ObjetoConHistorial.Delete();
                }
                else
                    thisObj.IniciarMovimientoHistorial();
            }
        }
    }
}