using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Sistema;

namespace FDIT.Core.Fondos
{
  [ Persistent( "fondos.ComprobanteModelo" ) ]
  [ DefaultClassOptions ]
  [DefaultProperty("Tipo")]
    public class ComprobanteModelo : BasicObject, IObjetoPorEmpresa
  {
    private Empresa fEmpresa;
    private ComprobanteTipo fTipo;
    private ComprobanteModeloItem fItemPrincipal;

    public ComprobanteModelo( Session session ) : base( session )
    {
    }

    //TODO: parametrizar esto de filtrar tipos de comp por modulo
    [DataSourceCriteria( "Modulo = 'Fondos'" )]
    [LookupEditorMode(LookupEditorMode.AllItems)]
    public ComprobanteTipo Tipo
    {
      get { return fTipo; }
      set { SetPropertyValue( "Tipo", ref fTipo, value ); }
    }

    [DataSourceProperty("Items")]
    public ComprobanteModeloItem ItemPrincipal
    {
      get { return fItemPrincipal; }
      set { SetPropertyValue( "ItemPrincipal", ref fItemPrincipal, value ); }
    }

    [ Association ]
    [ Aggregated ]
    public XPCollection< ComprobanteModeloItem > Items
    {
      get { return GetCollection< ComprobanteModeloItem >( "Items" ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }
  }
}
