using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.Controls;
using GKYU.PresentationLogicLibrary.Transactions;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class TouchTemplateViewModel
        : FileViewModel
    {
        protected ObservableCollection<TouchCommandViewModel> _touchCommands = new ObservableCollection<TouchCommandViewModel>();
        public ObservableCollection<TouchCommandViewModel> TouchCommands { get { return _touchCommands; } }
        public List<List<string>> CashierUpperRightPanel { get; set; }
        public List<List<string>> CashierLowerRightPanel { get; set; }
        public TransactionTicketViewModel _transactionTicketViewModel;
        public TransactionTicketViewModel TicketViewModel { get { return _transactionTicketViewModel; } set { if (_transactionTicketViewModel == value) return; _transactionTicketViewModel = value; OnPropertyChanged(); } }
        public TouchTemplateViewModel(string name, FileModel fileModel)
            : base(name, fileModel)
        {
            CashierUpperRightPanel = new List<List<string>>() {
                { new List<string>(){ "Department", "Manager"} },
                { new List<string>(){ "Tender", "Other"} },
                { new List<string>(){ "SignOn", "Paid Out"} },
                { new List<string>(){ "SignOff", "ROA"} },
            };
            CashierLowerRightPanel = new List<List<string>>() {
                { new List<string>(){ "7", "8", "9", " " } },
                { new List<string>(){ "4", "5", "6", "<-" } },
                { new List<string>(){ "1", "2", "3", "C" } },
                { new List<string>(){ ".", "0", "00", "Total" } },
            };
        }
    }
}
