using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ImageUtility.Shell.Helpers;
using ImageUtility.Shell.MVVM;

namespace ImageUtility.Shell.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Decleration(s)

        private static readonly string[] Filters = {"jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg"};
        private const string TargetFolderName = "output";

        #endregion

        #region Property(s)

        private ObservableCollection<string> _sourceImagesFiles;
        public ObservableCollection<string> SourceImagesFiles
        {
            get => _sourceImagesFiles;
            set
            {
                _sourceImagesFiles = value;
                OnPropertyChanged();
            }
        }

        private string _sourceFolder;
        public string SourceFolder
        {
            get => _sourceFolder;
            set
            {
                _sourceFolder = value;
                OnPropertyChanged();
            }
        }

        private int _compressQuality;
        public int CompressQuality
        {
            get => _compressQuality;
            set
            {
                _compressQuality = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command(a)

        public ICommand SelectSourceFolderCommand { get; set; }
        public ICommand ConvertCommand { get; set; }
        #endregion

        #region Constructor(s)

        public MainWindowViewModel()
        {
            SourceImagesFiles = new ObservableCollection<string>();
            CompressQuality = 80;

            SelectSourceFolderCommand = new RelayCommand(param => SelectSourceFolderCommandAction(), param => !IsBusy);
            ConvertCommand = new RelayCommand(param => ConvertCommandActionAsync(), param => !IsBusy);
        }

        #endregion

        #region Command Action(s)

        private void SelectSourceFolderCommandAction()
        {
            SelectSourceFolder();
            LoadImageFiles();
        }

        private async void ConvertCommandActionAsync()
        {
            await ConvertImagesAsync();
        }

        #endregion

        #region Method(s)

        private void SelectSourceFolder()
        {
            var folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };

            var result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                SourceFolder = folderDlg.SelectedPath;
            }
        }

        private void LoadImageFiles()
        {
            SourceImagesFiles.Clear();
            foreach (var filePath in DirectoryHelper.GetFilesOfExtension(SourceFolder, Filters, false))
            {
                SourceImagesFiles.Add(filePath);
            }
        }

        private bool ValidateSourceDirectory()
        {
            if (string.IsNullOrWhiteSpace(SourceFolder))
            {
                MessageBox.Show(@"source folder path cannot be empty.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private bool ValidateTargetDirectory(string targetDirectory)
        {
            if (string.IsNullOrWhiteSpace(targetDirectory))
            {
                MessageBox.Show(@"save folder path cannot be empty.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (Directory.Exists(targetDirectory))
            {
                MessageBox.Show(@"Save folder location already exists.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private async Task ConvertImagesAsync()
        {
            if (!ValidateSourceDirectory())
            {
                return;
            }

            var targetDirectory = Path.Combine(SourceFolder, TargetFolderName);
            if (!ValidateTargetDirectory(targetDirectory))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(targetDirectory);
            }
            catch (Exception x)
            {
                MessageBox.Show(@"Unable to create save folder." + Environment.NewLine + x.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsBusy = true;

            try
            {
                foreach (var sourceImagesFile in SourceImagesFiles)
                {
                    var fileName = Path.GetFileName(sourceImagesFile);
                    ImageHelpers.CompressImage(sourceImagesFile, Path.Combine(targetDirectory, fileName), CompressQuality);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(@"Where was an error while converting images." + Environment.NewLine + x.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                IsBusy = false;
            }
            
            if (MessageBox.Show(
                    @"Operation completed successfully. Do you want to open save folder in windows explorer?", @"Done",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) ==
                DialogResult.Yes)
            {
                try
                {
                    Process.Start(targetDirectory);
                }
                catch (Exception x)
                {
                    MessageBox.Show(@"Unable to open save folder." + Environment.NewLine + x.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}