using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.CoreLibrary
{
    public static class FileExtensions
    {
        public static FileInfo Archive(this FileInfo fileInfo, string path, bool bOverwrite)
        {
            string outputPath = (path.Contains("."))?Path.GetDirectoryName(path):path;
            string outputFileName = (path.Contains(".")) ? Path.GetFileName(path) : string.Empty;
            if (string.Empty == outputFileName)
                outputFileName = fileInfo.Name;
            outputFileName = Path.Combine(outputPath, outputFileName);
            if (File.Exists(outputFileName) && bOverwrite)
                File.Delete(outputFileName);
            fileInfo.MoveTo(outputFileName);
            return new FileInfo(outputFileName);
        }
    }
}
