using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Scripts
{
    public class Loader
    {
        public string Name { get; set; }
        protected Dictionary<string, Action> _actions = new Dictionary<string, Action>();
        public Loader()
        {

        }
        public void Add(string name, Action action) { _actions.Add(name, action); }
        public int Load(string name)
        {
            _actions[name]();
            return 0;
        }
        public int LoadAll()
        {
            foreach (Action action in _actions.Values)
            {
                action();
            }
            return 0;
        }
    }
}
