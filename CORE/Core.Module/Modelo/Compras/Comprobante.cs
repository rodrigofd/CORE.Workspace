using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;

namespace FDIT.Core.Compras
{
  /// <summary>
  ///   Representa un comprobante de una operación de venta (destinada a un cliente)
  /// </summary>
  [ ImageName( "document-invoice-cmp" ) ]
  [ Persistent( @"compras.Comprobante" ) ]
  [ MapInheritance( MapInheritanceType.ParentTable ) ]
  [ DefaultProperty( "Descripcion" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Comprobante de compra" ) ]
  public class Comprobante : Gestion.Comprobante
  {
    public Comprobante( Session session ) : base( session )
    {
      ActualizarOrigDest( );
    }

    //TODO: parametrizar esto de filtrar tipos de comp por modulo
    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Compra'" ) ]
    public override ComprobanteTipo Tipo
    {
      get { return base.Tipo; }
      set { base.Tipo = value; }
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "OriginantesDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Proveedor" ) ]
    [ RuleRequiredField ]
    public override Persona Originante
    {
      get { return base.Originante; }
      set
      {
        base.Originante = value;

        if( CanRaiseOnChanged )
        {
          //intentar bajar los valores predeterminados, si el destinatario es un cliente
          if( Originante != null )
          {
            Domicilio = null;
            IdentificacionNro = null;
            IdentificacionTipo = null;

            var proveedor = Session.FindObject< Proveedor >( new BinaryOperator( "Persona.Oid", Originante.Oid ) );
            if( proveedor != null )
            {
              Tipo = proveedor.ComprobanteTipoPredeterminado;
              CondicionDePago = proveedor.CondicionDePagoPredeterminada;
              Lista = proveedor.ListaPredeterminada;

              var proveedorEmpresa = proveedor.ProveedorPorEmpresas.FirstOrDefault( pe => pe.Empresa.Oid == Empresa.Oid );
              if( proveedorEmpresa != null )
                Cuenta = proveedorEmpresa.CuentaFondosPredeterminada;
            }

            CopiarDatosPersona( Originante );
          }
        }
      }
    }

    internal void ActualizarOrigDest( )
    {
      //filtrar los orig/dest posibles
      OriginantesDisponibles.Criteria = CriteriaOperator.Parse( "[<Proveedor>][^.Oid = Persona.Oid]" );
      DestinatariosDisponibles.Criteria = CriteriaOperator.Parse( "[<Empresa>][^.Oid = Persona.Oid]" );
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Originante = null;
      Destinatario = CoreAppLogonParameters.Instance.EmpresaActual( Session ).Persona;
      Fecha = DateTime.Today;
    }
  }
}
