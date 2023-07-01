using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class ObservableDictionaryViewModel
        : FileViewModel
    {
        public ObservableDictionaryViewModel(string name, FileModel fileModel)
            : base(name, fileModel)
        {

        }
    }
}
