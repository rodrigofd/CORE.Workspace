using System;
using System.IO;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;
using FDIT.Core.Modelo.Sistema;
using Comprobante = FDIT.Core.Fondos.Comprobante;

namespace FDIT.Core.Controllers.Fondos
{
  public class ComprobanteController : ComprobanteBaseController
  {
    protected SimpleAction reversarAction;

    public ComprobanteController( )
    {
      TargetObjectType = typeof( Comprobante );
    }

    public override string RutaExpComprobantes
    {
      get { return Identificadores.GetInstance( ObjectSpace ).RutaExpComprobantes; }
    }

    protected override void CreateActions( )
    {
      base.CreateActions( );

      ComprobanteExportReportAction.Id = this.GetType() + "ExportReportAction";
      ComprobanteConfirmarAction.Id = this.GetType() + "ConfirmarAction";
      ComprobanteDuplicarAction.Id = this.GetType() + "DuplicarAction";

      // 
      // reversarAction
      // 
      reversarAction = new SimpleAction( components );
      reversarAction.Caption = "Reversar";
      reversarAction.Id = this.GetType() + "ReversarComprobante";
      reversarAction.ConfirmationMessage = "Confirma?";
      reversarAction.ImageName = "money_delete";
      reversarAction.Shortcut = null;
      reversarAction.Tag = null;
      reversarAction.TargetObjectsCriteria = null;
      reversarAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
      reversarAction.TargetViewId = null;
      reversarAction.ToolTip = null;
      reversarAction.TypeOfView = null;
      reversarAction.Execute += reversarAction_Execute;
    }

    protected override void ConfirmarComprobanteValidar( ComprobanteBase comprobante )
    {
      var totalDebe = ( decimal ) ( comprobante.Evaluate( "Items.Sum(DebeAlCambio)" ) );
      var totalHaber = ( decimal ) ( comprobante.Evaluate( "Items.Sum(HaberAlCambio)" ) );

      if( totalDebe != totalHaber )
        throw new UserFriendlyException( "Los items del comprobante no balancean correctamente" );
    }

    protected override void ConfirmarComprobanteAfter( ComprobanteBase comprobante )
    {
      //Recalcular estados de valores
      var compFondos = ( ( Comprobante ) comprobante );
      foreach( var comprobanteItemValor in compFondos.Items.SelectMany( item => item.Valores ) )
        comprobanteItemValor.Valor.CalcularEstado( );
    }

    public override void DuplicarComprobante( ComprobanteBase comprobante )
    {
      base.DuplicarComprobante( comprobante );
      ( ( Comprobante ) comprobante ).ComprobanteReversion = null;
    }

    private void reversarAction_Execute( object sender, SimpleActionExecuteEventArgs e )
    {
      var selectedComprobantes = View.SelectedObjects;

      if( selectedComprobantes.Count == 0 )
        throw new UserFriendlyException( "Seleccione al menos un comprobante" );

      var mensajes = "";
      var cloner = new BasicObjectCloner( true );

      foreach( Comprobante comprobante in selectedComprobantes )
      {
        try
        {
          if( comprobante.Estado != ComprobanteEstado.Confirmado )
            throw new UserFriendlyException( "No puede reversar un comprobante no confirmado." );

          if( comprobante.ComprobanteReversion != null )
            throw new UserFriendlyException( "El comprobante ya se encuentra reversado." );

          comprobante.SetIgnoreOnChangedRecursive( true );
          var copy = ( Comprobante ) cloner.CloneTo( comprobante, typeof( Comprobante ) );

          copy.Estado = ComprobanteEstado.Pendiente;
          copy.Fecha = DateTime.Today;
          var patronLeyendaReversado = Identificadores.GetInstance( ObjectSpace ).PatronConceptoReversado;
          if( !string.IsNullOrEmpty( patronLeyendaReversado ) )
            copy.Concepto = string.Format( patronLeyendaReversado, copy.Concepto );

          copy.ComprobanteReversion = null;
          copy.Numero = 0;

          foreach( var item in copy.Items )
            item.DebeHaber = item.DebeHaber == DebeHaber.Debe ? DebeHaber.Haber : DebeHaber.Debe;

          copy.Save( );

          comprobante.ComprobanteReversion = copy;
          comprobante.Save( );

          ConfirmarComprobante( copy );
        }
        catch( Exception exc )
        {
          mensajes += "\n" + comprobante.Descripcion + ": " + exc.Message;
        }
      }

      ObjectSpace.CommitChanges( );

      if( mensajes != "" )
        throw new UserFriendlyException( "Errores en el reversado de uno o más comprobantes:" + mensajes );

      View.Refresh( );
    }

    //TODO: crear motor de expansion de variables por campos de un objeto
    protected override string ExpandFilename( object obj, string filename )
    {
      var comp = ( Comprobante ) obj;

      filename = filename.Replace( "{Tipo}", comp.Tipo.Descripcion );
      filename = filename.Replace( "{Tipo.Codigo}", comp.Tipo.Codigo );
      filename = filename.Replace( "{Sector}", comp.Sector.ToString( "0000" ) );
      filename = filename.Replace( "{Numero}", comp.Numero.ToString( "00000000" ) );
      filename = filename.Replace( "{Empresa}", comp.Empresa.Descripcion );
      filename = filename.Replace( "{Fecha:yyyy}", comp.Fecha.ToString( "yyyy" ) );
      filename = filename.Replace( "{Fecha:MM}", comp.Fecha.ToString( "MM" ) );
      filename = filename.Replace( "{Fecha:MMM}", comp.Fecha.ToString( "MMM" ) );
      filename = filename.Replace( "{Fecha:dd}", comp.Fecha.ToString( "dd" ) );
      filename = filename.Replace( "{Fecha}", comp.Fecha.ToString( "yyyy.MM.dd" ) );

      var ruta = Path.GetFileNameWithoutExtension( filename );
      while( ruta.EndsWith( "." ) )
        ruta = ruta.Substring( 0, ruta.Length - 1 );

      ruta = Path.GetInvalidPathChars( ).Aggregate( ruta, ( current, c ) => current.Replace( c, '_' ) );

      ruta = Path.GetDirectoryName( filename ) + @"\" + ruta + Path.GetExtension( filename );

      return ruta;
    }
  }
}
