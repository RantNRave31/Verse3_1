using Core;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Verse3.Components;

namespace Verse3.Converters
{
    internal class BaseCompConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new JsonBaseCompClassConverter(), ReferenceLoopHandling = ReferenceLoopHandling.Serialize };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BaseCompViewModel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            try
            {
                object bc = JsonConvert.DeserializeObject<ShellComp>(jo.ToString(), SpecifiedSubclassConversion);
                if (jo["MetadataCompInfo"] != null)
                {
                    JToken mci = jo["MetadataCompInfo"];
                    CompInfo mdCompInfo = JsonConvert.DeserializeObject<CompInfo>(mci.ToString());
                    if (mci != null)
                    {
                        //CompInfo ci = Main_Verse3.ActiveMain.ActiveEditor.FindInArsenal(mci);
                        CompInfo ci = MainWindowViewModel.ActiveMain.MainWindowViewModel.FindInArsenal(mdCompInfo);
                        if (ci.ConstructorInfo != null)
                        {
                            if (bc != null && bc is ShellComp shell)
                            {
                                if (shell._info != null)
                                {
                                    //ConstructorInfo ctorInfo = DataViewModel.GetDeserializationCtor(ci);
                                    //if (ctorInfo != null)
                                    //{
                                    BaseCompViewModel actual = ci.ConstructorInfo.Invoke(new object[] { (int)shell.X, (int)shell.Y }) as BaseCompViewModel;
                                    if (actual != null)
                                    {
                                        shell.ApplyObjectData(ref actual);
                                        bc = actual;
                                    }
                                    else
                                    {
                                        bc = ci.ConstructorInfo.Invoke(new object[] { (int)shell.X, (int)shell.Y }) as BaseCompViewModel;
                                        CoreConsole.Log("Information lost, creating blank from arsenal");
                                    }
                                    //}
                                    //else
                                    //{
                                    //    bc = ci.ConstructorInfo.Invoke(new object[] { }) as BaseComp;
                                    //    CoreConsole.Log("Information lost, creating blank from arsenal");
                                    //}
                                }
                                else
                                {
                                    bc = ci.ConstructorInfo.Invoke(new object[] { (int)shell.X, (int)shell.Y }) as BaseCompViewModel;
                                    CoreConsole.Log("Information lost, creating blank from arsenal");
                                }
                            }
                            else
                            {
                                bc = ci.ConstructorInfo.Invoke(new object[] { }) as BaseCompViewModel;
                                CoreConsole.Log("Information lost, creating blank from arsenal");
                            }
                        }
                        else
                        {
                            CoreConsole.Log("Comp not found in arsenal", true);
                        }
                    }
                }
                return bc;
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                return null;
                //CoreConsole.Log(ex);
            }
            //switch (jo["ObjType"].Value<int>())
            //{
            //    case 1:
            //        return JsonConvert.DeserializeObject<DerivedType1>(jo.ToString(), SpecifiedSubclassConversion);
            //    case 2:
            //        return JsonConvert.DeserializeObject<DerivedType2>(jo.ToString(), SpecifiedSubclassConversion);
            //    default:
            //        throw new Exception();
            //}
            //throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

}
