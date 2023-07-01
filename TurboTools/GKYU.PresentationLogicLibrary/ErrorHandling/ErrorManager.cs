using GKYU.CoreLogicLibrary.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GKYU.PresentationLogicLibrary.ErrorHandling
{
    public class ErrorHandler
    {
        public void Execute(ErrorViewModel error)
        {
            error.Progress = 5;

            error.Progress = 100;
            error.Handled = true;
        }
    }
    public static class ErrorManager
    {
        public static ObservableCollection<ErrorContext> ErrorContexts = new ObservableCollection<ErrorContext>();
        public static ObservableCollection<ErrorViewModel> Errors = new ObservableCollection<ErrorViewModel>();
        public static Dictionary<string, Func<int, ErrorViewModel.CODE>> ErrorTranslators = new Dictionary<string, Func<int, ErrorViewModel.CODE>>();
        public static Dictionary<string,Dictionary<ErrorViewModel.CODE,ErrorHandler>> ErrorHandlers = new Dictionary<string,Dictionary<ErrorViewModel.CODE,ErrorHandler>>();
        public static event Func<ErrorViewModel, bool> OnBeforeError;
        public static event Func<ErrorViewModel, bool> OnAfterError;

        static ErrorManager()
        {
        }
        public static ErrorContext RegisterErrorContext(string name)
        {
            ErrorContext errorContext = null;
            ErrorContexts.Add(errorContext = new ErrorContext() { Name = name });
            ErrorHandlers.Add(name, new Dictionary<ErrorViewModel.CODE, ErrorHandler>());
            return errorContext;
        }
        public static ErrorHandler RegisterErrorHandler(string context, ErrorViewModel.CODE errorCode, Func<ErrorViewModel.CODE,string,ErrorViewModel.CODE> func)
        {
            ErrorHandler errorHandler = null;
            ErrorHandlers[context].Add(errorCode, errorHandler = new ErrorHandler());
            return errorHandler;
        }
        public static ErrorViewModel Error(string source, object sourceObject, ErrorViewModel.CODE errorCode, int severity, string errorMessage, bool waitForHandler = true)
        {
            ErrorViewModel error;
            Errors.Add(error = ErrorHandling.ErrorViewModel.Report(sourceObject, errorCode, errorMessage));
            if (null != OnBeforeError)
                OnBeforeError(error);
            if(ErrorHandlers[source][errorCode] != null)
            {
                ErrorHandlers[source][errorCode].Execute(error);
            }
            if (null != OnAfterError)
                OnAfterError(error);
            return error;
        }
    }
}
