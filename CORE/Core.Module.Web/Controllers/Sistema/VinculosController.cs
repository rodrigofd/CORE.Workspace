using System;
using FDIT.Core.Web.Controllers.Personas;

namespace FDIT.Core.Web.Controllers
{
  /// <summary>
  ///   Controlador para la funcion de RELACIONAR una persona a otras, mediante un popup
  /// </summary>
  public class VinculosController : Core.Controllers.VinculosController
  {
    private readonly LookupControllerHelper fLookupControllerHelper;

    public VinculosController( )
    {
      fLookupControllerHelper = new LookupControllerHelper( this, nuevoVinculoAction );
    }

    protected override void dialogController_WindowTemplateChanged( object sender, EventArgs e )
    {
      fLookupControllerHelper.dc_WindowTemplateChanged( sender, e );
    }

    protected override void OnActivated( )
    {
      base.OnActivated(  );

      fLookupControllerHelper.OnActivated( );
    }

    protected override void OnDeactivated( )
    {
      fLookupControllerHelper.OnDeactivated( );

      base.OnDeactivated( );
    }

    protected override void UpdateActionState( )
    {
      base.UpdateActionState( );

      fLookupControllerHelper.UpdateActionState(  );
    }
  }
}