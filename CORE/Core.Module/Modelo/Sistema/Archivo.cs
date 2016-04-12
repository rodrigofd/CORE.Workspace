using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core
{
    [DefaultProperty("FileName")]
    [DeferredDeletion(false), OptimisticLocking(false)]
    [Persistent("sistema.Archivo")]
    public class Archivo : XPCustomObject, IFileData, IEmptyCheckable
    {
        public static int ReadBytesSize = 4096; //4KB
        public static string FileSystemStoreLocation = "";

        private static readonly object SyncRoot = new object();
        private Guid fOid;
        private string fTempFileName = string.Empty;
        private Stream fTempSourceStream;

        static Archivo()
        {
        }

        public Archivo(Session session)
            : base(session)
        {
        }

        [Key]
        public Guid Oid
        {
            get { return fOid; }
            set { SetPropertyValue("Oid", ref fOid, value); }
        }

        public string RealFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && Oid != Guid.Empty)
                    return Path.Combine(FileSystemStoreLocation, string.Format("{0}-{1}", Oid, FileName));
                return null;
            }
        }

        #region IEmptyCheckable Members

        public bool IsEmpty
        {
            get { return FileDataHelper.IsFileDataEmpty(this) || !File.Exists(RealFileName); }
        }

        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Oid = Guid.NewGuid();
        }

        public static void CopyFileToStream(string sourceFileName, Stream destination)
        {
            if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
            using (Stream source = File.OpenRead(sourceFileName))
                CopyStream(source, destination);
        }

        public static void OpenFileWithDefaultProgram(string sourceFileName)
        {
            Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");
            Process.Start(sourceFileName);
        }

        public static void CopyStream(Stream source, Stream destination)
        {
            if (source == null || destination == null) return;
            var buffer = new byte[ReadBytesSize];

            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, read);
        }

        protected virtual void SaveFileToStore()
        {
            if (!string.IsNullOrEmpty(RealFileName))
            {
                try
                {
                    using (Stream destination = File.OpenWrite(RealFileName))
                    {
                        CopyStream(TempSourceStream, destination);
                        Size = (int) destination.Length;
                    }
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }

        private void RemoveOldFileFromStore()
        {
            //Dennis: We need to remove the old file from the store when saving the current object.
            if (!string.IsNullOrEmpty(fTempFileName) && fTempFileName != RealFileName)
            {
                //B222892
                try
                {
                    File.Delete(fTempFileName);
                    fTempFileName = string.Empty;
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Guard.ArgumentNotNullOrEmpty(FileSystemStoreLocation, "FileSystemStoreLocation");
            lock (SyncRoot)
            {
                if (!Directory.Exists(FileSystemStoreLocation))
                    Directory.CreateDirectory(FileSystemStoreLocation);
            }
            SaveFileToStore();
            RemoveOldFileFromStore();
        }

        protected override void OnDeleting()
        {
            //Dennis: We need to remove the old file from the store.
            Clear();
            base.OnDeleting();
        }

        protected override void Invalidate(bool disposing)
        {
            if (disposing && TempSourceStream != null)
            {
                TempSourceStream.Close();
                TempSourceStream = null;
            }
            base.Invalidate(disposing);
        }

        #region IFileData Members

        [Browsable(false)]
        public Stream TempSourceStream
        {
            get { return fTempSourceStream; }
            set
            {
                fTempSourceStream = value;
                //Dennis: For Windows Forms applications.
                if (fTempSourceStream is FileStream)
                {
                    try
                    {
                        fTempSourceStream = File.OpenRead(((FileStream) fTempSourceStream).Name);
                    }
                    catch (FileNotFoundException exc)
                    {
                        throw new UserFriendlyException(exc);
                    }
                }
            }
        }

        public void Clear()
        {
            //Dennis: When clearing the file name property we need to save the name of the old file to remove it from the store in the future. You can also setup a separate service for that.
            if (string.IsNullOrEmpty(fTempFileName))
                fTempFileName = RealFileName;
            FileName = string.Empty;
            Size = 0;
        }

        [Size(260)]
        public string FileName
        {
            get { return GetPropertyValue<string>("FileName"); }
            set { SetPropertyValue("FileName", value); }
        }

        //Dennis: Fires when uploading a file.
        void IFileData.LoadFromStream(string fileName, Stream source)
        {
            FileName = fileName;
            TempSourceStream = source;
            //Dennis: When assigning a new file we need to save the name of the old file to remove it from the store in the future.
            if (string.IsNullOrEmpty(fTempFileName))
                fTempFileName = RealFileName;
        }

        //Dennis: Fires when saving or opening a file.
        void IFileData.SaveToStream(Stream destination)
        {
            try
            {
                if (!string.IsNullOrEmpty(RealFileName))
                {
                    if (destination == null)
                        OpenFileWithDefaultProgram(RealFileName);
                    else
                        CopyFileToStream(RealFileName, destination);
                }
                else if (TempSourceStream != null)
                    CopyStream(TempSourceStream, destination);
            }
            catch (DirectoryNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
            catch (FileNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
        }

        [Persistent]
        public int Size
        {
            get { return GetPropertyValue<int>("Size"); }
            private set { SetPropertyValue("Size", value); }
        }

        #endregion
    }
}