using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.BusinessLogicLibrary.Bitmaps
{
    public class TextFileModel
        : FileModel
    {
        public TextFileModel(string fileName)
            : base(fileName)
        {

        }
        public TextFileModel()
            : this("New Text File.txt")
        {

        }
    }
}
