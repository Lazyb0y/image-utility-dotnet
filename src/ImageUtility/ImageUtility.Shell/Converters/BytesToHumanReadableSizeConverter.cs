using System;
using System.Windows.Data;

namespace ImageUtility.Shell.Converters
{
    public class BytesToHumanReadableSizeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is long length)
            {
                string[] sizes = {"B", "KB", "MB", "GB", "TB", "PB", "EB"};
                if (length == 0)
                {
                    return "0" + sizes[0];
                }

                var bytes = Math.Abs(length);
                var order = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                var num = Math.Round(bytes / Math.Pow(1024, order), 1);
                return $"{Math.Sign(length) * num:0.##} {sizes[order]}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
