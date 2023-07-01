using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Scripts
{
    public class Executor
    {
        public string Name { get; set; }
        protected Dictionary<string,Func<int>> _executions = new Dictionary<string, Func<int>>();
        public Executor()
        {

        }
        public void Add(string name, Func<int> execution) { _executions.Add(name, execution); }
        public int Execute(string name)
        {
            return _executions[name]();
        }
        public int ExecuteAll()
        {
            foreach (Func<int> execution in _executions.Values)
            {
                int returnCode = execution();
            }
            return 0;
        }
    }
}
