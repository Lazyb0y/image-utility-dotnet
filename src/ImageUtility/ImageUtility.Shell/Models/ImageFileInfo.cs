using System.Drawing;
using System.IO;
using ImageUtility.Shell.MVVM;

namespace ImageUtility.Shell.Models
{
    public class ImageFileInfo : Observable
    {
        #region Property(s)

        private FileInfo _fileInfo;
        public FileInfo FileInfo
        {
            get => _fileInfo;
            set
            {
                _fileInfo = value;
                OnPropertyChanged();
            }
        }

        private Size _dimension;
        public Size Dimension
        {
            get => _dimension;
            set
            {
                _dimension = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}