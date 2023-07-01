using Core;
using Verse3.Feedback;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Verse3.About;

namespace Verse3
{
    /// <summary>
    /// Interaction logic for NewMainWindowViewModel.xaml
    /// </summary>
    public partial class MainWindowModelView : Window
    {
        public MainWindowViewModel MainWindowViewModel { get { return ((MainWindowViewModel)DataContext); } }
        public MainWindowModelView()
        {
            InitializeComponent();
            //Open the Registry Key HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION
            const string featureBrowserEmulation =
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(featureBrowserEmulation, true))
            {
                if (key != null)
                {
                    //create a DWORD value with the name of application executable and value 11001
                    string appName = Process.GetCurrentProcess().ProcessName + ".exe";
                    if (key.GetValue(appName) == null)
                    {
                        key.SetValue(appName, 11001, RegistryValueKind.DWord);
                    }
                }
            }

            //Copy the contents of MonacoEditor Folder to AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string monacoEditorPath = System.IO.Path.Combine(appDataPath, "Verse3\\MonacoEditor");
            if (!Directory.Exists(monacoEditorPath))
            {
                Directory.CreateDirectory(monacoEditorPath);
                string[] files = Directory.GetFiles(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MonacoEditor"));
                foreach (string file in files)
                {
                    File.Copy(file, System.IO.Path.Combine(monacoEditorPath, System.IO.Path.GetFileName(file)));
                }
            }
            else
            {
                //if the files already exist, check if they are old and overwrite them
                string[] files = Directory.GetFiles(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MonacoEditor"));
                foreach (string file in files)
                {
                    if (File.Exists(System.IO.Path.Combine(monacoEditorPath, System.IO.Path.GetFileName(file))))
                    {
                        //compare contents of existing file to new file
                        string existingFileContents = File.ReadAllText(System.IO.Path.Combine(monacoEditorPath, System.IO.Path.GetFileName(file)));
                        string newFileContents = File.ReadAllText(file);
                        if (existingFileContents != newFileContents)
                        {
                            File.Copy(file, System.IO.Path.Combine(monacoEditorPath, System.IO.Path.GetFileName(file)), true);
                        }
                    }
                }
            }

            //const string currentUserSubKey =
            //@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            //using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(currentUserSubKey, false))
            //{
            //    string progId = (userChoiceKey.GetValue("ProgId").ToString());
            //    using (RegistryKey kp =
            //           Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command", false))
            //    {
            //        // Get default value and convert to EXE path.
            //        // It's stored as:
            //        //    "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
            //        // So we want the first quoted string only
            //        string rawValue = (string)kp.GetValue("");
            //        Regex reg = new Regex("(?<=\").*?(?=\")");
            //        Match m = reg.Match(rawValue);
            //        return m.Success ? m.Value : "";
            //    }
            //}
        }
        private void OpenFile(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.ShowNewForm(sender, e);
            MainWindowViewModel.MainWindowModelView = this;
            MainWindowViewModel.SelectedDataViewModel = (DataViewModel)DataViewModel.DataModel;
            MainWindowViewModel.SelectedDataModelView = new DataModelView();
            MainWindowViewModel.SelectedDataModelView.DataContext = DataViewModel.DataModel;
            tableLayoutPanel1.Content = MainWindowViewModel.SelectedDataModelView;
            MainWindowViewModel.SelectedDataModelView.MouseDown += Canvas_MouseDown;
            MainWindowViewModel.SelectedDataModelView.MouseUp += Canvas_MouseUp;
            MainWindowViewModel.SelectedDataModelView.MouseMove += Canvas_MouseMove;
            //InfiniteCanvasWPFControl.MouseMove += AddToCanvas_OnCall;
            MainWindowViewModel.SelectedDataModelView.Loaded += MainWindowViewModel.LoadLibraries;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MainWindowViewModel.SelectedMainWindowModelView is MainWindowModelView)
            {
                this.MainWindowViewModel.FramesPerSecond = MainWindowViewModel.SelectedDataModelView.AverageFPS.ToString();
                this.MainWindowViewModel.Status = MainWindowViewModel.SelectedDataModelView.GetMouseRelPosition().ToString();
            }
            else
            {
                if (loggedIn)
                {
                    if (Core.Core.GetUser() != null)
                    {
                        this.MainWindowViewModel.MainWindowViewModels.Add(new MainWindowViewModel(Core.Core.GetUser().Email));
                    }
                }
            }
            //label1.Text = InfiniteCanvasWPFControl.AverageFPS.ToString();
            //label2.Text = InfiniteCanvasWPFControl.GetMouseRelPosition().ToString();
        }
        private static string GetStandardBrowserPath()
        {
            const string currentUserSubKey =
            @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(currentUserSubKey, false))
            {
                string progId = (userChoiceKey.GetValue("ProgId").ToString());
                using (RegistryKey kp =
                       Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command", false))
                {
                    // Get default value and convert to EXE path.
                    // It's stored as:
                    //    "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                    // So we want the first quoted string only
                    string rawValue = (string)kp.GetValue("");
                    Regex reg = new Regex("(?<=\").*?(?=\")");
                    Match m = reg.Match(rawValue);
                    return m.Success ? m.Value : "";
                }
            }
        }
        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Core.Core.GetUser() == null)
            {
                string uri = await Core.Core.Login();

                //Supabase.Gotrue.Session s = await Core.Core.SetAuth(uri);

                //EmbeddedWPFBrowser browser = new EmbeddedWPFBrowser(uri);
                //if (browser.ShowDialog() == true)
                //{
                //    Core.Core.SetAuth(browser.token);
                //}

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = GetStandardBrowserPath();
                p.StartInfo.Arguments = uri;
                //p.OutputDataReceived += P_OutputDataReceived;
                p.Start();
                Core.Core.UserLoggedIn += OnUserLoggedIn;
                //p.BeginOutputReadLine();
                //while (!p.StandardOutput.EndOfStream)
                //{
                //    string t = p.StandardOutput.ReadLine();
                //    Core.Core.SetAuth(t);
                //}
                //System.Diagnostics.Process.Start("iexplore.exe", uri);
                //await p.WaitForExitAsync();
                //if (p.HasExited)
                //{
                //    MessageBox.Show(Core.Core.GetUser().Email);
                //}
            }
            else
            {
                //TODO:Splash screen?
                //Form childForm = new Form1();
                //childForm.MdiParent = this;
                //childForm.Text = "Window " + childFormNumber++;
                //childForm.Show();
                //MessageBox.Show("Logged in as: " + Core.Core.GetUser().Email);
                //Core.Core.Logout();
            }
        }
        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Core.Core.GetUser() != null)
            {
                //TODO:Splash screen?
                ((MainWindowViewModel)DataContext).MainWindowViewModels.Add(new MainWindowViewModel(Core.Core.GetUser().Email));
            }
            else
            {
                string uri = await Core.Core.Login();
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = GetStandardBrowserPath();
                p.StartInfo.Arguments = uri;
                p.Start();
                Core.Core.UserLoggedIn += OnUserLoggedIn;
            }
        }
        bool loggedIn = false;
        public void OnUserLoggedIn(object sender, EventArgs e)
        {
            loggedIn = true;
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutModelView about = new AboutModelView();
            about.ShowDialog();
        }

        //List<EmbeddedIDE> ides = new List<EmbeddedIDE>();

        //private void toolStripButton3_Click(object sender, EventArgs e)
        //{
        //EmbeddedIDE ide = new EmbeddedIDE();
        //ides.Add(ide);
        //ide.Show();
        //}

        //private void toolStripButton4_Click(object sender, EventArgs e)
        //{
        //if (ides.Count == 1)
        //{
        //    ides[0].GetScript();
        //}
        //}

        //private async void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    if (e.Data is string)
        //    {
        //        string t = e.Data as string;
        //        Supabase.Gotrue.Session s = await Core.Core.SetAuth(t);
        //    }
        //}
        private void loadLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((MainWindowViewModel)DataContext).HotLoadLibrary(openFileDialog.FileName);
            }
        }
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                this.Cursor = infiniteCanvas.Cursor;
            }
            if (this.MainWindowViewModel.SelectedDataViewModel.SelectedConnection != default)
            {
                //DataViewModel.ActiveConnection.Destination.
            }
            if (MainWindowViewModel.compsPendingInst.Count > 0)
            {
                ((MainWindowViewModel)DataContext).AddToCanvas_OnCall(sender, e);
            }
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                if (infiniteCanvas.MouseHandlingMode == MouseHandlingMode.None)
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
            //if (DataViewModel.ActiveNode != default /*&& started*/)
            //{
            //    //DrawBezierCurve(drawstart, InfiniteCanvasWPFControl.GetMouseRelPosition(), rtl);

            //    if (DataViewModel.ActiveConnection == default)
            //    {
            //        DataViewModel.ActiveConnection = CreateConnection(DataViewModel.ActiveNode);
            //    }
            //    else
            //    {
            //        ((BezierElement)DataViewModel.ActiveConnection).SetDestination(DataViewModel.ActiveNode);
            //        DataViewModel.ActiveConnection = default;
            //        DataViewModel.ActiveNode = default;
            //    }
            //    //started = false;
            //}
        }

        //public INode drawstart = default;
        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                if (infiniteCanvas.MouseHandlingMode == MouseHandlingMode.Panning)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                //if (started)
                //{
                //}
            }
        }






        // 
        // tabControl1
        // 
        //this.tabControl1.Controls.Add(this.tabPage1);

        // 
        // tabPage1
        // 
        //this.tabPage1.Controls.Add(this.flowLayoutPanel1);
        //this.tabPage1.Location = new System.Drawing.Point(4, 24);
        //this.tabPage1.Name = "tabPage1";
        //this.tabPage1.Size = new System.Drawing.Size(1005, 116);
        //this.tabPage1.TabIndex = 0;
        //this.tabPage1.Text = "tabPage1";
        //this.tabPage1.UseVisualStyleBackColor = true;
        // 
        // flowLayoutPanel1
        // 
        //this.flowLayoutPanel1.Controls.Add(this.groupBox1);
        //this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        //this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        //this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
        //this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        //this.flowLayoutPanel1.Size = new System.Drawing.Size(1005, 116);
        //this.flowLayoutPanel1.TabIndex = 0;

        // 
        // groupBox1
        // 
        //this.groupBox1.AutoSize = true;
        //this.groupBox1.Controls.Add(this.flowLayoutPanel2);
        //this.groupBox1.Location = new System.Drawing.Point(3, 3);
        //this.groupBox1.MinimumSize = new System.Drawing.Size(100, 100);
        //this.groupBox1.Name = "groupBox1";
        //this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
        //this.groupBox1.Size = new System.Drawing.Size(100, 100);
        //this.groupBox1.TabIndex = 0;
        //this.groupBox1.TabStop = false;
        //this.groupBox1.Text = "groupBox1";
        // 
        // flowLayoutPanel2
        // 
        //this.flowLayoutPanel2.Controls.Add(this.button1);
        //this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        //this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 16);
        //this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
        //this.flowLayoutPanel2.Name = "flowLayoutPanel2";
        //this.flowLayoutPanel2.Size = new System.Drawing.Size(100, 84);
        //this.flowLayoutPanel2.TabIndex = 0;

        // 
        // button1
        // 
        //this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        //this.button1.Location = new System.Drawing.Point(3, 3);
        //this.button1.Name = "button1";
        //this.button1.Size = new System.Drawing.Size(30, 30);
        //this.button1.TabIndex = 0;
        //this.button1.UseVisualStyleBackColor = true;

        private void exportCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)tableLayoutPanel1.RenderSize.Width, (int)tableLayoutPanel1.RenderSize.Height);
            //TODO:  finish this to export to bitmap
            //tableLayoutPanel1.DrawToBitmap(bmp, tableLayoutPanel1.RenderSize.Bounds);
            //save bmp to AppData/CanvasExports
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Verse3\\CanvasExports\\");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            {
                bmp.Save(System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmss") + "_ProjectVerseCanvasExport.png"));
            }

            //Try serializing the script to a json string

            //Show the feedback dialog
            FeedbackModelView ff = new FeedbackModelView();
            if (ff.ShowDialog() == true)
            {

            }
        }

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (Core.Core.GetUser() != null)
            {
                Core.Core.Logout();
            }
        }

        private void Form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VFSerializable VFfile = new VFSerializable((DataViewModel)this.MainWindowViewModel.SelectedDataViewModel);
            //show a save file dialog with default file extension *.vf or *.vfx
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Verse3 JSON File (*.vfj)|*.vfj|Verse3 File Extended (*.vfx)|*.vfx|Verse3 File (*.vf)|*.vf";
            saveFileDialog.DefaultExt = "vfj";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //save the file
                if (saveFileDialog.FileName.EndsWith(".vf"))
                {
                    VFfile.Serialize(saveFileDialog.FileName);
                }
                else if (saveFileDialog.FileName.EndsWith(".vfx"))
                {
                    //Serialize to xml
                    string xml = VFfile.ToXMLString();
                    File.WriteAllText(saveFileDialog.FileName, xml);
                }
                else if (saveFileDialog.FileName.EndsWith(".vfj") || saveFileDialog.FileName.EndsWith(".vfj.json"))
                {
                    //Serialize to xml
                    string xml = VFfile.ToJSONString();
                    File.WriteAllText(saveFileDialog.FileName, xml);
                }
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show an open file dialog to pick a *.vf or *.vfx file
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Verse3 JSON File (*.vfj)|*.vfj|Verse3 File Extended (*.vfx)|*.vfx|Verse3 File (*.vf)|*.vf";
            openFileDialog.DefaultExt = "vfj";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //load the file
                try
                {
                    if (openFileDialog.FileName.EndsWith(".vf"))
                    {
                        VFSerializable VFfile = VFSerializable.Deserialize(openFileDialog.FileName);
                        DataViewModel.DataModel = VFfile.DataViewModel;
                        this.MainWindowViewModel.SelectedDataModelView.ExpandContent();
                    }
                    else if (openFileDialog.FileName.EndsWith(".vfx"))
                    {
                        VFSerializable VFfile = VFSerializable.DeserializeXML(openFileDialog.FileName);
                        DataViewModel.DataModel = VFfile.DataViewModel;
                        this.MainWindowViewModel.SelectedDataViewModel.DataModelView.ExpandContent();
                    }
                    else if (openFileDialog.FileName.EndsWith(".vfj"))
                    {
                        VFSerializable VFfile = VFSerializable.DeserializeJSON(openFileDialog.FileName);
                        DataViewModel.DataModel = VFfile.DataViewModel;
                        this.MainWindowViewModel.SelectedDataViewModel.DataModelView.ExpandContent();
                    }
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
        }

        private void exportCanvasToImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)tableLayoutPanel1.RenderSize.Width, (int)tableLayoutPanel1.RenderSize.Height);
            // TODO:  canvas to image
            //tableLayoutPanel1.DrawToBitmap(bmp, tableLayoutPanel1.Bounds);
            //save bmp to AppData/CanvasExports
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Verse3\\CanvasExports\\");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            {
                bmp.Save(System.IO.Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmss") + "_ProjectVerseCanvasExport.png"));
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
