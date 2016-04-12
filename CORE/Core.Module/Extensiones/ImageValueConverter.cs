using System;
using System.Drawing;
using DevExpress.Xpo.Metadata;

namespace FDIT.Core
{
    public class ImageValueConverter : ValueConverter
    {
        private readonly ImageConverter _imageConverter;

        public ImageValueConverter()
        {
            _imageConverter = new ImageConverter();
        }

        public override Type StorageType => typeof (byte[]);

        public override object ConvertToStorageType(object value)
        {
            return value == null ? null : _imageConverter.ConvertTo(value, StorageType);
        }

        public override object ConvertFromStorageType(object value)
        {
            return value == null ? null : _imageConverter.ConvertFrom(value);
        }
    }
}