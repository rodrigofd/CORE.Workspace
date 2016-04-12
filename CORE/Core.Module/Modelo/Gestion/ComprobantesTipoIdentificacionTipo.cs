using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.ComprobantesTipoIdentificacionTipo" )]
  [System.ComponentModel.DisplayName( "Tipo de identificación por tipo de comprobante" )]
  public class ComprobantesTipoIdentificacionTipo : BasicObject
  {
    private ComprobanteTipo fComprobanteTipo;
    private IdentificacionTipo fIdentificacionTipo;

    public ComprobantesTipoIdentificacionTipo( Session session ) : base( session )
    {
    }

    [Association( @"ComprobantesTiposCategoriasReferencesIdentificacionesTipos" )]
    public ComprobanteTipo ComprobanteTipo
    {
      get { return fComprobanteTipo; }
      set { SetPropertyValue( "ComprobanteTipo", ref fComprobanteTipo, value ); }
    }

    public IdentificacionTipo IdentificacionTipo
    {
      get { return fIdentificacionTipo; }
      set { SetPropertyValue( "IdentificacionTipo", ref fIdentificacionTipo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}