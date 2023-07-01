using GKYU.PresentationCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.BusinessLogicLibrary.Bitmaps
{
    public class FileModel
        : Model
    {
        public string fullName;
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                if (value != fullName)
                {
                    fullName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Name
        {
            get
            {
                return Path.GetFileName(FullName);
            }
            set
            {
                FullName = Path.GetFullPath(fullName) + value;
                NotifyPropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        private bool canSave = true;
        public bool CanSave
        {
            get
            {
                return canSave;
            }
        }
        public FileModel(string fileName)
            : base()
        {
            fullName = fileName;
        }
        public FileModel()
            : this("New File.ext")
        {

        }
    }
}
