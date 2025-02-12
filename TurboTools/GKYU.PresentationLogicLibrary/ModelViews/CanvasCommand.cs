﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GKYU.PresentationLogicLibrary.ModelViews
{
    public class CanvasCommand : ICommand
    {
        public event EventHandler<object> Executed;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Executed != null)
                Executed(this, parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
