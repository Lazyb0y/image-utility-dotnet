using System.Collections.Generic;
using System.IO;

namespace ImageUtility.Shell.Helpers
{
    public static class DirectoryHelper
    {
        public static List<string> GetFilesOfExtension(string searchPath, string[] filters, bool isRecursive)
        {
            var filesFound = new List<string>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchPath, $"*.{filter}", searchOption));
            }

            return filesFound;
        }
    }
}