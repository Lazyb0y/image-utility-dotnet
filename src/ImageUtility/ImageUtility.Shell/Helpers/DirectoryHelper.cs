using System.Collections.Generic;
using System.IO;

namespace ImageUtility.Shell.Helpers
{
    public static class DirectoryHelper
    {
        public static List<FileInfo> GetFilesOfExtension(string searchPath, string[] filters, bool isRecursive)
        {
            var filesFound = new List<FileInfo>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            foreach (var filter in filters)
            {
                filesFound.AddRange(new DirectoryInfo(searchPath).GetFiles($"*.{filter}", searchOption));
            }

            return filesFound;
        }
    }
}