using GKYU.PresentationLogicLibrary.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
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

namespace GKYU.PresentationLogicLibrary.ModelViews
{
    /// <summary>
    /// Interaction logic for TouchTemplateModelView.xaml
    /// </summary>
    public partial class TouchTemplateModelView : UserControl
    {
        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

        [System.Flags]
        public enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004
        }

        private ObservableStyleDictionary _observableButtonStyles = null;
        public TouchTemplateModelView()
        {
            InitializeComponent();
            _observableButtonStyles = FindResource("ButtonStyles") as ObservableStyleDictionary;
            ReloadDictionary(null, null);
        }
        private void ReloadDictionary(object sender, RoutedEventArgs e)
        {
            // cache the selected index
            int index = StyleSelector.SelectedIndex;

            ResourceDictionary buttonStyles = Application.LoadComponent(
                new Uri("ButtonStyles.xaml", UriKind.Relative)) as ResourceDictionary;
            _observableButtonStyles.Clear();
            foreach (DictionaryEntry de in buttonStyles)
            {
                if (!(de.Value is Style)) continue;

                if (de.Key == typeof(Button))
                {
                    _observableButtonStyles["Default Style"] = de.Value as Style;
                }
                else if (de.Key is string)
                {
                    _observableButtonStyles[de.Key as string] = de.Value as Style;
                }
            }

            // restore the selected index, if still applicable; otherwise select the first index
            if (index > -1 && StyleSelector.Items.Count > index)
            {
                StyleSelector.SelectedIndex = index;
            }
            else
            {
                StyleSelector.SelectedIndex = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlaySound("Audio\\click.wav", new System.IntPtr(), PlaySoundFlags.SND_ASYNC);
        }
    }
}
