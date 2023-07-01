using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.IO;
using Core.Assemblies;

namespace Verse3.Assemblies
{
    public class AssemblyManagerViewModel
        : ViewModelBase
    {
        public ObservableCollection<AssemblyViewModel> Assemblies { get; set; }
        AssemblyViewModel _selectedAssembly;
        public AssemblyViewModel SelectedAssembly { get { return _selectedAssembly; } set { if (value == _selectedAssembly) return; _selectedAssembly = value; OnPropertyChanged(); } }
        public AssemblyManagerViewModel(string displayName)
            : base(displayName)
        {
            Assemblies = new ObservableCollection<AssemblyViewModel>();
            AssemblyLoader.OnAssemblyAdded += AssemblyLoader_OnAssemblyAdded;
        }

        private void AssemblyLoader_OnAssemblyAdded(System.Reflection.Assembly obj)
        {
            this.Assemblies.Add(new AssemblyViewModel(Path.GetFileNameWithoutExtension(obj.FullName), obj));
        }

        public IEnumerable<IElement> LoadLibrary(string path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                File.OpenRead(path).CopyTo(ms);
                var es = AssemblyLoader.Load(ms);
                return es;
            }
        }


    }
}
