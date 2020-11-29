using System;
using System.IO;
using System.Windows.Data;

namespace ImageUtility.Shell.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FilePathToNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new InvalidOperationException("The target must be a valid file path");
            }

            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                return Path.GetFileName(path);
            }

            throw new InvalidOperationException("Invalid file path");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}