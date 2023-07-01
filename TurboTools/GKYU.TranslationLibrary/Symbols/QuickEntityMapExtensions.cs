using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Symbols
{
    public static class QuickEntityExtensions
    {//protected Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>
        public static string[] NamespaceNames(this Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap)
        {
            List<string> list = new List<string>(entityMap.Keys);
            return list.ToArray();
        }
        public static Dictionary<string, List<Tuple<string, string>>> Classes(this Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap, string namespaceName)
        {
            return entityMap[namespaceName];
        }
        public static string[] ClassNames(this Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap, string namespaceName, string className)
        {
            List<string> list = new List<string>(entityMap[namespaceName].Keys);
            return list.ToArray();
        }
        public static List<Tuple<string, string>> MembersOf(this Dictionary<string, List<Tuple<string, string>>> memberMap, string className)
        {
            return memberMap[className];
        }
        public static Tuple<string, string> MemberOf(this Dictionary<string, List<Tuple<string, string>>> memberMap, string className, string memberName)
        {
            return memberMap[className].Where(x => x.Item2 == memberName).FirstOrDefault();
        }
        public static string[] MemberNames(this Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap, string namespaceName, string className)
        {
            List<string> list = new List<string>();
            foreach (Tuple<string, string> member in entityMap[namespaceName][className])
            {
                list.Add(member.Item2);
            }
            return list.ToArray();
        }
        public static string[] MemberPairVector(this Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap, string namespaceName, string className)
        {
            List<string> list = new List<string>();
            foreach (Tuple<string, string> member in entityMap[namespaceName][className])
            {
                list.Add(member.Item2);
                list.Add(member.Item1);
            }
            return list.ToArray();
        }
    }
}
