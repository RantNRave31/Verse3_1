using Core.Elements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Assemblies
{
    public static class AssemblyCompiler
    {
        #region Properties
        public static List<string> CompileLog { get; set; } = new List<string>();
        private static List<MetadataReference> references { get; set; } = new List<MetadataReference>();
        #endregion

        internal static void Init()
        {
            if (references == null || references.Count == 0)
            {
                references = new List<MetadataReference>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.IsDynamic)
                    {
                        continue;
                    }
                    var name = assembly.GetName().Name + ".dll";
                    CoreConsole.Log(name);
                    string loc = assembly.Location;
                    try
                    {
                        if (loc != string.Empty && File.Exists(loc))
                            references.Add(MetadataReference.CreateFromFile(loc));
                        else
                            CoreConsole.Log("Error loading assembly at :" + loc);
                    }
                    catch (Exception ex)
                    {
                        CoreConsole.Log(ex);
                    }
                }
            }
        }

        public static byte[] Compile(string code, string asmName)
        {
            Init();

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Preview));
            foreach (var diagnostic in syntaxTree.GetDiagnostics())
            {
                CompileLog.Add(diagnostic.ToString());
            }

            if (syntaxTree.GetDiagnostics().Any(i => i.Severity == DiagnosticSeverity.Error))
            {
                CompileLog.Add("Parse SyntaxTree Error!");
                return null;
            }

            CompileLog.Add("Parse SyntaxTree Success");

            CSharpCompilation compilation = CSharpCompilation.Create(asmName, new[] { syntaxTree },
                references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(stream);

                foreach (var diagnostic in result.Diagnostics)
                {
                    CompileLog.Add(diagnostic.ToString());
                }

                if (!result.Success)
                {
                    CompileLog.Add("Compilation error");
                    return null;
                }

                CompileLog.Add("Compilation success!");

                stream.Seek(0, SeekOrigin.Begin);

                //ERROR: Assembly loading on the wrong thread?
                //Assembly assembly = Assembly.Load(stream.ToArray());
                return stream.ToArray();
            }
        }

        internal static byte[] CompileOnly(string code)
        {
            Init();

            var assembly = Compile(code, "tmp_" + DateTime.UtcNow.ToFileTimeUtc().ToString());
            if (assembly != null)
            {
                return assembly;
            }

            return null;
        }

        internal static IElement CreateRunClass(Type type)
        {
            try
            {
                //TODO: TODO: IMPORTANT!! Implement parameter passing for parametric construction of elements at load time
                //Use case example: License key for a paid element in a library can be passed as a parameter and checked by the constructor on load
                var instance = Activator.CreateInstance(type) as IElement;
                if (instance != null)
                {
                    return instance;
                }
                return null;
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                return null;
            }
        }
    }
}
