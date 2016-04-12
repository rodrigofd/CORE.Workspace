using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
  [Persistent( @"personas.IdentificacionClase" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Clase de identificación" )]
  public class IdentificacionClase : BasicObject
  {
    private string fNombre;
    private string fCodigo;

    public IdentificacionClase( Session session ) : base( session )
    {
    }

    [Size( 10 )]
    [System.ComponentModel.DisplayName( "Código" )]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Size( 50 )]
    [Index(1)]
public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [Association( @"IdentificacionesTiposReferencesIdentificacionesClases", typeof( IdentificacionTipo ) )]
    public XPCollection< IdentificacionTipo > IdentificacionesTipos
    {
      get { return GetCollection<IdentificacionTipo>( "IdentificacionesTipos" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}