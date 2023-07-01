using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GKYU.CoreLogicLibrary.ErrorHandling;

namespace GKYU.TranslationLibrary.ErrorHandling
{
    public class ErrorHandler
    {
        public void Execute(Error error)
        {
            error.Progress = 5;

            error.Progress = 100;
            error.Handled = true;
        }
    }
    public static class ErrorManager
    {
        public static ObservableCollection<ErrorContext> ErrorContexts = new ObservableCollection<ErrorContext>();
        public static ObservableCollection<Error> Errors = new ObservableCollection<Error>();
        public static Dictionary<string, Func<int, Error.CODE>> ErrorTranslators = new Dictionary<string, Func<int, Error.CODE>>();
        public static Dictionary<string,Dictionary<Error.CODE,ErrorHandler>> ErrorHandlers = new Dictionary<string,Dictionary<Error.CODE,ErrorHandler>>();
        public static event Func<Error, bool> OnBeforeError;
        public static event Func<Error, bool> OnAfterError;

        static ErrorManager()
        {
        }
        public static ErrorContext RegisterErrorContext(string name)
        {
            ErrorContext errorContext = null;
            ErrorContexts.Add(errorContext = new ErrorContext() { Name = name });
            ErrorHandlers.Add(name, new Dictionary<Error.CODE, ErrorHandler>());
            return errorContext;
        }
        public static ErrorHandler RegisterErrorHandler(string context, Error.CODE errorCode, Func<Error.CODE,string, Error.CODE> func)
        {
            ErrorHandler errorHandler = null;
            ErrorHandlers[context].Add(errorCode, errorHandler = new ErrorHandler());
            return errorHandler;
        }
        public static Error Error(string source, object sourceObject, Error.CODE errorCode, int severity, string errorMessage, bool waitForHandler = true)
        {
            Error error;
            Errors.Add(error = ErrorHandling.Error.Report(sourceObject, errorCode, errorMessage));
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
