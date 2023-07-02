using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Verse3.Assemblies
{
    public class AssemblyViewModel
        : ViewModelBase
    {
        protected readonly Assembly _assembly;
        public string Name { get { return _assembly.GetName().Name; } }
        public string FullName { get { return _assembly.FullName; } }
        public string Version { get { return _assembly.GetName().Version.ToString(); } }
        public string ImageRuntimeVersion { get { return _assembly.ImageRuntimeVersion; } }
        public AssemblyViewModel(string displayName, Assembly assembly)
            : base(displayName)
        {
            _assembly = assembly;
        }
    }
}
