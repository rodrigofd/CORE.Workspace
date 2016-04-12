using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Reports;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using FDIT.Core.Compras;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;
using Comprobante = FDIT.Core.Compras.Comprobante;

namespace FDIT.Core.Controllers.Compras
{
  public class ComprobanteController : ComprobanteBaseController
  {
    public ComprobanteController( ) : base( )
    {
      TargetObjectType = typeof( Comprobante );
    }

    public override string RutaExpComprobantes
    {
      get { return Identificadores.GetInstance( ObjectSpace ).RutaExpComprobantes; }
    }
    
    protected override void OnActivated( )
    {
      base.OnActivated( );
    }

    protected override void OnDeactivated( )
    {
      base.OnDeactivated( );
    }

    public override void DuplicarComprobante( ComprobanteBase comprobante )
    {
      var comprobanteCompras = ( Comprobante ) comprobante;

      comprobanteCompras.AutorizadaCodigo = null;
      comprobanteCompras.AutorizadaCodigoFecVto = null;
      comprobanteCompras.AutorizadaFecha = null;
      comprobanteCompras.AutorizadaNotas = null;
      comprobanteCompras.AutorizadaResultado = null;

      comprobanteCompras.Fecha = ( DateTime ) ( comprobanteCompras.Vencimiento = DateTime.Today );

      comprobanteCompras.GenerarCuotas( );
    }
  }
}
