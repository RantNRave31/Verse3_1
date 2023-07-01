using Core;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SaveXML;
using Supabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static SaveXML.FileOperations;
using Newtonsoft.Json;
using Verse3.Components;
using Verse3.Converters;

namespace Verse3
{
    [Serializable]
    public class VFSerializable : XmlAttributesContainer, ISerializable
    {
        
        [XmlElement]
        public DataViewModel DataViewModel { get; set; }
        //[XmlIgnore]
        [JsonIgnore]
        public XmlSerializer XMLSerializer
        {
            get
            {

                Type[] LoadedComps = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                      from lType in a.GetTypes()
                                      where typeof(BaseCompViewModel).IsAssignableFrom(lType)
                                      select lType).ToArray();
                Type[] FilteredComps = (from lType in LoadedComps
                                        where lType.IsClass && !lType.IsAbstract
                                        select lType).ToArray();
                Dictionary<string, Type> UniqueComps = new Dictionary<string, Type>();
                foreach (Type lType in LoadedComps)
                {
                    if (!UniqueComps.Values.Contains(lType) && !UniqueComps.Keys.Contains(lType.FullName) && lType.FullName != "Verse3.BaseComp")
                    {
                        UniqueComps.Add(lType.FullName, lType);
                    }
                }
                //if (this.XMLAttributes == null) System.Diagnostics.Debug.WriteLine("XMLAttributes are null.");
                //XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataViewModel), this.XMLAttributes);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(BaseCompViewModel), this.XMLAttributes, UniqueComps.Values.ToArray(),
                    new XmlRootAttribute("BaseComp"), "http://www.w3.org/2001/XMLSchema-instance");
                return xmlSerializer;
            }
        }
        //[XmlIgnore]
        [JsonIgnore]
        public override XmlAttributeOverrides XMLAttributes
        {
            get
            {
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attributes = new XmlAttributes();

                foreach (BaseCompViewModel comp in DataViewModel.Comps)
                {
                    if (!attributes.XmlElements.Contains(new XmlElementAttribute(comp.GetType().FullName, comp.GetType())))
                    {
                        attributes.XmlElements.Add(new XmlElementAttribute(comp.GetType().FullName, comp.GetType()));
                    }
                }

                overrides.Add(typeof(BaseCompViewModel), attributes);
                return overrides;
            }
        }
        public string ToXMLString()
        {
            try
            {
                //if (this.XMLAttributes == null) throw new Exception("XMLAttributes is null.");
                if (this.XMLSerializer == null) throw new Exception("XMLSerializer is null.");

                StringBuilder sb = new StringBuilder();

                foreach (BaseCompViewModel comp in DataViewModel.Comps)
                {
                    XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document.

                    // Creates a stream whose backing store is memory. 
                    using (MemoryStream xmlStream = new MemoryStream())
                    {
                        this.XMLSerializer.Serialize(xmlStream, comp);
                        xmlStream.Position = 0;
                        //Loads the XML document from the specified string.
                        xmlDoc.Load(xmlStream);
                        sb.AppendLine(xmlDoc.InnerXml);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                return null;
            }
        }
        public VFSerializable(DataViewModel dataViewModel)
        {
            DataViewModel = dataViewModel;
        }
        public VFSerializable()
        {
        }

        public VFSerializable(SerializationInfo info, StreamingContext context)
        {
            if (info is null) throw new NullReferenceException("Null SerializationInfo");
            object dvm = info.GetValue("DataViewModel", typeof(DataViewModel));
            if (dvm != null && dvm is DataViewModel newDVM)
            {
                this.DataViewModel = newDVM;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DataViewModel", DataViewModel);
        }

        //public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("DataViewModel", DataViewModel);
        //}

        internal void Serialize(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                //CoreConsole.Log(ex);
                CoreConsole.Log(ex);
            }
            //finally
            //{
            //    try
            //    {
            //        Supabase.Gotrue.User user = Client.Instance.Auth.CurrentUser;
                    
            //        var file = ShellFile.FromFilePath(path);

            //        // Read and Write:

            //        //string[] oldAuthors = file.Properties.System.Author.Value;
            //        //string oldTitle = file.Properties.System.Title.Value;

            //        //file.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
            //        //file.Properties.System.Title.Value = "Example Title";

            //        // Alternate way to Write:

            //        ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter();

            //        string authorId = "DEVELOPER";
            //        if (user != null)
            //        {
            //            authorId = user.Id;
            //        }
            //        propertyWriter.WriteProperty(SystemProperties.System.Author, new string[] { "AuthorID::" + authorId });
            //        propertyWriter.Close();
            //    }
            //    catch (Exception ex1)
            //    {
            //        throw ex1;
            //    }
            //}
        }

        internal static VFSerializable Deserialize(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                    formatter.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                    //Supabase.Gotrue.User user = Client.Instance.Auth.CurrentUser;

                    //var file = ShellFile.FromFilePath(path);

                    //string oldAuthor = file.Properties.System.Author.Value[0];
                    //string authorId = "DEVELOPER";
                    //if (user != null)
                    //{
                    //    authorId = user.Id;
                    //}
                    //if (oldAuthor == ("AuthorID::" + authorId))
                    //{
                    //    System.Diagnostics.Debug.WriteLine("File created by " + oldAuthor);
                    //    if (authorId != "DEVELOPER") return null;
                    //}
                    return (VFSerializable)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                //CoreConsole.Log(ex);
                CoreConsole.Log(ex);
                return null;
            }
        }

        internal void SerializeXML(string fileName)
        {
            try
            {
                XMLFile file = new XMLFile(fileName);
                file.SetObject(this);
                RESPONSE_CODES resp = FileOperations.Save(file, true);
                if (resp == RESPONSE_CODES.SAVE_SUCCESS)
                {
                    Supabase.Gotrue.User user = Client.Instance.Auth.CurrentUser;

                    var file1 = ShellFile.FromFilePath(fileName);

                    // Read and Write:

                    //string[] oldAuthors = file.Properties.System.Author.Value;
                    //string oldTitle = file.Properties.System.Title.Value;

                    //file.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
                    //file.Properties.System.Title.Value = "Example Title";

                    // Alternate way to Write:

                    ShellPropertyWriter propertyWriter = file1.Properties.GetPropertyWriter();

                    string authorId = "DEVELOPER";
                    if (user != null)
                    {
                        authorId = user.Id;
                    }
                    propertyWriter.WriteProperty(SystemProperties.System.Author, new string[] { "AuthorID::" + authorId });
                    propertyWriter.Close();
                }
            }
            catch (Exception ex)
            {
                //CoreConsole.Log(ex);
                CoreConsole.Log(ex);
            }
        }

        internal static VFSerializable DeserializeXML(string fileName)
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    XMLFile file = FileOperations.Load(fileName);
                    if (file != null)
                    {
                        Supabase.Gotrue.User user = Client.Instance.Auth.CurrentUser;

                        var file1 = ShellFile.FromFilePath(fileName);

                        string oldAuthor = file1.Properties.System.Author.Value[0];
                        string authorId = "DEVELOPER";
                        if (user != null)
                        {
                            authorId = user.Id;
                        }
                        if (oldAuthor == ("AuthorID::" + authorId))
                        {
                            System.Diagnostics.Debug.WriteLine("File created by " + oldAuthor);
                            CoreConsole.Log("File created by " + oldAuthor);
                            if (authorId != "DEVELOPER") return null;
                        }
                        return (VFSerializable)file.GetObject();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //CoreConsole.Log(ex);
                CoreConsole.Log(ex);
                return null;
            }
        }

        internal string ToJSONString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new BaseCompConverter());
            settings.Converters.Add(new NodeConverter());
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            settings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        internal static VFSerializable DeserializeJSON(string filePath)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new BaseCompConverter());
            settings.Converters.Add(new NodeConverter());
            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            settings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            string text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<VFSerializable>(text, settings);
        }
    }

}
