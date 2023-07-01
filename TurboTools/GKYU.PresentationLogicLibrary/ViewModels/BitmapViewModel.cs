using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationCoreLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class BitmapViewModel
        :FileViewModel
    {
        public BitmapFileModel _bitmapModel;

        private WriteableBitmap _writeableBitmap;
        public WriteableBitmap WriteableBitmap
        {
            get { return _writeableBitmap; }
            private set
            {
                if (value == _writeableBitmap) return; _writeableBitmap = value;
                OnPropertyChanged("WidthInPixels");
                OnPropertyChanged("HeightInPixels");
                OnPropertyChanged("WriteableBitmap");
            }
        }
        public int WidthInPixels
        {
            get
            {
                return _writeableBitmap.PixelWidth;
            }
        }
        public int HeightInPixels
        {
            get
            {
                return _writeableBitmap.PixelHeight;
            }
        }
        public int BytesPerPixel { get { return (_writeableBitmap.Format.BitsPerPixel + 7) / 8; } }
        public int Stride { get { return _writeableBitmap.PixelWidth * BytesPerPixel; } }

        private readonly ObservableCollection<string> _history = new ObservableCollection<string>();
        public BitmapFileModel BitmapModel
        {
            get { return _bitmapModel; }
            set
            {
                _bitmapModel = value;
                OnPropertyChanged("BitmapModel");
            }
        }
        public BitmapViewModel(string name, FileModel fileViewModel, int width, int height, int dpiHorizontal, int dpiVertical, PixelFormat pixelFormat)
            : base(name, fileViewModel)
        {
            _writeableBitmap = new WriteableBitmap(width, height, dpiHorizontal, dpiVertical, PixelFormats.Rgb24, null);
        }
        private void AddToHistory(string item)
        {
            if (!_history.Contains(item))
                _history.Add(item);
        }
        #region Load Command
        RelayCommand _loadCommand;
        private bool _canLoad = true;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new RelayCommand(this.Load, param => this.CanLoad);
                }
                return _loadCommand;
            }
        }
        public bool CanLoad
        {
            get
            {
                return _canLoad;
            }
        }
        public void Load(object parameter)
        {

        }
        #endregion
        #region Save Command
        RelayCommand _saveCommand;
        private bool _canSave = true;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(this.Save, param => this.CanSave);
                }
                return _saveCommand;
            }
        }
        public bool CanSave
        {
            get
            {
                return _canSave;
            }
        }
        public void Save(object parameter)
        {

        }
        #endregion
        #region Clear Command
        RelayCommand _clearCommand;
        private bool _canClear = true;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(this.Clear, param => this.CanClear);
                }
                return _clearCommand;
            }
        }
        public bool CanClear
        {
            get
            {
                return _canClear;
            }
        }
        public void Clear(object parameter)
        {
            // Reserve the back buffer for updates.
            WriteableBitmap.Lock();

            for (int i = 0; i < HeightInPixels; i++)
            {
                for (int j = 0; j < WidthInPixels; j++)
                {
                    SetPixel(j, i, Color.FromRgb(255, 255, 255));
                }
            }

            // Specify the area of the bitmap that changed.
            WriteableBitmap.AddDirtyRect(new Int32Rect(0, 0, WriteableBitmap.PixelWidth, WriteableBitmap.PixelHeight));

            // Release the back buffer and make it available for display.
            WriteableBitmap.Unlock();
        }
        #endregion
        #region Scale Command
        RelayCommand _scaleCommand;
        private bool _canScale = true;
        public ICommand ScaleCommand
        {
            get
            {
                if (_scaleCommand == null)
                {
                    _scaleCommand = new RelayCommand(this.Scale, param => this.CanScale);
                }
                return _scaleCommand;
            }
        }
        public bool CanScale
        {
            get
            {
                return _canScale;
            }
        }
        public void Scale(object parameter)
        {
            double d = double.Parse((string)parameter);
            this.WriteableBitmap = new WriteableBitmap(new TransformedBitmap(_writeableBitmap, new ScaleTransform(d, d)));

        }
        #endregion
        public ICommand BeginRender
        {
            get
            {
                return new RelayCommand((o) => {
                    _bitmapModel.BeginRender();
                });
            }
        }
        public ICommand EndRender
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    _bitmapModel.EndRender();
                });
            }
        }
        public void BeginEdit()
        {
            WriteableBitmap.Lock();
        }
        public void EndEdit()
        {
            WriteableBitmap.Unlock();
        }
        void SetPixel(int x, int y, Color c)
        {
            int posX = x * BytesPerPixel;
            int posY = y * Stride;
            unsafe
            {
                byte* backBuffer = (byte*)WriteableBitmap.BackBuffer;
                backBuffer[posY + posX] = c.R;
                backBuffer[posY + posX + 1] = c.G;
                backBuffer[posY + posX + 2] = c.B;
            }
        }
        public void DrawPixel(int column, int row)
        {
            // Reserve the back buffer for updates.
            WriteableBitmap.Lock();

            SetPixel(column, row, Color.FromRgb(0, 0, 0));

            // Specify the area of the bitmap that changed.
            WriteableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));

            // Release the back buffer and make it available for display.
            WriteableBitmap.Unlock();
        }
        public void ErasePixel(int column, int row)
        {
            byte[] ColorData = { 0, 0, 0, 0 }; // B G R

            // Reserve the back buffer for updates.
            WriteableBitmap.Lock();

            Int32Rect rect = new Int32Rect(
                    column,
                    row,
                    1,
                    1);

            WriteableBitmap.WritePixels(rect, ColorData, 4, 0);

            // Specify the area of the bitmap that changed.
            //??? do i need this here or does the writepixels procedure do it?
            WriteableBitmap.AddDirtyRect(rect);

            // Release the back buffer and make it available for display.
            WriteableBitmap.Unlock();
        }
    }
}
