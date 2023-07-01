using GKYU.TranslationLibrary.Symbols;
using System;

namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Instance
            : Symbol
           , IEquatable<Instance>
        {
            public bool Equals(Instance other)
            {
                return base.Equals(other);
            }

        }
        public class Instance<T>
            : Instance
            , IEquatable<Instance<T>>
            where T : Syntax.TypeDeclaration
        {
            public T MakeType { get; set; }
            public bool Equals(Instance<T> other)
            {
                return base.Equals(other);
            }
        }

    }
}
