using System;

namespace FDIT.Core.Web.Controllers.Personas
{
  /// <summary>
  ///   Controlador para la funcion de RELACIONAR una persona a otras, mediante un popup
  /// </summary>
  public class RelacionController : Core.Controllers.Personas.RelacionController
  {
    private readonly LookupControllerHelper fLookupControllerHelper;

    public RelacionController( )
    {
      fLookupControllerHelper = new LookupControllerHelper( this, nuevaRelacionAction );
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
