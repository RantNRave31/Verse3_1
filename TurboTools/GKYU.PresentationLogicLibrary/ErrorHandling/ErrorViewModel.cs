using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.ErrorHandling
{
    public partial class ErrorViewModel
        : ViewModelBase
    {
        public ErrorViewModel.CODE errorCodeID;
        public object source;
        public string message { get; set; }
        public string Name
        {
            get
            {
                if (ErrorCodes.ContainsKey(errorCodeID))
                    return "<UNDEFINED>";
                else
                    return ErrorCodes[errorCodeID].name;
            }
        }
        protected int _progress;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (value == _progress)
                    return;
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }
        protected bool _handled;
        public bool Handled
        {
            get
            {
                return _handled;
            }
            set
            {
                if (value == _handled)
                    return;
                _handled = value;
                OnPropertyChanged("Handled");
            }
        }
        public static event Action<ErrorViewModel> OnErrorHandled;
        public ErrorViewModel(object source, ErrorViewModel.CODE errorCodeID, string message)
            : base("ErrorViewModel")
        {
            this.errorCodeID = errorCodeID;
            this.message = message;
        }
        public static ErrorViewModel Report(object source, ErrorViewModel.CODE errorCodeID, string message)
        {
            ErrorViewModel result = new ErrorViewModel(source, errorCodeID, message);
            return result;
        }
    }
}
