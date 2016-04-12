using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.FileAttachments.Win;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using FDIT.Core.Controllers;
using ListView = DevExpress.ExpressApp.ListView;

namespace FDIT.Core.Win.Controllers
{
  /// <summary>
  ///   <para>
  ///     A <see cref="T:DevExpress.ExpressApp.ViewController" /> that allows users to create a new object with the selected
  ///     file attachment via the <b>AddFromFile</b> Action and by dragging and dropping a file into the List Editor's
  ///     control.
  ///   </para>
  /// </summary>
  public class ArchivosAdjuntosListViewController : FileAttachmentControllerBase
  {
    private SimpleAction addFromFileAction;
    private IContainer components;

    /// <summary>
    ///   <para>
    ///     Initializes a new instance of the FileAttachmentListViewController class.
    ///   </para>
    /// </summary>
    public ArchivosAdjuntosListViewController( )
    {
      InitializeComponent( );
      RegisterActions( components );
      TargetViewType = ViewType.ListView;
      TargetObjectType = typeof( ArchivoAdjunto );
    }

    /// <summary>
    ///   <para>
    ///     Provides access to the FileAttachmentListViewController's <b>AddFromFile</b> Action.
    ///   </para>
    /// </summary>
    /// <value>
    ///   A <see cref="T:DevExpress.ExpressApp.Actions.SimpleAction" /> object that is the <b>AddFromFile</b> Action.
    /// </value>
    public SimpleAction AddFromFileAction
    {
      get { return addFromFileAction; }
    }

    protected override void OnFrameAssigned( )
    {
      base.OnFrameAssigned( );

      var attachCtrl = Frame.GetController< FileAttachmentListViewController >( );
      if( attachCtrl != null )
        attachCtrl.Active[ "reemplazado" ] = false;
    }

    private void grid_DragDrop( object sender, DragEventArgs e )
    {
      if( e.Effect != DragDropEffects.Copy || !e.Data.GetDataPresent( DataFormats.FileDrop ) )
        return;
      AddFiles( ( string[ ] ) e.Data.GetData( DataFormats.FileDrop ) );
    }

