using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class FileEditViewModel
        : FileViewModel
    {
        public FileEditViewModel(string displayName, FileModel fileModel)
            : base(displayName, fileModel)
        {

        }
    }
}
