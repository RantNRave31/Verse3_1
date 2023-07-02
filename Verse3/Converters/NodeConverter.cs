using Core;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Verse3.Nodes;
using Core.Nodes;

namespace Verse3.Converters
{
    internal class NodeConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new JsonNodeClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return typeof(INode).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            try
            {
                ShellNode bc = JsonConvert.DeserializeObject<ShellNode>(jo.ToString(), SpecifiedSubclassConversion);

                return bc;
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                return null;
                //CoreConsole.Log(ex);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

}
