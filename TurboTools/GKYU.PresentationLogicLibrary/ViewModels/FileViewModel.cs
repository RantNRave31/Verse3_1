using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.Settings;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class FileViewModel
        : ViewModelBase
    {
        private FileModel _fileModel;
        public FileModel FileModel
        {
            get
            {
                return _fileModel;
            }
            set
            {
                _fileModel = value;
                OnPropertyChanged("FileModel");
            }
        }

        public Action<object, EventArgs> RequestClose { get; internal set; }
        public RelayCommand CloseCommand { get; set; }

        public Dictionary<string, Configuration.Setting> PropertyMap { get; set; }

        public ObservableCollection<Configuration.Setting> Properties { get; set; }
        public string FileName { get; set; }

        public FileViewModel(string name, FileModel fileModel)
            : base(name)
        {
            _fileModel = fileModel;
            PropertyMap = new Dictionary<string, Configuration.Setting>();
            Properties = new ObservableCollection<Configuration.Setting>();
            CloseCommand = new RelayCommand(o => { if (null != RequestClose) RequestClose(this, new EventArgs()); });
        }
    }
}
