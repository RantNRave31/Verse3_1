using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.BusinessLogicLibrary.Bitmaps
{
    public class ImageFileModel
        : FileModel
    {
        private int _widthInPixels;
        public int WidthInPixels
        {
            get
            {
                return _widthInPixels;
            }
            set
            {
                if (value != _widthInPixels)
                {
                    _widthInPixels = value;
                    NotifyPropertyChanged();
                }

            }
        }
        private int _heightInPixels;
        public int HeightInPixels
        {
            get
            {
                return _heightInPixels;
            }
            set
            {
                if (value != _heightInPixels)
                {
                    _heightInPixels = value;
                    NotifyPropertyChanged();
                }

            }
        }

        public ImageFileModel(string fileName)
            : base(fileName)
        {

        }
        public ImageFileModel()
            : this("New Image File.ext")
        {

        }
        public ImageFileModel(int widthInPixels, int heightInPixels)
            : this("New Image File.ext")
        {
            _widthInPixels = widthInPixels;
            _heightInPixels = heightInPixels;
        }
    }
}
