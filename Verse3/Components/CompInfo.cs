using Core;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace Verse3.Components
{
    [Serializable] //READ ONLY SERIALIZATION
    public readonly struct CompInfo
    {
        public CompInfo(BaseCompViewModel comp, string name, string group, string tab, Color accent = default, Type[] ArgumentTypes = default)
        {
            ConstructorInfo = comp.GetType().GetConstructor(new Type[] { typeof(int), typeof(int) });
            if (ArgumentTypes != default)
            {
                ConstructorInfo = comp.GetType().GetConstructor(ArgumentTypes);
            }
            Name = name;
            Group = group;
            Tab = tab;
            Description = "DEVELOPMENT BUILD : Define values for all relevant properties before publishing";
            Author = "DEVELOPMENT_BUILD";
            Version = "0.0.0.1";
            License = "DEVELOPMENT BUILD : No warranties. Use at your own risk. Not cloud-safe";
            Website = "https://iiterate.de";
            Repository = "https://iiterate.de";
            Icon = null;
            if (accent == default)
            {
                Random rnd = new Random();
                byte rc = (byte)Math.Round(rnd.NextDouble() * 125.0);
                byte gc = (byte)Math.Round(rnd.NextDouble() * 125.0);
                byte bc = (byte)Math.Round(rnd.NextDouble() * 125.0);
                Accent = Color.FromRgb(rc, gc, bc);
            }
            else Accent = accent;
            TypeName = comp.GetType().FullName;
            BuiltAgainst = Assembly.GetExecutingAssembly().ImageRuntimeVersion;
            //IsValid = true;
            IsDevelopmentBuild = true;
        }
        public CompInfo(IRenderable comp, string name, string group, string tab)
        {
            ConstructorInfo = comp.GetType().GetConstructor(new Type[] { typeof(int), typeof(int) });
            Name = name;
            Group = group;
            Tab = tab;
            Description = "DEVELOPMENT BUILD : Define values for all relevant properties before publishing";
            Author = "DEVELOPMENT_BUILD";
            Version = "0.0.0.1";
            License = "DEVELOPMENT BUILD : No warranties. Use at your own risk. Not cloud-safe";
            Website = "https://iiterate.de";
            Repository = "https://iiterate.de";
            Icon = null;
            TypeName = comp.GetType().FullName;
            BuiltAgainst = Assembly.GetExecutingAssembly().ImageRuntimeVersion;
            //IsValid = true;
            IsDevelopmentBuild = true;
        }
        public CompInfo(BaseCompViewModel comp, string name, string group, string tab, string description, string author, string version, string license, string website, string repository, BitmapSource icon, Color accent)
        {
            //if (comp is ShellComp) ConstructorInfo = comp.GetType().GetConstructor(new Type[] { typeof(SerializationInfo), typeof(StreamingContext) });
            /*else*/
            ConstructorInfo = comp.GetType().GetConstructor(new Type[] { typeof(int), typeof(int) });
            Name = name;
            Group = group;
            Tab = tab;
            Description = description;
            Author = author;
            Version = version;
            License = license;
            Website = website;
            Repository = repository;
            Icon = icon;
            Accent = accent;
            TypeName = comp.GetType().FullName;
            BuiltAgainst = Assembly.GetExecutingAssembly().ImageRuntimeVersion;
            //IsValid = true;
            IsDevelopmentBuild = false;
        }
        [JsonIgnore]
        public ConstructorInfo ConstructorInfo { get; init; }
        public string Name { get; init; }
        public string Group { get; init; }
        public string Tab { get; init; }
        public string Description { get; init; }
        public string Author { get; init; }
        public string Version { get; init; }
        public string License { get; init; }
        public string Website { get; init; }
        public string Repository { get; init; }
        public string TypeName { get; }
        public string BuiltAgainst { get; }
        public bool IsValid
        {
            get
            {
                if (ConstructorInfo == null) return false;
                if (Name == null) return false;
                if (Group == null) return false;
                if (Tab == null) return false;
                //if (Description == null) return false;
                //if (Author == null) return false;
                //if (Version == null) return false;
                //if (License == null) return false;
                //if (Website == null) return false;
                //if (Repository == null) return false;
                //if (TypeName == null) return false;
                //if (BuiltAgainst == null) return false;
                return true;
            }
        }
        public bool IsDevelopmentBuild { get; }

        //TODO: Try allowing SVGs as Icons
        [JsonIgnore]
        public BitmapSource Icon { get; init; }
        public Color Accent { get; init; }

        //public Type[] ConstructorParamTypes { get; set; }
        //public string[] ConstructorParamNames { get; set; }
        //public object[] ConstructorDefaults { get; set; }

        string ImageToBase64(BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder();
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        BitmapSource Base64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream);
            }
        }

        public override string ToString()
        {
            //Serialize CompInfo to JSON string
            return JsonConvert.SerializeObject(this);
        }

        internal static CompInfo FromString(BaseCompViewModel baseComp, string value)
        {
            //Deserialize JSON string to CompInfo
            if (value is null) return default;
            CompInfo compInfoDeserialized = JsonConvert.DeserializeObject<CompInfo>(value);

            CompInfo compInfoOut = new CompInfo(baseComp,
                compInfoDeserialized.Name,
                compInfoDeserialized.Group,
                compInfoDeserialized.Tab,
                compInfoDeserialized.Description,
                compInfoDeserialized.Author,
                compInfoDeserialized.Version,
                compInfoDeserialized.License,
                compInfoDeserialized.Website,
                compInfoDeserialized.Repository,
                null,
                compInfoDeserialized.Accent);

            return compInfoOut;
            //        if (ci.IsValid)
            //{
            //    if (ci.Name != GetCompInfo().Name)
            //    {
            //        _nameOverride = ci.Name;
            //        CompInfo.Inject(this);
            //    }
            //}
        }
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
