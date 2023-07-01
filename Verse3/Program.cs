using Core;
using HandyControl.Tools;
using NamedPipeWrapper;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Verse3
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            

            // Define global culture information for reliable double conversions
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            // Set default handy control language to english
            ConfigHelper.Instance.Lang = System.Windows.Markup.XmlLanguage.GetLanguage("en");
            //ConfigHelper.Instance.SetLang("en");


            MainApplication app = new MainApplication();
            app.Run();
        }

    }
}