    private void grid_DragOver( object sender, DragEventArgs e )
    {
      if( addFromFileAction.Active && e.Data.GetDataPresent( DataFormats.FileDrop ) )
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    protected string[ ] FilterFileNames( string[ ] fileNames )
    {
      string fileTypesFilter = GetFileTypesFilter( );
      if( string.IsNullOrEmpty( fileTypesFilter ) || Regex.IsMatch( fileTypesFilter, "\\*\\.\\*" ) )
        return fileNames;
      MatchCollection matchCollection = Regex.Matches( fileTypesFilter.ToLower( ), "((\\*\\..[^\\),\\|].)|(\\*\\.\\*))" );
      var list = new List< string >( );
      foreach( var path in fileNames )
      {
        string str = Path.GetExtension( path ).ToLower( );
        foreach( Capture capture in matchCollection )
        {
          if( capture.Value.EndsWith( str ) )
          {
            list.Add( path );
            break;
          }
        }
      }
      return list.ToArray( );
    }

    protected void AddFiles( string[ ] fileNames )
    {
      foreach( var str in FilterFileNames( fileNames ) )
      {
        using( var fileStream = new FileStream( str, FileMode.Open, FileAccess.Read, FileShare.Read ) )
        {
          var member = View.ObjectTypeInfo.FindMember( View.ObjectTypeInfo.FindAttribute< FileAttachmentAttribute >( ).FileDataPropertyName );

          var @object = ObjectSpace.CreateObject < ArchivoAdjunto>( );
          
          ArchivosAdjuntosController.SetupNewObject( @object, ObjectSpace, ( ListView ) View );
        
          var fileData = member.GetValue( @object ) as IFileData;
          if( fileData == null )
          {
            fileData = FileAttachmentsWindowsFormsModule.CreateFileData( ObjectSpace, member );
            member.SetValue( @object, fileData );
          }
          FileDataHelper.LoadFromStream( fileData, Path.GetFileName( str ), fileStream, str );
          if( View.IsRoot )
            ObjectSpace.CommitChanges( );
          ( ( ListView ) View ).CollectionSource.Add( @object );
          if( !View.IsRoot )
            ObjectSpace.SetModified( @object );
        }
      }
    }

    private void View_ControlsCreated( object sender, EventArgs e )
    {
      var control = ( ( ListView ) View ).Editor.Control as Control;
      if( control == null )
        return;
      control.DragDrop += grid_DragDrop;
      control.DragOver += grid_DragOver;
    }

    private void InitializeComponent( )
    {
      components = new Container( );
      addFromFileAction = new SimpleAction( components );
      addFromFileAction.Caption = "Add From File...";
      addFromFileAction.Category = "ObjectsCreation";
      addFromFileAction.ToolTip = "Create a new record with the selected file attached";
      addFromFileAction.Id = "CustomAddFromFile";
      addFromFileAction.ImageName = "MenuBar_AttachmentObject";
      addFromFileAction.Shortcut = "CtrlN";
      addFromFileAction.Execute += addFromFileAction_OnExecute;
    }

    private void addFromFileAction_OnExecute( object sender, SimpleActionExecuteEventArgs args )
    {
      AddFromFile( );
    }

    private void UpdateActionState( )
    {
      string diagnosticInfo;
      addFromFileAction.Enabled.SetItemValue( "Security", DataManipulationRight.CanCreate( View, View.ObjectTypeInfo.Type, LinkToListViewController.FindCollectionSource( Frame ), out diagnosticInfo ) );
      if( View != null && View.IsRoot )
        addFromFileAction.Enabled.SetItemValue( "ObjectSpaceNotModified", !ObjectSpace.IsModified );
      else
        addFromFileAction.Enabled.RemoveItem( "ObjectSpaceNotModified" );
    }

    private void View_AllowNewChanged( object sender, EventArgs e )
    {
      UpdateActionState( );
    }

    private void ObjectSpace_ModifiedChanged( object sender, EventArgs e )
    {
      UpdateActionState( );
    }

    private void CollectionSource_CollectionLoaded( object sender, EventArgs e )
    {
      UpdateActionState( );
    }

    private string GetFileTypesFilterValue( string className, string propertyName )
    {
      IModelClass modelClass = View.Model.Application.BOModel[ className ];
      IModelMember member = modelClass.FindMember( propertyName );
      IModelCommonFileTypeFilters commonFileTypeFilters = member == null || !member.MemberInfo.Owner.IsAssignableFrom( modelClass.TypeInfo ) ? modelClass as IModelCommonFileTypeFilters : member as IModelCommonFileTypeFilters;
      if( commonFileTypeFilters != null )
        return commonFileTypeFilters.FileTypeFilters.FileTypesFilter;
      return string.Empty;
    }

    protected virtual string GetFileTypesFilter( )
    {
      var listView = View as ListView;
      string str = GetFileTypesFilterValue( listView.ObjectTypeInfo.FullName, GetFileAttachmentAttribute( listView ).FileDataPropertyName );
      if( !listView.IsRoot && listView.CollectionSource is PropertyCollectionSource )
      {
        string typesFilterValue = GetFileTypesFilterValue( ( ( PropertyCollectionSource ) listView.CollectionSource ).MasterObjectType.FullName, ( ( PropertyCollectionSource ) listView.CollectionSource ).MemberInfo.Name );
        if( !string.IsNullOrEmpty( typesFilterValue ) )
          str = typesFilterValue;
      }
      return str;
    }

    protected virtual void AddFromFile( )
    {
      using( var openFileDialog = new OpenFileDialog( ) )
      {
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.DereferenceLinks = true;
        openFileDialog.Multiselect = true;
        openFileDialog.Filter = GetFileTypesFilter( );
        if( openFileDialog.ShowDialog( Form.ActiveForm ) != DialogResult.OK )
          return;
        AddFiles( openFileDialog.FileNames );
      }
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );
      if( !( bool ) Active )
        return;
      UpdateActionState( );
      ( ( ListView ) View ).CollectionSource.CollectionChanged += CollectionSource_CollectionLoaded;
      View.ControlsCreated += View_ControlsCreated;
      View.AllowNewChanged += View_AllowNewChanged;
      ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
    }

    protected override void OnDeactivated( )
    {
      base.OnDeactivated( );

      var attachCtrl = Frame.GetController<FileAttachmentListViewController>( );
      if( attachCtrl != null && attachCtrl.Active.Contains( "reemplazado" ) )
        attachCtrl.Active.RemoveItem( "reemplazado" );

      var listView = ( ListView ) View;
      listView.CollectionSource.CollectionChanged -= CollectionSource_CollectionLoaded;
      listView.ControlsCreated -= View_ControlsCreated;
      View.AllowNewChanged -= View_AllowNewChanged;
      ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
      if( listView.Editor == null )
        return;
      var control = ( ( ListView ) View ).Editor.Control as Control;
      if( control == null )
        return;
      control.DragDrop -= grid_DragDrop;
      control.DragOver -= grid_DragOver;
    }
  }
}
