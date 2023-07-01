using GKYU.BusinessLogicLibrary.Bitmaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class TouchCommandViewModel
        : FileViewModel
    {
        public TouchCommandViewModel(string name, FileModel fileModel)
            : base(name, fileModel)
        {

        }
    }
}
