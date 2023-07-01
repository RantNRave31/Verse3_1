using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using System.Windows;
using System.IO;
using System.Threading;

using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using System.Windows.Media;
using GKYU.PresentationLogicLibrary.Transactions;
using GKYU.PresentationCoreLibrary.ViewModels;

namespace GKYU.PresentationLogicLibrary
{
    public static class ColllectionExtensions
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }
    }
    public class FileViewCommand
    {
        public bool Selected { get; set; }
        public string Name { get; set; }
    }
    public partial class WorkspaceViewModel
        : ViewModelBase
    {
        public static WorkspaceViewModel Instance { get; set; }
        public class CommunicationsInterface
        {
            public int Port { get; set; }
            public string Name { get; set; }
            public string DeviceID { get; set; }
        }
        public ObservableCollection<FileViewModel> FileViewModels { get; set; }
        private FileViewModel _selectedFileViewModel;
        public FileViewModel SelectedFileViewModel { get { return _selectedFileViewModel; } set { if (value == _selectedFileViewModel) return; _selectedFileViewModel = value; OnPropertyChanged("SelectedFileViewModel"); } }
        private ObservableCollection<CommandViewModel> _commands;
        public ObservableCollection<CommandViewModel> Commands
        {
            get
            {
                return _commands;
            }
        }
        public ObservableCollection<FileViewCommand> FileViewCommands { get; set; }
        #region Configuration
        private Tuple<string, FileViewCommand>[] _packingListParameters { get; set; }
        private string _pinPadDirectory;
        public string PinPadDirectory
        {
            get
            {
                return _pinPadDirectory;
            }
            set
            {
                if (value == _pinPadDirectory)
                    return;
                _pinPadDirectory = value;
                OnPropertyChanged("PinPadDirectory");
            }
        }
        private DateTime _currentDateTime;
        public DateTime CurrentDateTime
        {
            get
            {
                return _currentDateTime;
            }
            set
            {
                if (value == _currentDateTime)
                    return;
                _currentDateTime = value;
                OnPropertyChanged("CurrentDate");
                OnPropertyChanged("CurrentTime");
            }
        }
        public string CurrentDate
        {
            get
            {
                return _currentDateTime.ToShortDateString();
            }
            set
            {

            }
        }
        public string CurrentTime
        {
            get
            {
                return _currentDateTime.ToLongTimeString();
            }
            set
            {

            }
        }
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value == _password)
                    return;
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private bool _securePassword;
        public bool SecurePassword
        {
            get
            {
                return _securePassword;
            }
            set
            {
                if (value == _securePassword)
                    return;
                _securePassword = value;
                OnPropertyChanged("SecurePassword");
            }
        }
        #endregion
        public WorkspaceViewModel(string displayName)
            : base(displayName)
        {
            Instance = this;
            FileViewModels = new ObservableCollection<FileViewModel>();
            _commands = new ObservableCollection<CommandViewModel>()
            {
                new CommandViewModel("Open File", new RelayCommand(o => {
                    if(null == o)
                        o = "New File.bmp";
                    string fileExtension = Path.GetExtension((string)o);
                    switch(fileExtension.ToLower())
                    {
                        case ".bmp":
                            this.FileViewModels.Add(this.SelectedFileViewModel = new BitmapViewModel((string)o, new BitmapFileModel((string)o), 1024, 768, 300, 300, PixelFormats.Rgb24  ));
                            break;
                        case ".txt":
                            this.FileViewModels.Add(this.SelectedFileViewModel = new TextViewModel((string)o, new TextFileModel()));
                            break;
                        default:
                            break;
                    }
                })),
            };

            _packingListParameters = new Tuple<string, FileViewCommand>[]
            {
                new Tuple<string,FileViewCommand>("", new FileViewCommand(){ Selected=false, Name="Load PinPad OS" } ),
                new Tuple<string,FileViewCommand>("", new FileViewCommand(){ Selected=false, Name="Load PinPad Form Agent" } ),
                new Tuple<string,FileViewCommand>("", new FileViewCommand(){ Selected=false, Name="Load PinPad CTLS" } ),
                new Tuple<string,FileViewCommand>("", new FileViewCommand(){ Selected=false, Name="Load PinPad Keys" } ),
            };
            FileViewCommands = new ObservableCollection<FileViewCommand>()
            {
                _packingListParameters[0].Item2,
                _packingListParameters[1].Item2,
                _packingListParameters[2].Item2,
                _packingListParameters[3].Item2,
                new FileViewCommand(){ Selected=true, Name="Pull PinPad Parameters" },
                new FileViewCommand(){ Selected=false, Name="Set PinPad Date/Time" },
                new FileViewCommand(){ Selected=false, Name="Set PinPad Password" },
            };
            PinPadDirectory = @"C:\RDS\PinPad";
        }
        #region Refresh Command
        CommandHandler _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new CommandHandler(Refresh, param => { return true; });
                }
                return _refreshCommand;
            }
        }
        private static List<string> GetPinPadSerialPorts()
        {
            List<string> serialPorts = new List<string>();
            string comportInfo = string.Empty;

            //using (var entitySearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity"))
            //using (var serialPortSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName"))
            //{
            //    var portList = serialPortSearcher.Get().Cast<ManagementBaseObject>().ToList();
            //    foreach (var matchingEntity in portList)
            //    {
            //        comportInfo = matchingEntity["PortName"].ToString();
            //        serialPorts.Add(comportInfo);
            //    }
            serialPorts.Add("COM1");
            serialPorts.Add("COM2");

            //}
            return serialPorts;
        }
        private void RefreshEquipmentPanel()
        {
            List<string> serialPorts = GetPinPadSerialPorts();
            FileViewModels.Clear();
            foreach (string serialPort in serialPorts)
            {
                string port = serialPort.Substring(3);
                FileViewModels.Add(new EquipmentViewModel(string.Format("PinPad ({0})", serialPort), new FileModel()) { Port = int.Parse(port), Name = string.Format("PinPad ({0})", serialPort) });
            }

        }
        private TransactionSaleViewModel AddSaleItem(TransactionTicketViewModel transactionTicketViewModel, int storeID, int registerID, int cashierID, int ticketID, int sequenceNumber, int departmentNumber, decimal plu, string description, string taxCode, int count, decimal quantity, string quantityUOM, decimal cost, int priceMSU, string priceUOM, decimal price)
        {
            TransactionSaleViewModel result = new TransactionSaleViewModel(string.Format("{0}{1}{2}{3}{4}", storeID, registerID, cashierID, ticketID, sequenceNumber)) {
                Department = departmentNumber,
                PLU = plu,
                Description = description,
                TaxCode = taxCode,
                Count = count,
                Quantity = quantity,
                QuantityUOM = quantityUOM,
                PriceMSU = (priceMSU <= 0) ? 1 : priceMSU,
                PriceUOM = priceUOM,
                Cost = cost,
                CurrencySymbol = "$",
                Price = price,
                Amount = price / priceMSU * quantity
            };
            transactionTicketViewModel.TransactionItems.Add(result);
            transactionTicketViewModel.ItemCount += result.Count;
            transactionTicketViewModel.SaleTotal += result.Amount;
            switch (result.TaxCode)
            {
                case "TF":
                    transactionTicketViewModel.TaxTotal += (result.Amount * 0.045M);
                    break;
                default:
                    break;
            }
            return result;
        }
        private TransactionTicketViewModel CreateTicket(int storeID, int registerID, int cashierID, int ticketID)
        {
            TransactionTicketViewModel result = new TransactionTicketViewModel("Transactions", new FileModel() { });
            int sequenceNumber = 0;
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 3, 4011, "Banannas", "TF", 1, 1.6M, "LB", 0.00M, 1, "LB", 0.49M);
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 1, 123456, "Taco Seasoning", "TF", 1, 1.6M, "EA", 0.00M, 1, "EA", 0.49M);
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 2, 58632145, "Hammer", "NF", 1, 1.6M, "EA", 0.00M, 1, "EA", 0.49M);
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 3, 4035, "Bell Pepper", "TF", 1, 1.6M, "EA", 0.00M, 1, "EA", 0.49M);
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 6, 1540012345, "Borden Whole Milk", "TF", 1, 1.6M, "GAL", 0.00M, 1, "GAL", 0.49M);
            AddSaleItem(result, storeID, registerID, cashierID, ticketID, sequenceNumber++, 5, 123342546, "Roast Beef, Sliced", "TF", 1, 1.6M,"LB", 0.00M, 1,"LB", 0.49M);
            return result;
        }
        public virtual void Refresh(object parameter)
        {
            RefreshEquipmentPanel();
            FileViewModels.Add(new FileEditViewModel("Verse3", new FileModel()) { Name = "Verse3" });
            FileViewModels.Add(new ObservableDictionaryViewModel("Dictionary", new FileModel()) { Name = "Dictionary" });
            
            DataMatrixViewModel dataMatrixModel = new DataMatrixViewModel("DataMatrix", new FileModel() { Name="DataMatrix"}) { };
            dataMatrixModel.DataMatrix = DataMatrixViewModel.CreateDateRange(new DateTime(1968, 12, 28), CurrentDateTime);
            FileViewModels.Add(dataMatrixModel);
            
            TransactionTicketViewModel transactionTicketViewModel = CreateTicket(1, 2, 9999, 1);
            FileViewModels.Add(transactionTicketViewModel);
            
            TouchTemplateViewModel touchTemplateViewModel = new TouchTemplateViewModel("Touch", new FileModel()) { Name = "Touch" };
            touchTemplateViewModel.TicketViewModel = transactionTicketViewModel;
            touchTemplateViewModel.TouchCommands.Add(new TouchCommandViewModel("Department", new FileModel() { Name = "TouchTemplate.xml" }));
            touchTemplateViewModel.TouchCommands.Add(new TouchCommandViewModel("Tender", new FileModel() { Name = "TouchTemplate.xml" }));
            touchTemplateViewModel.TouchCommands.Add(new TouchCommandViewModel("Other", new FileModel() { Name = "TouchTemplate.xml" }));
            touchTemplateViewModel.TouchCommands.Add(new TouchCommandViewModel("SignOn", new FileModel() { Name = "TouchTemplate.xml" }));
            touchTemplateViewModel.TouchCommands.Add(new TouchCommandViewModel("SignOff", new FileModel() { Name = "TouchTemplate.xml" }));
            FileViewModels.Add(touchTemplateViewModel); ;
            SelectedFileViewModel = FileViewModels.FirstOrDefault();
        }
        #endregion
        #region Load PinPad OS Command
        CommandHandler _loadPinPadOSCommand;
        public ICommand LoadPinPadOSCommand
        {
            get
            {
                if (_loadPinPadOSCommand == null)
                {
                    _loadPinPadOSCommand = new CommandHandler(LoadPinPadOS, (x) => { return CanLoadPinPadOS(); });
                }
                return _loadPinPadOSCommand;
            }
        }
        public bool CanLoadPinPadOS()
        {
            _packingListParameters[0] = new Tuple<string, FileViewCommand>(Directory.GetFiles(Path.Combine(PinPadDirectory, "OS"), "*.tgz").FirstOrDefault()
                , _packingListParameters[0].Item2);
            return File.Exists(_packingListParameters[0].Item1);
        }
        public void LoadPinPadOS(object parameter)
        {
            foreach (EquipmentViewModel equipmentViewModel in FileViewModels)
            {
                ThreadStart threadStart = new ThreadStart(equipmentViewModel.LoadPinPadOS);
                if (equipmentViewModel.WorkerThreads.Count == 0)
                {
                    Thread workerThread = new Thread(threadStart);
                    equipmentViewModel.WorkerThreads.Enqueue(workerThread);
                    workerThread.Start();
                }
            }
        }
        #endregion
        #region Load PinPad FormAgent Command
        CommandHandler _loadPinPadFormAgentCommand;
        public ICommand LoadPinPadFormAgentCommand
        {
            get
            {
                if (_loadPinPadFormAgentCommand == null)
                {
                    _loadPinPadFormAgentCommand = new CommandHandler(LoadPinPadFormAgent, (x) => { return CanLoadPinPadFormAgent(); });
                }
                return _loadPinPadFormAgentCommand;
            }
        }
        public bool CanLoadPinPadFormAgent()
        {
            _packingListParameters[1] = new Tuple<string, FileViewCommand>(Directory.GetFiles(Path.Combine(PinPadDirectory, "FormAgent"), "*.tgz").FirstOrDefault()
                , _packingListParameters[1].Item2);
            return File.Exists(_packingListParameters[1].Item1);
        }
        public void LoadPinPadFormAgent(object parameter)
        {
            foreach (EquipmentViewModel equipmentViewModel in FileViewModels)
            {
                ThreadStart threadStart = new ThreadStart(equipmentViewModel.LoadPinPadFormAgent);
                if (equipmentViewModel.WorkerThreads.Count == 0)
                {
                    Thread workerThread = new Thread(threadStart);
                    equipmentViewModel.WorkerThreads.Enqueue(workerThread);
                    workerThread.Start();
                }
            }
        }
        #endregion
        #region Load PinPad CTLS Command
        CommandHandler _loadPinPadCTLSCommand;
        public ICommand LoadPinPadCTLSCommand
        {
            get
            {
                if (_loadPinPadCTLSCommand == null)
                {
                    _loadPinPadCTLSCommand = new CommandHandler(LoadPinPadCTLS, (x) => { return CanLoadPinPadCTLS(); });
                }
                return _loadPinPadCTLSCommand;
            }
        }
        public bool CanLoadPinPadCTLS()
        {
            _packingListParameters[2] = new Tuple<string, FileViewCommand>(Directory.GetFiles(Path.Combine(PinPadDirectory, "CTLS"), "*.tgz").FirstOrDefault()
                , _packingListParameters[2].Item2);
            return File.Exists(_packingListParameters[2].Item1);
        }
        public void LoadPinPadCTLS(object parameter)
        {
            foreach (EquipmentViewModel equipmentViewModel in FileViewModels)
            {
                ThreadStart threadStart = new ThreadStart(equipmentViewModel.LoadPinPadCTLS);
                if (equipmentViewModel.WorkerThreads.Count == 0)
                {
                    Thread workerThread = new Thread(threadStart);
                    equipmentViewModel.WorkerThreads.Enqueue(workerThread);
                    workerThread.Start();
                }
            }
        }
        #endregion
        #region Load PinPad Keys Command
        CommandHandler _loadPinPadKeysCommand;
        public ICommand LoadPinPadKeysCommand
        {
            get
            {
                if (_loadPinPadKeysCommand == null)
                {
                    _loadPinPadKeysCommand = new CommandHandler(LoadPinPadKeys, (x) => { return CanLoadPinPadKeys(); });
                }
                return _loadPinPadKeysCommand;
            }
        }
        public bool CanLoadPinPadKeys()
        {
            _packingListParameters[3] = new Tuple<string, FileViewCommand>(Directory.GetFiles(Path.Combine(PinPadDirectory, "Keys"), "*.tgz").FirstOrDefault()
                , _packingListParameters[3].Item2);
            return File.Exists(_packingListParameters[3].Item1);
        }
        public void LoadPinPadKeys(object parameter)
        {
            foreach (EquipmentViewModel equipmentViewModel in FileViewModels)
            {
                ThreadStart threadStart = new ThreadStart(equipmentViewModel.LoadPinPadKeys);
                if (equipmentViewModel.WorkerThreads.Count == 0)
                {
                    Thread workerThread = new Thread(threadStart);
                    equipmentViewModel.WorkerThreads.Enqueue(workerThread);
                    workerThread.Start();
                }
            }
        }
        #endregion
        #region AddIns
        #endregion

    }
}
