using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace GKYU.BusinessLogicLibrary.Bitmaps
{
    public class BitmapFileModel
        : ImageFileModel
    {
        public class Factory
        {
            private int nBitmaps;
            private int nMaxBitmaps;

            public Factory(int idSeed = 0, int idMax = 65535)
            {
                nBitmaps = idSeed;
                nMaxBitmaps = idMax;
            }
            private int GetNextBitmapID() { if (nBitmaps < nMaxBitmaps) return nBitmaps++; else return -1; }
            public BitmapFileModel Bitmap(int width, int height, int dpiHorizontal, int dpiVertical, PixelFormat pixelFormat)
            {
                return new BitmapFileModel(GetNextBitmapID(), width, height, dpiHorizontal, dpiVertical, pixelFormat);
            }
        }

        public int ID { get; set; }
        public WriteableBitmap mWriteableBitmap;
        private int mBytesPerPixel = 0;
        private int mStride = 0;

        public BitmapFileModel(string fileName)
            : base(fileName)
        {

        }
        public BitmapFileModel(int id, int width, int height, int dpiHorizontal, int dpiVertical, PixelFormat pixelFormat)
            : base(width, height)
        {
            ID = id;
            mWriteableBitmap = new WriteableBitmap(width, height, dpiHorizontal, dpiVertical, PixelFormats.Rgb24, null);
            mBytesPerPixel = (mWriteableBitmap.Format.BitsPerPixel + 7) / 8;
            mStride = mWriteableBitmap.PixelWidth * mBytesPerPixel;
        }

        public void BeginRender()
        {
            mWriteableBitmap.Lock();
        }
        public void EndRender()
        {
            mWriteableBitmap.Unlock();
        }
        void SetPixel(int x, int y, Color c)
        {
            int posX = x * mBytesPerPixel;
            int posY = y * mStride;
            unsafe
            {
                byte* backBuffer = (byte*)mWriteableBitmap.BackBuffer;
                backBuffer[posY + posX] = c.R;
                backBuffer[posY + posX + 1] = c.G;
                backBuffer[posY + posX + 2] = c.B;
            }
        }
        void ClearScreen()
        {
            int totalBytes = mStride * HeightInPixels;

            unsafe
            {
                byte* backBuffer = (byte*)mWriteableBitmap.BackBuffer;
                for (int i = 0; i < totalBytes; i++)
                {
                    backBuffer[i] = 255;
                }
            }
        }
        public void Clear()
        {
            // Reserve the back buffer for updates.
            mWriteableBitmap.Lock();

            for (int i = 0; i < HeightInPixels; i++)
            {
                for (int j = 0; j < WidthInPixels; j++)
                {
                    SetPixel(j, i, Color.FromRgb(255, 255, 255));
                }
            }

            // Specify the area of the bitmap that changed.
            mWriteableBitmap.AddDirtyRect(new Int32Rect(0, 0, mWriteableBitmap.PixelWidth, mWriteableBitmap.PixelHeight));

            // Release the back buffer and make it available for display.
            mWriteableBitmap.Unlock();

        }
        public void DrawPixel(int column, int row)
        {
            // Reserve the back buffer for updates.
            mWriteableBitmap.Lock();

            SetPixel(column, row, Color.FromRgb(0, 0, 0));

            // Specify the area of the bitmap that changed.
            mWriteableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));

            // Release the back buffer and make it available for display.
            mWriteableBitmap.Unlock();
        }

        public void ErasePixel(int column, int row)
        {
            byte[] ColorData = { 0, 0, 0, 0 }; // B G R

            // Reserve the back buffer for updates.
            mWriteableBitmap.Lock();

            Int32Rect rect = new Int32Rect(
                    column,
                    row,
                    1,
                    1);

            mWriteableBitmap.WritePixels(rect, ColorData, 4, 0);

            // Specify the area of the bitmap that changed.
            //??? do i need this here or does the writepixels procedure do it?
            mWriteableBitmap.AddDirtyRect(rect);

            // Release the back buffer and make it available for display.
            mWriteableBitmap.Unlock();
        }
        public void Save()
        {

        }
    }
}
