using System.ComponentModel;
using DevExpress.ExpressApp;

namespace FDIT.Core.Seguros.Controllers
{
  public class NuevoDocumentoDetailController : ViewController<DetailView>
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private IContainer components;

    public NuevoDocumentoDetailController( )
    {
      InitializeComponent( );
      RegisterActions( components );

      TargetViewType = ViewType.DetailView;
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

      this.View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
    }

    void ObjectSpace_ObjectChanged( object sender, ObjectChangedEventArgs e )
    {
    }

    protected override void OnDeactivated( )
    {
      this.View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;

      base.OnDeactivated( );
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container( );
    }

    #endregion
  }
}
