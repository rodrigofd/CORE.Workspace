using System.ComponentModel;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.CRM;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;

namespace FDIT.Core.EWS
{
  [ ImageName( "wrench-screwdriver" ) ]
  [ Persistent( @"ews.Identificadores" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Preferencias de Exchange" ) ]
  public class Identificadores : XPLiteObject
  {
    private ActividadEstado fActividadEstadoEmailPendiente;
    private ActividadEstado fActividadEstadoEmailCompletado;
    private ActividadTipo fActividadTipoEmail;
    private Cuenta fCuentaSincContactos;
    private Cuenta fCuentaSincEmails;
    private DireccionTipo fDireccionTipoLaboral;
    private DireccionTipo fDireccionTipoOtra;
    private DireccionTipo fDireccionTipoParticular;
    private int fEmpresa;
    private IdentificacionTipo fIdentificacionTipoEmail;
    private IdentificacionTipo fIdentificacionTipoFaxOtro;
    private IdentificacionTipo fIdentificacionTipoFaxParticular;
    private IdentificacionTipo fIdentificacionTipoTTY;
    private IdentificacionTipo fIdentificacionTipoTelAsistente;
    private IdentificacionTipo fIdentificacionTipoTelAuto;
    private IdentificacionTipo fIdentificacionTipoTelDevLlamada;
    private IdentificacionTipo fIdentificacionTipoTelFaxTrabajo;
    private IdentificacionTipo fIdentificacionTipoTelISDN;
    private IdentificacionTipo fIdentificacionTipoTelMovil;
    private IdentificacionTipo fIdentificacionTipoTelOtro;
    private IdentificacionTipo fIdentificacionTipoTelPager;
    private IdentificacionTipo fIdentificacionTipoTelParticular;
    private IdentificacionTipo fIdentificacionTipoTelPpalCia;
    private IdentificacionTipo fIdentificacionTipoTelPrimario;
    private IdentificacionTipo fIdentificacionTipoTelRadio;
    private IdentificacionTipo fIdentificacionTipoTelTrabajo;
    private IdentificacionTipo fIdentificacionTipoTelex;
    private IdentificacionTipo fIdentificacionTipoIM;
    private IdentificacionTipo fIdentificacionTipoWebTrabajo;
    private Propiedad fPropiedadPuesto;
    private Propiedad fPropiedadProfesion;
    private Propiedad fPropiedadEsposo;
    private Propiedad fPropiedadDepartamento;
    private Propiedad fPropiedadAsistente;

    public Identificadores( Session session ) : base( session )
    {
    }

    [ Key ]
    [ Browsable( false ) ]
    public int Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue< int >( "Empresa", ref fEmpresa, value ); }
    }

    [ System.ComponentModel.DisplayName( "Cuenta para sincronizar contactos" ) ]
    public Cuenta CuentaSincContactos
    {
      get { return fCuentaSincContactos; }
      set { SetPropertyValue( "CuentaSincContactos", ref fCuentaSincContactos, value ); }
    }

    [ System.ComponentModel.DisplayName( "Cuenta para sincronizar emails" ) ]
    public Cuenta CuentaSincEmails
    {
      get { return fCuentaSincEmails; }
      set { SetPropertyValue( "CuentaSincEmails", ref fCuentaSincEmails, value ); }
    }

    public ActividadTipo ActividadTipoEmail
    {
      get { return fActividadTipoEmail; }
      set { SetPropertyValue( "ActividadTipoEmail", ref fActividadTipoEmail, value ); }
    }

    public ActividadEstado ActividadEstadoEmailPendiente
    {
      get { return fActividadEstadoEmailPendiente; }
      set { SetPropertyValue( "ActividadEstadoEmailPendiente", ref fActividadEstadoEmailPendiente, value ); }
    }

    public ActividadEstado ActividadEstadoEmailCompletado
    {
      get { return fActividadEstadoEmailCompletado; }
      set { SetPropertyValue( "ActividadEstadoEmailCompletado", ref fActividadEstadoEmailCompletado, value ); }
    }

    public DireccionTipo DireccionTipoParticular
    {
      get { return fDireccionTipoParticular; }
      set { SetPropertyValue( "DireccionTipoParticular", ref fDireccionTipoParticular, value ); }
    }

    public DireccionTipo DireccionTipoLaboral
    {
      get { return fDireccionTipoLaboral; }
      set { SetPropertyValue( "DireccionTipoLaboral", ref fDireccionTipoLaboral, value ); }
    }

    public DireccionTipo DireccionTipoOtra
    {
      get { return fDireccionTipoOtra; }
      set { SetPropertyValue( "DireccionTipoOtra", ref fDireccionTipoOtra, value ); }
    }

    public IdentificacionTipo IdentificacionTipoEmail
    {
      get { return fIdentificacionTipoEmail; }
      set { SetPropertyValue( "IdentificacionTipoEmail", ref fIdentificacionTipoEmail, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelAsistente
    {
      get { return fIdentificacionTipoTelAsistente; }
      set { SetPropertyValue( "IdentificacionTipoTelAsistente", ref fIdentificacionTipoTelAsistente, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelFaxTrabajo
    {
      get { return fIdentificacionTipoTelFaxTrabajo; }
      set { SetPropertyValue( "IdentificacionTipoTelFaxTrabajo", ref fIdentificacionTipoTelFaxTrabajo, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelTrabajo
    {
      get { return fIdentificacionTipoTelTrabajo; }
      set { SetPropertyValue( "IdentificacionTipoTelTrabajo", ref fIdentificacionTipoTelTrabajo, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelDevLlamada
    {
      get { return fIdentificacionTipoTelDevLlamada; }
      set { SetPropertyValue( "IdentificacionTipoTelDevLlamada", ref fIdentificacionTipoTelDevLlamada, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelAuto
    {
      get { return fIdentificacionTipoTelAuto; }
      set { SetPropertyValue( "IdentificacionTipoTelAuto", ref fIdentificacionTipoTelAuto, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelPpalCia
    {
      get { return fIdentificacionTipoTelPpalCia; }
      set { SetPropertyValue( "IdentificacionTipoTelPpalCia", ref fIdentificacionTipoTelPpalCia, value ); }
    }

    public IdentificacionTipo IdentificacionTipoFaxParticular
    {
      get { return fIdentificacionTipoFaxParticular; }
      set { SetPropertyValue( "IdentificacionTipoFaxParticular", ref fIdentificacionTipoFaxParticular, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelParticular
    {
      get { return fIdentificacionTipoTelParticular; }
      set { SetPropertyValue( "IdentificacionTipoTelParticular ", ref fIdentificacionTipoTelParticular, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelISDN
    {
      get { return fIdentificacionTipoTelISDN; }
      set { SetPropertyValue( "IdentificacionTipoTelISDN ", ref fIdentificacionTipoTelISDN, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelMovil
    {
      get { return fIdentificacionTipoTelMovil; }
      set { SetPropertyValue( "IdentificacionTipoTelMovil", ref fIdentificacionTipoTelMovil, value ); }
    }

    public IdentificacionTipo IdentificacionTipoFaxOtro
    {
      get { return fIdentificacionTipoFaxOtro; }
      set { SetPropertyValue( "IdentificacionTipoFaxOtro", ref fIdentificacionTipoFaxOtro, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelOtro
    {
      get { return fIdentificacionTipoTelOtro; }
      set { SetPropertyValue( "IdentificacionTipoTelOtro", ref fIdentificacionTipoTelOtro, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelPager
    {
      get { return fIdentificacionTipoTelPager; }
      set { SetPropertyValue( "IdentificacionTipoTelPager", ref fIdentificacionTipoTelPager, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelPrimario
    {
      get { return fIdentificacionTipoTelPrimario; }
      set { SetPropertyValue( "IdentificacionTipoTelPrimario", ref fIdentificacionTipoTelPrimario, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelRadio
    {
      get { return fIdentificacionTipoTelRadio; }
      set { SetPropertyValue( "IdentificacionTipoTelRadio", ref fIdentificacionTipoTelRadio, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTelex
    {
      get { return fIdentificacionTipoTelex; }
      set { SetPropertyValue( "IdentificacionTipoTelex", ref fIdentificacionTipoTelex, value ); }
    }

    public IdentificacionTipo IdentificacionTipoTTY
    {
      get { return fIdentificacionTipoTTY; }
      set { SetPropertyValue( "IdentificacionTipoTTY", ref fIdentificacionTipoTTY, value ); }
    }

    public IdentificacionTipo IdentificacionTipoWebTrabajo
    {
      get { return fIdentificacionTipoWebTrabajo; }
      set { SetPropertyValue( "IdentificacionTipoWebTrabajo", ref fIdentificacionTipoWebTrabajo, value ); }
    }

    public Propiedad PropiedadPuesto
    {
      get { return fPropiedadPuesto; }
      set { SetPropertyValue( "PropiedadPuesto", ref fPropiedadPuesto, value ); }
    }

    public Propiedad PropiedadDepartamento
    {
      get { return fPropiedadDepartamento; }
      set { SetPropertyValue( "PropiedadDepartamento", ref fPropiedadDepartamento, value ); }
    }

    public Propiedad PropiedadAsistente
    {
      get { return fPropiedadAsistente; }
      set { SetPropertyValue( "PropiedadAsistente", ref fPropiedadAsistente, value ); }
    }

    public Propiedad PropiedadProfesion
    {
      get { return fPropiedadProfesion; }
      set { SetPropertyValue( "PropiedadProfesion", ref fPropiedadProfesion, value ); }
    }

    public Propiedad PropiedadEsposo
    {
      get { return fPropiedadEsposo; }
      set { SetPropertyValue( "PropiedadEsposo", ref fPropiedadEsposo, value ); }
    }
    
    public IdentificacionTipo IdentificacionTipoIM
    {
      get { return fIdentificacionTipoIM; }
      set { SetPropertyValue( "IdentificacionTipoIM", ref fIdentificacionTipoIM, value ); }
    }

    public static Identificadores GetInstance( IObjectSpace objectSpace )
    {
      return GetInstance( ( ( XPObjectSpace ) objectSpace ).Session );
    }

    public static Identificadores GetInstance( Session session )
    {
      return GetInstance( session, CoreAppLogonParameters.Instance.EmpresaActualId );
    }

    public static Identificadores GetInstance( Session session, int idEmpresa )
    {
      var col = session.GetObjectByKey< Identificadores >( idEmpresa );
      if( col != null )
        return col;
      col = new Identificadores( session ) { Empresa = idEmpresa };
      col.Save( );

      return col;
    }
  }
}
