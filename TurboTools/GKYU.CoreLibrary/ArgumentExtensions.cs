using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary
{
    public static class ArgumentExtensions
    {
        public static KeyValuePair<string, string> ParseArgument(this string argument)
        {
            string[] fields = argument.Split('=');
            return new KeyValuePair<string, string>(fields[0].Trim(), fields[1].Trim());
        }
    }
}
