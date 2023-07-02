﻿using Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Verse3.Elements
{
    /// <summary>
    /// Interaction logic for IDEElement.xaml
    /// </summary>
    public partial class IDEElementModelView : UserControl, IBaseElementView<IDEElementViewModel>
    {
        #region IBaseElementView Members

        private IDEElementViewModel _element;
        public IDEElementViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    _element = this.DataContext as IDEElementViewModel;
                }
                return _element;
            }
            private set
            {
                _element = value as IDEElementViewModel;
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion

        #region Constructor and Render

        public IDEElementModelView()
        {
            InitializeComponent();
            EmulatedIDEBrowser.WebMessageReceived += WebMessageReceived;
            EmulatedIDEBrowser.NavigationCompleted += EmulatedIDEBrowser_NavigationCompleted;
            //Get path of MonacoEditor folder in AppData
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Verse3\\MonacoEditor";
            this.EmulatedIDEBrowser.Source = new Uri(System.IO.Path.Combine(path, "index.html"));
        }

        private void EmulatedIDEBrowser_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            SetScript("");
            string path1 = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Verse3", "DefaultScripts",
                "CSharpDefaultScript.cs");
            if (File.Exists(path1))
            {
                string script = File.ReadAllText(path1);
                SetScript(script);
            }
        }


        //Method to run a task asynchronously and call a method when it's done
        public async void CaptureBrowserPreview(/*System.IO.MemoryStream s*/)
        {
            //await this.EmulatedIDEBrowser.CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Png, s);

            string pic = await this.EmulatedIDEBrowser.CoreWebView2.CallDevToolsProtocolMethodAsync("Page.captureScreenshot", "{}");
            JObject o3 = JObject.Parse(pic);
            JToken data = o3["data"]!;

            byte[] bytes = Convert.FromBase64String(data.ToString());
            Image image = new Image();
            //double picHeight = 0d;
            //double picWidth = 0d;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BitmapFrame bitmap = System.Windows.Media.Imaging.BitmapDecoder.Create(stream, BitmapCreateOptions.None,
                                        BitmapCacheOption.OnLoad).Frames[0];
                //if (bitmap != null)
                //{
                //    stream.CopyTo(s);
                //}
                //picHeight = bitmap.Height;
                //picWidth = bitmap.Width;
                //image.Source = bitmap;
                
                //set bitmap as source
                //this.EmulatedIDEBrowser.Source = b;
                //Image image = new Image();
                //image.Source = bitmap;
                this.OverlayImage.Source = bitmap;
                this.OverlayImage.Visibility = Visibility.Visible;
                this.EmulatedIDEBrowser.IsEnabled = false;
                this.EmulatedIDEBrowser.Visibility = Visibility.Hidden;
            }
        }

        public void Render()
        {
            if (this.Element != null)
            {
                if (this.Element.RenderView != this) this.Element.RenderView = this;
                if (!this.EmulatedIDEBrowser.IsFocused && this.OverlayImage.Visibility == Visibility.Hidden)
                {
                    //create stream for bitmap
                    using (System.IO.MemoryStream s = new System.IO.MemoryStream())
                    {
                        if (this.EmulatedIDEBrowser.CoreWebView2 != null)
                        {
                            Type T = this.EmulatedIDEBrowser.DataContext.GetType();
                            CaptureBrowserPreview();
                            //Task t = this.EmulatedIDEBrowser.CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Png, s);
                            //t.ContinueWith(t => HideIDE(t, s));
                            //t.Wait();
                        }
                    }
                    //this.EmulatedIDEBrowser.IsEnabled = false;
                    //this.EmulatedIDEBrowser.Visibility = Visibility.Hidden;
                }
            }
        }

        #endregion


        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when the mouse is moved over a Rectangle.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        #endregion

        #region KeyboardEvents

        /// <summary>
        /// Event raised when a key is pressed down.
        /// </summary>
        void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when a key is released.
        /// </summary>
        void OnKeyUp(object sender, KeyEventArgs e)
        {
        }

        #endregion

        #region UserControlEvents

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //DependencyPropertyChangedEventArgs
            Element = this.DataContext as IDEElementViewModel;
            //if (this.Element.RenderView != this) this.Element.RenderView = this;
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Element = this.DataContext as IDEElementViewModel;
            //if (this.Element.RenderView != this) this.Element.RenderView = this;
            Render();
        }

        #endregion

        public void ExecuteJS(string js)
        {
            //start task of EmulatedIDEBrowser.ExecuteScriptAsync(js) in a parallel thread and wait for it to finish
            Task<string> task = EmulatedIDEBrowser.ExecuteScriptAsync(js)/*.ContinueWith(t => OnExecutionCompleted(t))*/;
            //task.Start();
            task.WaitAsync(new TimeSpan(5000)).ContinueWith(t => OnExecutionCompleted(t));
            //return task.Result;
        }

        public string OnExecutionCompleted(Task<string> t)
        {
            if (t.IsFaulted)
            {
                CoreConsole.Log(t.Exception.ToString());
                return null;
            }
            else
            {
                CoreConsole.Log(t.Result.ToString());
                this.Element._script = t.Result.ToString();
                return t.Result.ToString();
            }
        }

        public void GetScript()
        {
            ExecuteJS("getValue();");
        }

        private void WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            CoreConsole.Log(e.TryGetWebMessageAsString());
            this.Element._script = e.TryGetWebMessageAsString();
            this.Element.UpdateScript(e.TryGetWebMessageAsString());
        }

        private void OverlayImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.EmulatedIDEBrowser.IsEnabled)
            {
                this.OverlayImage.Source = null;
                this.OverlayImage.Visibility = Visibility.Hidden;
                this.EmulatedIDEBrowser.IsEnabled = true;
                this.EmulatedIDEBrowser.Visibility = Visibility.Visible;
                ArsenalViewModel.StaticSelectedDataViewModel.DataModelView.ClearSelection();
            }
        }

        private void EmulatedIDEBrowser_LostFocus(object sender, RoutedEventArgs e)
        {
            RenderingCore.Render(this.Element);
        }

        internal void SetScript(string v)
        {
            v = System.Web.HttpUtility.JavaScriptStringEncode(v);
            ExecuteJS("setValue(\"" + v + "\");");
        }
    }
}
