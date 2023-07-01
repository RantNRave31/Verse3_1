using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GKYU.PresentationCoreLibrary.ViewModels
{
    public class CommandViewModel
        : ViewModelBase
    {
        public CommandViewModel(string displayName, ICommand command)
            : base(displayName)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            Command = command;
        }
        public ICommand Command { get; private set; }
    }
}
