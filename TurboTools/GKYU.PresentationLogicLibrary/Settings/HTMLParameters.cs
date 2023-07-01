using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.PresentationLogicLibrary.Settings
{
    public class HTMLParameters
    {
        public static StringBuilder Parameter(string name, string value)
        {
            StringBuilder result = new StringBuilder();
            result.Append(name);
            result.Append('=');
            result.Append(value);
            return result;
        }
        public static void AddParameter(StringBuilder parameters, string name, string value)
        {
            StringBuilder parameter = Parameter(name, value);
            if (parameters.Length > 0)
            {
                parameters.Append('&');
            }
            parameters.Append(parameter);
        }
        public static StringBuilder AddParameters(IEnumerable<Tuple<string, string>> parameters)
        {
            StringBuilder result = new StringBuilder();
            foreach (Tuple<string, string> parameter in parameters)
            {
                if (parameter.Item2 != null && parameter.Item2 != string.Empty)
                    AddParameter(result, parameter.Item1, parameter.Item2);
            }
            return result;
        }
    }
}
