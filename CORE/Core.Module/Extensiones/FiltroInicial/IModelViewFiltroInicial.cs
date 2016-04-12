using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.Persistent.Base;

namespace FDIT.Core.FiltroInicial
{
  public enum TipoFiltro
  {
    SinFiltro = 0,
    Automatico = 1,
    Parametros = 2,
  }

  public interface IModelViewFiltroInicial : IModelNode
  {
    TipoFiltro TipoFiltro{ get; set; }

    [ DataSourceProperty( ModelViewsDomainLogic.DataSourcePropertyPath ) ]
    [ Category( "Data" ) ]
    IModelDetailView VistaParaParametros{ get; set; }

    string TituloVistaParametros{ get; set; }
  }
}