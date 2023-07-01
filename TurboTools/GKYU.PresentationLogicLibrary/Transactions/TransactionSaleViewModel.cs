using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.Transactions
{
    public class TransactionSaleViewModel
        : TransactionNodeViewModel
    {
        protected int _departmentID;
        public int Department { get { return _departmentID; } set { if (_departmentID == value) return; _departmentID = value; OnPropertyChanged(); } }
        protected decimal _plu;
        public decimal PLU { get { return _plu; } set { if (_plu == value) return; _plu = value; OnPropertyChanged(); } }
        protected string _description;
        public string Description { get { return _description; } set { if (_description == value) return; _description = value; OnPropertyChanged(); } }
        protected string _taxCode;
        public string TaxCode { get { return _taxCode; } set { if (_taxCode == value) return; _taxCode = value; OnPropertyChanged(); } }
        protected string _utilizationCode;
        public string UtilizationCode { get { return _utilizationCode; } set { if (_utilizationCode == value) return; _utilizationCode = value; OnPropertyChanged(); } }

        protected int _count;
        public int Count { get { return _count; } set { if (_count == value) return; _count = value; OnPropertyChanged(); } }
        protected string _quantityUOM;
        public string QuantityUOM { get { return _quantityUOM; } set { if (_quantityUOM == value) return; _quantityUOM = value; OnPropertyChanged(); } }
        protected decimal _quantity;
        public decimal Quantity { get { return _quantity; } set { if (_quantity == value) return; _quantity = value; OnPropertyChanged(); } }        
        protected decimal _cost;
        public decimal Cost { get { return _cost; } set { if (_cost == value) return; _cost = value; OnPropertyChanged(); } }

        protected string _priceUOM;
        public string PriceUOM { get { return _priceUOM; } set { if (_priceUOM == value) return; _priceUOM = value; OnPropertyChanged(); } }
        protected int _priceMSU;
        public int PriceMSU { get { return _priceMSU; } set { if (_priceMSU == value) return; _priceMSU = value; OnPropertyChanged(); } }
        protected string _currencySymbol;
        public string CurrencySymbol { get { return _currencySymbol; } set { if (_currencySymbol == value) return; _currencySymbol = value; OnPropertyChanged(); } }
        protected decimal _price;
        public decimal Price { get { return _price; } set { if (_price == value) return; _price = value; OnPropertyChanged(); } }
        protected decimal _amount;
        public decimal Amount { get { return _amount; } set { if (_amount == value) return; _amount = value; OnPropertyChanged(); } }
        public string QuantityDescription { get { return string.Format("{0} ({1})", Quantity, QuantityUOM); } }
        public string PriceDescription { get { return string.Format("{0} ({1}) / {2}{3}", PriceMSU, PriceUOM, CurrencySymbol, Price); } }
        public string SaleDescription { get { return string.Format("{0} {1} @ {2}", QuantityDescription, Description, PriceDescription); } }
        public string AmountDescription { get { return string.Format("{0}{1}", CurrencySymbol, Amount); } }
        public TransactionSaleViewModel(string name)
            : base(name)
        {

        }
    }
}
