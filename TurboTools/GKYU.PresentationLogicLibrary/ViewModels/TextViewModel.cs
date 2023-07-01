using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class TextViewModel
        : FileViewModel
    {

        private RelayCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }
        public bool CanSave = true;
        public void Save()
        {
            //if (!_bitmap.IsValid)
            //    throw new InvalidOperationException("...");
            //if (this.IsNewCustomer)
            //    _customerRepository.AddCustomer(_customer);
            base.OnPropertyChanged("DisplayName");

        }

        public TextViewModel(string displayName, FileModel fileViewModel)
            : base(displayName, fileViewModel)
        {

        }
    }
}
