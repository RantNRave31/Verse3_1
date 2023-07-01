using GKYU.CollectionsLibrary.Collections;
using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.IO;

namespace GKYU.TranslationLibrary.Patterns
{
    public abstract class CompilerBase
    {

        public delegate int TranslationFunction(object targetObject, object sourceObject);

        public Dictionary3D<string, TranslationFunction> _translationTableMap = new Dictionary3D<string, TranslationFunction>();
        protected MacroProcessor _macroProcessor { get; set; }
        protected TemplateProcessor _templateProcessor { get; set; }
        protected MakeProcessor _make { get; set; }
        public object this[string key]
        {
            get
            {
                return _macroProcessor[key].Object;
            }
            set
            {
                _macroProcessor[key].Object = value;
            }
        }
        static CompilerBase()
        {

        }
        public CompilerBase(MacroProcessor macroProcessor)
        {
            _templateProcessor = new TemplateProcessor(_macroProcessor = macroProcessor);
            _translationTableMap["Text File", "MacroProcessor", "Text File"] = (targetObject, sourceObject) =>
            {
                int returnCode = 0;
                PushContext();
                Console.WriteLine("Source File {0} Name {1}", this["SourceFileType"], sourceObject);
                Console.WriteLine("Target File {0} Name {1}", this["TargetFileType"], targetObject);
                _macroProcessor.Translate(
                    Path.Combine(this["SolutionFolder"].ToString(), this["ProjectFolder"].ToString(), (string)targetObject)
                    , Path.Combine(this["SolutionFolder"].ToString(), this["ProjectFolder"].ToString(), (string)sourceObject)
                    );
                PopContext();
                return returnCode;
            };
            _translationTableMap["Text File", "TemplateProcessor", "Text File"] = (targetObject, sourceObject) =>
            {
                int returnCode = 0;
                PushContext();
                Console.WriteLine("Source File {0} Name {1}", this["SourceFileType"], sourceObject);
                Console.WriteLine("Target File {0} Name {1}", this["TargetFileType"], targetObject);
                _templateProcessor.Translate(
                    Path.Combine(this["SolutionFolder"].ToString(), this["ProjectFolder"].ToString(), (string)targetObject)
                    , Path.Combine(this["SolutionFolder"].ToString(), this["ProjectFolder"].ToString(), (string)sourceObject)
                    );
                PopContext();
                return returnCode;
            };
            _make = new MakeProcessor(_macroProcessor);
            _make.AddFileTypeDeclaration(".txt", "Text File", string.Empty);
            _make.AddFileTypeDeclaration(".qdx", "QuickDex File", string.Empty);
            _make.AddFileTypeDeclaration(".cs", "Source Code", string.Empty);
            _make.AddFileTypeDeclaration(".xml", "XML Data", string.Empty);
            _make.AddFileTypeDeclaration(".xsd", "XML Schema", string.Empty);
            //_make.AddTypeDeclaration("Transaction");
            //_make.AddInstructionDeclaration("Scan", "Token<Transaction>", "Read", "File<Transaction>");
            //_make.AddInstructionDeclaration("Parse", "Symbol<Transaction>", "Read", "Token<Transaction>");
            //_make.AddInstructionDeclaration("Write", "File<Transaction>", "Write", "Token<Transaction>");
            //_make.AddInstructionDeclaration("Formate", "Token<Transaction>", "Write", "Symbol<Transaction>");
            //_make.AddInstructionDeclaration("Translate", "File<Console>", "Translate", "File<Console>");
        }
        public void DefineMacro(string name, object value)
        {
            _macroProcessor[name].Object = value;
        }
        public bool Expect(string name, string errorText)
        {
            return _macroProcessor.Expect(name, errorText);
        }
        public void PushContext()
        {
            _macroProcessor.PushContext();
        }

        public void PopContext()
        {
            _macroProcessor.PopContext();
        }
        public abstract void Load();
        public void Configure(string companyCode, string authorName, string solutionFolder)
        {
            DefineMacro("Company", companyCode);
            DefineMacro("Author", authorName);
            DefineMacro("SolutionFolder", solutionFolder);
            DefineMacro("SourceFolderName", @"$(SolutionFolder)\$(ProjectFolder)");
            DefineMacro("TargetFolderName", @"$(SolutionFolder)\$(ProjectFolder)");
        }
        public virtual int Translate(string targetType, string memoryType, string sourceType, object targetObject, object sourceObject, bool deferTranslation = false)
        {
            int returnCode = 0;
            PushContext();
            if (_translationTableMap.ContainsKey(targetType, memoryType, sourceType))
            {
                returnCode = _translationTableMap[targetType, memoryType, sourceType](targetObject, sourceObject);
            }
            else
                returnCode = -1;
            PopContext();
            return returnCode;
        }
        public virtual void Translate(string targetFileName, string sourceFileName, bool enableExpansion = false)
        {
            Translate(targetFileName, sourceFileName, enableExpansion);
        }

    }
}