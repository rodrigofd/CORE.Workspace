#region

using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Native;
using FDIT.Core.Util;

#endregion

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.Identificadores" ) ]
  [ System.ComponentModel.DisplayName( "Preferencias de Fondos" ) ]
  public class Identificadores : IdentificadoresBase< Identificadores >
  {
    private Especie fEspeciePredeterminada;
    private Moneda fMonedaPredeterminada;
    private int fVencimientoChequeDias;
    private string fRutaExpComprobantes;
    private string fPatronConceptoReversado;

    public Identificadores( Session session )
      : base( session )
    {
    }

    [LookupEditorMode(LookupEditorMode.AllItems)]
    public Moneda MonedaPredeterminada
    {
      get { return fMonedaPredeterminada; }
      set { SetPropertyValue( "MonedaPredeterminada", ref fMonedaPredeterminada, value ); }
    }

    [LookupEditorMode(LookupEditorMode.AllItems)]
    public Especie EspeciePredeterminada
    {
      get { return fEspeciePredeterminada; }
      set { SetPropertyValue( "EspeciePredeterminada", ref fEspeciePredeterminada, value ); }
    }

    public int VencimientoChequeDias
    {
      get { return fVencimientoChequeDias; }
      set { SetPropertyValue< int >( "VencimientoChequeDias", ref fVencimientoChequeDias, value ); }
    }

    public string PatronConceptoReversado
    {
      get { return fPatronConceptoReversado; }
      set { SetPropertyValue( "PatronConceptoReversado", ref fPatronConceptoReversado, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [DisplayName( "Ruta para exportación de comprobantes" )]
    public string RutaExpComprobantes
    {
      get { return fRutaExpComprobantes; }
      set { SetPropertyValue( "RutaExpComprobantes", ref fRutaExpComprobantes, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
