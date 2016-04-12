using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Depósito" ) ]
  [ Persistent( @"stock.Deposito" ) ]
  public class Deposito : BasicObject
  {
    private string fCodigo;
    private Direccion fDireccion;
    private bool fLlevaStock;
    private string fNombre;
    private int fCalificacion;
    private bool fGestionFinanciera;
    private string fNota;
    private DateTime fFechaBaja;

    public Deposito( Session session ) : base( session )
    {
    }

    [ Size( 10 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 50 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    public bool LlevaStock
    {
      get { return fLlevaStock; }
      set { SetPropertyValue( "LlevaStock", ref fLlevaStock, value ); }
    }

    public Direccion Direccion
    {
      get { return fDireccion; }
      set { SetPropertyValue( "Direccion", ref fDireccion, value ); }
    }

    public int Calificacion
    {
      get { return fCalificacion; }
      set { SetPropertyValue<int>( "Calificacion", ref fCalificacion, value ); }
    }

    public bool GestionFinanciera
    {
      get { return fGestionFinanciera; }
      set { SetPropertyValue( "GestionFinanciera", ref fGestionFinanciera, value ); }
    }

    [System.ComponentModel.DisplayName( "Fecha de baja" )]
    public DateTime FechaBaja
    {
      get { return fFechaBaja; }
      set { SetPropertyValue<DateTime>( "FechaBaja", ref fFechaBaja, value ); }
    }

    public string Nota
    {
      get { return fNota; }
      set { SetPropertyValue( "Nota", ref fNota, value ); }
    }

    [ Aggregated ]
    [ Association( @"Depositos_UbicacionesReferencesDepositos", typeof( DepositoUbicacion ) ) ]
    public XPCollection< DepositoUbicacion > Ubicaciones
    {
      get { return GetCollection< DepositoUbicacion >( "Ubicaciones" ); }
    }
  }
}
