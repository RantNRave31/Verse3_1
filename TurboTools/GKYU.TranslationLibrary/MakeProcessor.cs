using GKYU.CollectionsLibrary.Collections;
using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Patterns;
using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;

using GKYU.TranslationLibrary.Translators;
using GKYU.CollectionsLibrary.Collections.Graphs;
using GKYU.TranslationLibrary.Grammars;

namespace GKYU.TranslationLibrary
{
    public class Make
    {
        public class FileTypeDeclaration
            : Syntax.TypeDeclaration
        {
            public string FileTypeName { get; set; }
            public string FileGrammarName { get; set; }
            public string RelativePath { get; set; }
            public bool Exists { get; set; }
            public bool Generated { get; set; }
            public bool Template { get; set; }
            public Dictionary<string, FragmentTypeDeclaration> PatternFragmentMap = new Dictionary<string, FragmentTypeDeclaration>();
        }
        public class FragmentTypeDeclaration
            : Syntax.TypeDeclaration
        {
            public string FileName { get; set; }
            public string Pattern { get; set; }
            public string ReadFormula { get; set; }
            public string ReadFormat { get; set; }
            public string WriteFormula { get; set; }
            public string WriteFormat { get; set; }
        }
        public class StepTypeDeclaration
            : Syntax.TypeDeclaration
            , IEquatable<StepTypeDeclaration>
        {
            public int Step { get; set; }
            public bool Equals(StepTypeDeclaration other)
            {
                return base.Equals(other);
            }
        }
        public class InstructionTypeDeclaration
            : Syntax.TypeDeclaration
        {
            public List<StepTypeDeclaration> Steps { get; set; }
            public InstructionTypeDeclaration()
                : base()
            {
                
            }
        }
        public class FileInstanceDeclaration
            : Syntax.Instance<FileTypeDeclaration>
           , IEquatable<FileInstanceDeclaration>
        {
            public bool Equals(FileInstanceDeclaration other)
            {
                return base.Equals(other);
            }

        }
        public class TaskInstanceDeclaration
            : Syntax.Instance<InstructionTypeDeclaration>
           , IEquatable<InstructionTypeDeclaration>
        {
            public bool Equals(InstructionTypeDeclaration other)
            {
                return base.Equals(other);
            }

        }
        public class MakeDictionary
            : Dictionary<string,InstructionTypeDeclaration>
        {
            public Dictionary<string,Syntax.TypeDeclaration> MakeTypes { get; set; }
            public MakeDictionary()
                : base()
            {
                MakeTypes = new Dictionary<string, Syntax.TypeDeclaration>();
            }
            public void AddFileExtensions(Dictionary<string, string> fileExtensionMap)
            {
                foreach (KeyValuePair<string, string> pair in fileExtensionMap)
                {
                    Syntax.TypeDeclaration makeType = new Syntax.TypeDeclaration() { Name = pair.Key };
                    this.MakeTypes.Add(pair.Key, makeType);
                }
            }
        }
        public class SymbolTable
            : Syntax.SymbolTable
        {
            public SymbolTable()
                : base()
            {
                Register<FileTypeDeclaration>();
                Register<FragmentTypeDeclaration>();
                Register<StepTypeDeclaration>();
                Register<InstructionTypeDeclaration>();
                Register<FileInstanceDeclaration>();
                Register<TaskInstanceDeclaration>();
                //Register<StepDeclaration>();
            }
        }

    }
    public class MakeProcessor
    {
        protected Make.SymbolTable _symbolTable = new Make.SymbolTable();
        public static Make.MakeDictionary _makeDictionary = new Make.MakeDictionary();
        public Dictionary<string, Syntax.TypeDeclaration> _fileExtension2ObjectTypeMap = new Dictionary<string, Syntax.TypeDeclaration>();
        public Dictionary2D<string, Make.FileTypeDeclaration> _fileName2FileDeclarationMap = new Dictionary2D<string, Make.FileTypeDeclaration>();
        public Dictionary<string, Make.FragmentTypeDeclaration> _fragmentName2FragmentDeclarationMap = new Dictionary<string, Make.FragmentTypeDeclaration>();
        static MakeProcessor()
        {
        }
        public MakeProcessor(MacroProcessor macroProcessor)
        {

        }
        public virtual void Load()
        {
            throw new NotImplementedException();
        }
        public void AddTypeDeclaration(string makeTypeName)
        {
            Syntax.TypeDeclaration makeType = _symbolTable.Create<Syntax.TypeDeclaration>();
            makeType.Name = makeTypeName;
            _makeDictionary.MakeTypes.Add(makeTypeName, makeType);
        }
        public void AddFileTypeDeclaration(string fileType, string fileName, string filePath)
        {
            if (!_makeDictionary.MakeTypes.ContainsKey(fileType))
                AddTypeDeclaration(fileType);
            Make.FileTypeDeclaration fileDeclaration = _symbolTable.Create<Make.FileTypeDeclaration>();
            fileDeclaration.Name = fileName;
            fileDeclaration.RelativePath = filePath;
            _fileName2FileDeclarationMap[fileType, fileName] = fileDeclaration;
        }
        public void AddFragmentDeclaration(string fragmentKey, string pattern, string readFormula, string readFormat, string writeFormula, string writeFormat, string fileName)
        {
            Make.FragmentTypeDeclaration fragment = _symbolTable.Create<Make.FragmentTypeDeclaration>();
            fragment.Name = fragmentKey;
            fragment.Pattern = pattern;
            fragment.ReadFormula = readFormula;
            fragment.ReadFormat = readFormat;
            fragment.WriteFormula = writeFormula;
            fragment.WriteFormat = writeFormat;
            fragment.FileName = fileName;
            _fragmentName2FragmentDeclarationMap[fragmentKey] = fragment;
        }
        public void AddInstructionDeclaration(string instructionName, string subjectTypeName, string action, string objectTypeName)
        {

        }
        public void AddProjectDeclaration(string projectName)
        {

        }
        public void Make(string targetFileName, string makeType)
        {

        }
    }
}