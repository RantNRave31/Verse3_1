using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using System.Collections.ObjectModel;

namespace GKYU.PresentationLogicLibrary.Transactions
{
    public class TransactionTicketViewModel
        : FileViewModel
    {
        protected decimal _itemCount;
        public decimal ItemCount { get { return _itemCount; } set { if (_itemCount == value) return; _itemCount = value; OnPropertyChanged(); } }
        protected decimal _saleTotal;
        public decimal SaleTotal { get { return _saleTotal; } set { if (_saleTotal == value) return; _saleTotal = value; OnPropertyChanged(); } }
        protected decimal _taxTotal;
        public decimal TaxTotal { get { return _taxTotal; } set { if (_taxTotal == value) return; _taxTotal = value; OnPropertyChanged(); } }
        protected decimal _tenderTotal;
        public decimal TenderTotal { get { return _tenderTotal; } set { if (_tenderTotal == value) return; _tenderTotal = value; OnPropertyChanged(); } }
        public decimal BalanceDue { get { return SaleTotal + TaxTotal - TenderTotal; } }
        public ObservableCollection<TransactionNodeViewModel> TransactionItems {get;set;}
        public TransactionTicketViewModel(string name, FileModel fileModel)
            : base(name, fileModel)
        {
            TransactionItems = new ObservableCollection<TransactionNodeViewModel>();
        }
    }
}
