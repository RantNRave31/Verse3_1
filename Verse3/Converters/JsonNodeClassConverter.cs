using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Core.Nodes;

namespace Verse3.Converters
{
    internal class JsonNodeClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(INode).IsAssignableFrom(objectType) && !objectType.IsAbstract) return null;
            return base.ResolveContractConverter(objectType);
        }
    }

}
