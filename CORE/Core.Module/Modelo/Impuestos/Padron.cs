using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Impuestos
{
  [Persistent( @"impuestos.Padron" )]
  [DefaultClassOptions]
  [DefaultProperty( "IdentificacionNro" )]
  [System.ComponentModel.DisplayName( "Padrón por impuesto" )]
  public class Padron : XPLiteObject
  {
    private decimal fAlicuotaPercepcion;
    private decimal fAlicuotaRetencion;
    private DateTime fFechaPublicacion;
    private DateTime fFechaVigDesde;
    private DateTime fFechaVigHasta;
    private int fGrupoPercepcion;
    private int fGrupoRetencion;
    private Impuesto fImpuesto;
    private long fOid;
    private long fIdentificacionNro;
    private char fMarcaCbioAlicuota;
    private char fMarcaSujeto;
    private string fTipoContribuyente;

    public Padron( Session session ) : base( session )
    {
    }
    
    [Key(true)]
    public long Oid
    {
      get { return fOid; }
      set { SetPropertyValue( "Oid", ref fOid, value ); }
    }

    [Association( @"PadronReferencesImpuestos" )]
    public Impuesto Impuesto
    {
      get { return fImpuesto; }
      set { SetPropertyValue( "Impuesto", ref fImpuesto, value ); }
    }

    public long IdentificacionNro
    {
      get { return fIdentificacionNro; }
      set { SetPropertyValue( "IdentificacionNro", ref fIdentificacionNro, value ); }
    }

    public DateTime FechaPublicacion
    {
      get { return fFechaPublicacion; }
      set { SetPropertyValue< DateTime >( "FechaPublicacion", ref fFechaPublicacion, value ); }
    }

    public DateTime FechaVigDesde
    {
      get { return fFechaVigDesde; }
      set { SetPropertyValue< DateTime >( "FechaVigDesde", ref fFechaVigDesde, value ); }
    }

    public DateTime FechaVigHasta
    {
      get { return fFechaVigHasta; }
      set { SetPropertyValue< DateTime >( "FechaVigHasta", ref fFechaVigHasta, value ); }
    }

    [Size(1)]
    public string TipoContribuyente
    {
      get { return fTipoContribuyente; }
      set { SetPropertyValue( "TipoContribuyente", ref fTipoContribuyente, value ); }
    }

    public char MarcaSujeto
    {
      get { return fMarcaSujeto; }
      set { SetPropertyValue( "MarcaSujeto", ref fMarcaSujeto, value ); }
    }

    public char MarcaCbioAlicuota
    {
      get { return fMarcaCbioAlicuota; }
      set { SetPropertyValue( "MarcaCbioAlicuota", ref fMarcaCbioAlicuota, value ); }
    }

    public decimal AlicuotaPercepcion
    {
      get { return fAlicuotaPercepcion; }
      set { SetPropertyValue< decimal >( "AlicuotaPercepcion", ref fAlicuotaPercepcion, value ); }
    }

    public decimal AlicuotaRetencion
    {
      get { return fAlicuotaRetencion; }
      set { SetPropertyValue< decimal >( "AlicuotaRetencion", ref fAlicuotaRetencion, value ); }
    }

    public int GrupoPercepcion
    {
      get { return fGrupoPercepcion; }
      set { SetPropertyValue< int >( "GrupoPercepcion", ref fGrupoPercepcion, value ); }
    }

    public int GrupoRetencion
    {
      get { return fGrupoRetencion; }
      set { SetPropertyValue< int >( "GrupoRetencion", ref fGrupoRetencion, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}