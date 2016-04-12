using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.ViewVariantsModule;
using FDIT.Core.Fondos;

namespace FDIT.Core.Controllers.Fondos
{
  public class ComprobanteItemDetailViewController : ViewController<DetailView>
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public ComprobanteItemDetailViewController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetObjectType = typeof( ComprobanteItem );
      TargetViewNesting = Nesting.Root;
    }

    /// <summary>
    ///   Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      mostrarVarianteParaClaseValor( );

      ObjectSpace.Refreshing += ObjectSpace_Refreshing;
      ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
    }

    private void ObjectSpace_Refreshing( object sender, EventArgs e )
    {
      mostrarVarianteParaClaseValor( );
    }

    private void ObjectSpace_ObjectChanged( object sender, ObjectChangedEventArgs e )
    {
      //Capturar el seteo de un valor en el campo Especie
      var ci = e.Object as ComprobanteItem;
      if( ci != null && e.PropertyName == "Especie" && ci.Especie != null )
      {
        mostrarVarianteParaClaseValor( );
      }
    }

    private void mostrarVarianteParaClaseValor( )
    {
      //Alterar la ListView de ComprobanteItemValor, para mostrar la variante específica de ESE TIPO DE VALOR
      //Esto se hace con un truco, aprovechando la funcionalidad VIEW VARIANTS de DX
      var comprobanteItem = ( ComprobanteItem ) View.CurrentObject;

      var listPropertyEditor = ( ListPropertyEditor ) ( ( DetailView ) View ).FindItem( "Valores" );

      if( listPropertyEditor.Frame == null )
        listPropertyEditor.CreateControl( );

      var valorType = comprobanteItem.Especie == null || String.IsNullOrEmpty( comprobanteItem.Especie.TipoValor )
                        ? typeof( Valor )
                        : XafTypesInfo.Instance.FindTypeInfo( comprobanteItem.Especie.TipoValor ).Type;

      var variantController = listPropertyEditor.Frame.GetController< ChangeVariantController >( );
      if( variantController == null ) return;
      foreach( var item in variantController.ChangeVariantAction.Items.Where( item => item.Id == valorType.FullName + "_Valores_ListView" ) )
      {
        var previousValue = variantController.ChangeVariantAction.Enabled[ "NotModified" ];

        variantController.ChangeVariantAction.Enabled[ "NotModified" ] = true;
        variantController.ChangeVariantAction.Active[ "Hidden by ComprobanteItemValorController" ] = true;
        variantController.ChangeVariantAction.DoExecute( item );
        variantController.ChangeVariantAction.Active[ "Hidden by ComprobanteItemValorController" ] = false;

        variantController.ChangeVariantAction.Enabled[ "NotModified" ] = previousValue;
        return;
      }
    }

    protected override void OnDeactivated( )
    {
      ObjectSpace.Refreshing -= ObjectSpace_Refreshing;
      ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;

      base.OnDeactivated( );
    }

    internal static void UpdateModel( XafApplication application )
    {
      var modelViews = ( IModelList< IModelView > ) application.Model.Application.Views;
      var modelViewVariants = ( ( IModelViewVariants ) modelViews[ "FDIT.Core.Fondos.ComprobanteItem_Valores_ListView" ] ).Variants;
      var rootModelViewVariants = ( ( ModelNode ) modelViewVariants );

      var documentoItemType = XafTypesInfo.Instance.PersistentTypes.First( info => info.Type == typeof( Valor ) );
      var variantId = documentoItemType.Type.FullName + "_Valores_ListView";
      var defaultVariant = rootModelViewVariants.AddNode< IModelVariant >( variantId );
      defaultVariant.View = modelViews[ variantId ];
      defaultVariant.Caption = variantId;
      modelViewVariants.Current = defaultVariant;

      foreach( var type in documentoItemType.Descendants.OrderBy( info => info.Name ) )
      {
        variantId = type.Type.FullName + "_Valores_ListView";
        var modelViewVariant = rootModelViewVariants.AddNode< IModelVariant >( variantId );
        modelViewVariant.View = modelViews[ variantId ];
        modelViewVariant.Caption = variantId;
      }
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new Container( );
    }

    #endregion
  }
}
