//using System;
//using System.ComponentModel;
//using DevExpress.Persistent.Base;
//using DevExpress.Xpo;
//using FDIT.Core.Personas;
//using FDIT.Core.Sistema;

//namespace FDIT.Core.Stock
//{
//  [Persistent( @"stock.Almacen" )]
//  [DefaultClassOptions]
//  [DefaultProperty( "Nombre" )]
//  [System.ComponentModel.DisplayName( "Almacén" )]
//  public class Almacen : BasicObject, IObjetoPorEmpresa
//  {
//    private int fCalificacion;
//    private Direccion fDireccion;
//    private Empresa fEmpresa;
//    private DateTime fFechaBaja;
//    private bool fGestionFinanciera;
//    private string fNombre;
//    private string fNotas;

//    public Almacen( Session session ) : base( session )
//    {
//    }

//    public string Nombre
//    {
//      get { return fNombre; }
//      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
//    }

//    public Direccion Direccion
//    {
//      get { return fDireccion; }
//      set { SetPropertyValue( "Direccion", ref fDireccion, value ); }
//    }

//    public int Calificacion
//    {
//      get { return fCalificacion; }
//      set { SetPropertyValue< int >( "Calificacion", ref fCalificacion, value ); }
//    }

//    public bool GestionFinanciera
//    {
//      get { return fGestionFinanciera; }
//      set { SetPropertyValue( "GestionFinanciera", ref fGestionFinanciera, value ); }
//    }

//    [System.ComponentModel.DisplayName( "Fecha de baja" )]
//    public DateTime FechaBaja
//    {
//      get { return fFechaBaja; }
//      set { SetPropertyValue< DateTime >( "FechaBaja", ref fFechaBaja, value ); }
//    }

//    public string Notas
//    {
//      get { return fNotas; }
//      set { SetPropertyValue( "Notas", ref fNotas, value ); }
//    }

//    [Delayed]
//    [Association]
//    [System.ComponentModel.DisplayName( "Existencias" )]
//    public XPCollection< Existencia > Existencias
//    {
//      get { return GetCollection< Existencia >( @"Existencias" ); }
//    }

//    [Browsable( false )]
//    public Empresa Empresa
//    {
//      get { return fEmpresa; }
//      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
//    }

//    public override void AfterConstruction( )
//    {
//      base.AfterConstruction( );
//    }
//  }
//}