using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Impuestos;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;
using FDIT.Core.Util;

namespace FDIT.Core.Ventas
{
  /// <summary>
  ///   Representa un comprobante de una operación de venta (destinada a un cliente)
  /// </summary>
  [ ImageName( "document-invoice" ) ]
  [ Persistent( @"ventas.Comprobante" ) ]
  [ MapInheritance( MapInheritanceType.ParentTable ) ]
  [ DefaultProperty( "Descripcion" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Comprobante de venta" ) ]
  public class Comprobante : Gestion.Comprobante
  {
    public Comprobante( Session session ) : base( session )
    {
      ActualizarOrigDest( );
    }

    //TODO: parametrizar esto de filtrar tipos de comp por modulo
    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Venta'" ) ]
    public override ComprobanteTipo Tipo
    {
      get { return base.Tipo; }
      set { base.Tipo = value; }
    }

    [ RuleRequiredField ]
    public override Talonario Talonario
    {
      get { return base.Talonario; }
      set { base.Talonario = value; }
    }

    [ ImmediatePostData ]
    [ System.ComponentModel.DisplayName( "Fecha" ) ]
    [ RuleRequiredField ]
    public override DateTime Fecha
    {
      get { return base.Fecha; }
      set
      {
        base.Fecha = value;

        if( CanRaiseOnChanged )
          Vencimiento = value;
      }
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "DestinatariosDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Cliente" ) ]
    [ RuleRequiredField ]
    public override Persona Destinatario
    {
      get { return base.Destinatario; }
      set
      {
        base.Destinatario = value;

        if( CanRaiseOnChanged )
        {
          //intentar bajar los valores predeterminados, si el destinatario es un cliente
          if( Destinatario != null )
          {
            Domicilio = null;
            IdentificacionNro = null;
            IdentificacionTipo = null;

            var cliente = Session.FindObject< Cliente >( new BinaryOperator( "Persona.Oid", Destinatario.Oid ) );
            if( cliente != null )
            {
              Tipo = cliente.ComprobanteTipoPredeterminado;
              CondicionDePago = cliente.CondicionDePagoPredeterminada;
              Lista = cliente.ListaPredeterminada;
              Descuento = cliente.Descuento;
            }

            CopiarDatosPersona( Destinatario );
          }
        }
      }
    }

    internal void ActualizarOrigDest( )
    {
      //filtrar los orig/dest posibles
      OriginantesDisponibles.Criteria = CriteriaOperator.Parse( "[<Empresa>][^.Oid = Persona.Oid]" );
      DestinatariosDisponibles.Criteria = CriteriaOperator.Parse( "[<Cliente>][^.Oid = Persona.Oid]" );
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Originante = CoreAppLogonParameters.Instance.EmpresaActual( Session ).Persona;
      Destinatario = null;
    }

    public override void OnItemsChanged( ComprobanteItem item )
    {
      if( CanRaiseOnChanged )
      {
        //Al actualizar items, recalcular impuestos (para comprobantes propios)
        if( Propio && !item.Generado )
          AplicarImpuestos( );

        base.OnItemsChanged( item );
      }
    }

    //TODO: refactorear a un motor en impuestos
    /// <summary>
    ///   Motor de calculo de impuestos de venta
    /// </summary>
    public void AplicarImpuestos( )
    {
      IgnoreOnChanged = true;
      Items.Where( item => item.Generado ).Empty( );
      IgnoreOnChanged = false;

      foreach( var impuesto in new XPCollection< Impuesto >( Session, true ) )
      {
        if( impuesto == Impuestos.Impuestos.IVA )
        {
          var dic = new Dictionary< int, ComprobanteItem >( );
          foreach( var item in Items.Where( item => item.Concepto != null && item.Concepto.AlicuotaImpuesto != null ) )
          {
            if( !dic.ContainsKey( item.Concepto.AlicuotaImpuesto.Oid ) )
            {
              dic[ item.Concepto.AlicuotaImpuesto.Oid ] = new ComprobanteItem( Session )
                                                          {
                                                            Generado = true,
                                                            Concepto = Session.FindObject< Concepto >( new BinaryOperator( "AlicuotaImpuestoAsociada.Oid", item.Concepto.AlicuotaImpuesto.Oid ) ),
                                                            Cantidad = 1,
                                                            Alicuota = item.Concepto.AlicuotaImpuesto.Valor
                                                          };
            }
            dic[ item.Concepto.AlicuotaImpuesto.Oid ].BaseImponible += item.ImporteTotal;
          }

          foreach( var itm in dic )
          {
            itm.Value.PrecioUnitario = Math.Round( itm.Value.BaseImponible * ( itm.Value.Alicuota / 100 ), 2 );
            Items.Add( itm.Value );
          }
        }
        else if( impuesto == Impuestos.Impuestos.IngresosBrutosProvBuenosAires )
        {
          var datosImp = CoreAppLogonParameters.Instance.EmpresaActual( Session ).Persona.DatosImpositivos.FirstOrDefault( di => di.Impuesto == Impuestos.Impuestos.IngresosBrutosProvBuenosAires );
          if( datosImp != null && datosImp.AgentePercepcion )
          {
            var cuitCliente = 0L;
            if( !long.TryParse( this.IdentificacionNro, out cuitCliente ) ) continue;

            var padron = Session.FindObject<Padron>( CriteriaOperator.Parse( "Oid = ?", cuitCliente ) );
            if( padron != null )
            {
              var tasa = padron.AlicuotaPercepcion;
              var baseImponible = Items.Where( item => item.Concepto != null && item.Concepto.Clase == ConceptoClase.General ).Sum( item => item.ImporteTotal );
              var importe = baseImponible * tasa / 100;

              Items.Add( new ComprobanteItem( Session )
                         {
                           Generado = true,
                           Concepto = Identificadores.GetInstance( Session ).PercepcionIIBBBAConcepto,
                           Cantidad = 1,
                           PrecioUnitario = importe
                         } );
            }
          }
        }
      }
    }
  }
}
