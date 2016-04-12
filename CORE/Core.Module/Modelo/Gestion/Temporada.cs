using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.Temporada" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Temporada" )]
  public class Temporada : BasicObject
  {
    private DateTime fFechaDesde;
    private DateTime fFechaHasta;
    private string fNombre;

    public Temporada( Session session )
      : base( session )
    {
    }
    
    [Size( 50 )]
    [System.ComponentModel.DisplayName( "Nombre" )]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    public DateTime FechaDesde
    {
      get { return fFechaDesde; }
      set { SetPropertyValue< DateTime >( "FechaDesde", ref fFechaDesde, value ); }
    }

    public DateTime FechaHasta
    {
      get { return fFechaHasta; }
      set { SetPropertyValue< DateTime >( "FechaHasta", ref fFechaHasta, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}