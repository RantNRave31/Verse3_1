using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.Transactions
{
    public class TransactionNodeViewModel
        : ViewModelBase
    {
        protected int _storeID;
        public int StoreID { get { return _storeID; } set { if (_storeID == value) return; _storeID = value; OnPropertyChanged(); } }
        protected int _registerID;
        public int RegisterID { get { return _registerID; } set { if (_registerID == value) return; _registerID = value; OnPropertyChanged(); } }
        protected int _cashierID;
        public int CashierID { get { return _cashierID; } set { if (_cashierID == value) return; _cashierID = value; OnPropertyChanged(); } }
        protected int _ticketID;
        public int TicketID { get { return _ticketID; } set { if (_ticketID == value) return; _ticketID = value; OnPropertyChanged(); } }
        protected int _sequenceNumber;
        public int SequenceNumber { get { return _sequenceNumber; } set { if (_sequenceNumber == value) return; _sequenceNumber = value; OnPropertyChanged(); } }
        public TransactionNodeViewModel(string name)
            : base(name)
        {

        }
    }
}
