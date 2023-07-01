using GKYU.CollectionsLibrary.Collections;
using GKYU.TranslationLibrary.Translators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary
{
    public class TemplateProcessor
    {
        internal class Parser
            : ParserBase<Token, Token>
        {
            enum STATE : int
            {
                INITIAL = 0,
                MATCH,
                NO_MATCH,
                CHARACTER,
                TEMPLATE_REFERENCE_BEGIN,
                TEMPLATE_NAME,
                TEMPLATE_REFERENCE_END,
                TEMPLATE_NAME_BAD,
                TEMPLATE_NAME_GOOD,
                TEMPLATE_READ,
                FINAL
            }
            MacroProcessor _macro;
            public Parser(MacroProcessor macro, string unexpandedText)
                : base(new StreamLexer(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(unexpandedText)))))
            {
                _macro = macro;
            }
            protected override Token Next()
            {
                Token token = new Token() { Kind = -1 };
                StringBuilder sb = new StringBuilder();
                StringBuilder templateName = new StringBuilder();
                StringBuilder styleName = new StringBuilder();
                bool bCollection = false;
                while (true)
                {
                    switch (STATE.INITIAL)
                    {
                        case STATE.INITIAL:
                            if (-1 == CurrentInput.Kind)
                            {
                                token.Value = sb.ToString();
                                return token;
                            }
                            else if ('<' == CurrentInput.Kind && '#' == _scanner.Peek(1).Kind)
                                goto case STATE.TEMPLATE_REFERENCE_BEGIN;
                            goto case STATE.CHARACTER;
                        case STATE.MATCH:
                            if (-1 == CurrentInput.Kind)
                                goto case STATE.FINAL;
                            else if ('<' == CurrentInput.Kind && '#' == _scanner.Peek(1).Kind)
                                goto case STATE.TEMPLATE_REFERENCE_BEGIN;
                            goto case STATE.CHARACTER;
                        case STATE.NO_MATCH:
                            Console.WriteLine("TemplateProcessor>  NO MATCH");
                            goto case STATE.FINAL;
                        case STATE.CHARACTER:// Normal Text
                            sb.Append((char)CurrentInput.Kind);
                            NextInput();
                            if (-1 == CurrentInput.Kind)
                                goto case STATE.MATCH;
                            else if ('<' == CurrentInput.Kind && '#' == _scanner.Peek(1).Kind)
                                goto case STATE.TEMPLATE_REFERENCE_BEGIN;
                            goto case STATE.CHARACTER;
                        case STATE.TEMPLATE_REFERENCE_BEGIN:// Begin Macro
                            Expect('<', "EXPECT Macro to begin with '<'");
                            Expect('#', "EXPECT Macro to begin with '<#'");
                            Expect('=', "EXPECT Macro to begin with '<#'");
                            templateName.Clear();
                            if ('#' == CurrentInput.Kind)
                                goto case STATE.TEMPLATE_REFERENCE_END;
                            goto case STATE.TEMPLATE_NAME;
                        case STATE.TEMPLATE_NAME:// Macro Name
                            templateName.Append((char)CurrentInput.Kind);
                            NextInput();
                            if (-1 == CurrentInput.Kind)
                                goto case STATE.NO_MATCH;
                            if (Accept('*'))// Kleene Star Notation, like RegEx.  means, find a list of kind and frigin loop repeat each line
                            {
                                // Load 1 template per record of type in symbol table productionspace 
                                bCollection = true;
                                sb.Append("<#= Kleene Not Supported(yet)");
                            }
                            if ('#' == CurrentInput.Kind)
                                goto case STATE.TEMPLATE_REFERENCE_END;
                            goto case STATE.TEMPLATE_NAME;
                        case STATE.TEMPLATE_REFERENCE_END:// End Macro
                            Expect('#', "EXPECT Macro to end with '#'");
                            Expect('>', "EXPECT Macro to end with '#>'");
                            if (bCollection)
                            {
                                bCollection = false;
                                goto case STATE.MATCH;
                            }
                            if (Dictionary.ContainsKey(templateName.ToString().Trim(),styleName.ToString().Trim()))
                                goto case STATE.TEMPLATE_NAME_GOOD;
                            goto case STATE.TEMPLATE_NAME_BAD;
                        case STATE.TEMPLATE_NAME_BAD:// Bad Macro
                            sb.Append("<#= Template Definition Not Found#>)");
                            goto case STATE.NO_MATCH;
                        case STATE.TEMPLATE_NAME_GOOD:// Good Macro
                            string templateFileName = Dictionary[templateName.ToString().Trim(), styleName.ToString().Trim()];
                            if (!File.Exists(templateFileName))
                            {
                                sb.Append("$(Template File Not Found)");
                                goto case STATE.NO_MATCH;
                            }
                            goto case STATE.TEMPLATE_READ;
                        case STATE.TEMPLATE_READ:// Good File
                            string expandedTemplateFileName = _macro.Parse(templateFileName, true);
                            using (StreamReader sr = new StreamReader(expandedTemplateFileName))
                            {
                                string unexpandedText;
                                while(string.Empty != (unexpandedText = sr.ReadLine()))
                                {
                                    string expandedText = _macro.Parse(unexpandedText, true);
                                    TemplateProcessor.Parser fragmentParser = new Parser(_macro, expandedText);
                                    sb.AppendLine(fragmentParser.Read().Value);
                                }
                            }
                            goto case STATE.MATCH;
                        case STATE.FINAL:
                            EndOfInput = true;
                            token.Value = sb.ToString();
                            return token;
                    }
                }
            }
        }

        public static Dictionary2D<string, string> Dictionary = new Dictionary2D<string, string>();

        protected MacroProcessor _macroProcessor;
        static TemplateProcessor()
        {
            Dictionary["FileDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\FileDeclaration.txt";
            Dictionary["NamespaceDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\NamespaceDeclaration.txt";
            Dictionary["InterfaceDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\InterfaceDeclaration.txt";
            Dictionary["EnumerationDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\EnumerationDeclaration.txt";
            Dictionary["ClassDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\ClassDeclaration.txt";
            Dictionary["MemberDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\MemberDeclaration.txt";
            Dictionary["MethodDeclaration", ""] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\MethodDeclaration.txt";

            Dictionary["InterfaceDeclaration", "Builder"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Builder\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Builder"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Builder\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Visitor"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Visitor\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Visitor"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Visitor\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Copier"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Copier\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Copier"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Copier\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Factory"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Factory\InterfaceDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Builder"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Builder\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Builder"] = @"C:\Users\rantn\source\repos\RDSSolution\GKYU.TranslationLibrary\Resources\Templates\Builder\ClassDeclaration.txt";

        }
        public TemplateProcessor(MacroProcessor macroProcessor)
        {
            _macroProcessor = macroProcessor;
        }
        public void Translate(string targetFileName, string sourceFileName, bool enableExpansion = false)
        {
            using (StreamReader sr = new StreamReader(sourceFileName))
            {
                using (StreamWriter sw = new StreamWriter(targetFileName))
                {
                    string unexpandedText;
                    while (string.Empty != (unexpandedText = sr.ReadLine()))
                    {
                        string expandedText = _macroProcessor.Parse(unexpandedText);
                        Parser parser = new Parser(_macroProcessor, expandedText);
                        sw.WriteLine(parser.Read().Value);
                    }

                }
            }
        }
    }
}
