using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Verse3.Components;

namespace Verse3.Converters
{
    internal class JsonBaseCompClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(BaseCompViewModel).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

}
