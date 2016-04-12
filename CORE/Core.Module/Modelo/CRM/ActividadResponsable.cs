using DevExpress.Xpo;
using FDIT.Core.Seguridad;

namespace FDIT.Core.CRM
{
  [ Persistent( @"crm.ActividadResponsable" ) ]
  [ System.ComponentModel.DisplayName( "Responsables de la actividad" ) ]
  public class ActividadResponsable : BasicObject
  {
    private Actividad fActividad;
    private bool fPrincipal;
    private Usuario fResponsable;

    public ActividadResponsable( Session session ) : base( session )
    {
    }

    [ Association( @"ActividadesResponsablesReferencesActividades" ) ]
    public Actividad Actividad
    {
      get { return fActividad; }
      set { SetPropertyValue( "Actividad", ref fActividad, value ); }
    }

    public Usuario Responsable
    {
      get { return fResponsable; }
      set { SetPropertyValue( "Responsable", ref fResponsable, value ); }
    }

    public bool Principal
    {
      get { return fPrincipal; }
      set { SetPropertyValue( "Principal", ref fPrincipal, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
