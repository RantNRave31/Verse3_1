using System;
using System.IO;
using GKYU.CollectionsLibrary.Collections;
using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Patterns;
using GKYU.TranslationLibrary.Symbols;
using GKYU.TranslationLibrary.Translators;

namespace GKYU.TranslationLibrary.Grammars
{
    public class TextTemplate
    {

        public interface IBuildTemplateObjects
        {
            TextTemplate.Node Node();
            TemplateDeclaration TemplateDeclaration();
        }
        public interface ICopyTemplateObjects
        {
            TextTemplate.Node Visit(TextTemplate.Node node);
            TemplateDeclaration Visit(TemplateDeclaration templateDeclaration);
        }
        public interface IVisitTemplateObjects
            : IVisitSymbol
        {
            void Visit(TextTemplate.Node node);
            void Visit(TemplateDeclaration templateDeclaration);
        }
        public class Node
            : Symbol.Named
        {
            public TextTemplate.Node Parent { get; set; }
            public string Type { get; set; }
            public string Style { get; set; }
            public Node()
                : base()
            {
                this.Parent = null;
                this.Type = string.Empty;
                this.Style = string.Empty;
            }
            public Node(Symbol.Table symbolTable)
                : base(symbolTable)
            {
                this.Parent = null;
                this.Type = string.Empty;
                this.Style = string.Empty;
            }
            public Node(TextTemplate.Node reference)
                : base()
            {
                this.Parent = reference.Parent;
                this.Type = reference.Type;
                this.Style = reference.Style;
            }
            public virtual TextTemplate.Node Accept(ICopyTemplateObjects visitor)
            {
                return visitor.Visit(this);
            }
            public virtual void Accept(IVisitTemplateObjects visitor)
            {
                visitor.Visit(this);
            }
        }
        public class TemplateDeclaration
            : Node
        {
            public TemplateDeclaration()
                : base()
            {

            }
            public TemplateDeclaration(Symbol.Table symbolTable)
                : base(symbolTable)
            {

            }
            public TemplateDeclaration(TemplateDeclaration reference)
                : base(reference)
            {

            }
            public new TemplateDeclaration Accept(ICopyTemplateObjects visitor)
            {
                return visitor.Visit(this);
            }
            public override void Accept(IVisitTemplateObjects visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Factory
        {
            public static object Create<T>()
                where T : Symbol
            {
                if (typeof(T) == typeof(Node)) return Node();
                return null;
            }
            public static Node Node()
            {
                return new TextTemplate.Node();
            }
            public static TemplateDeclaration TemplateDeclaration()
            {
                return new TemplateDeclaration();
            }
        }
        public class Visitor
            : VisitorBase<TextTemplate.Node>
            , IVisitTemplateObjects
        {
            public override void Visit(Symbol symbol)
            {
                throw new NotImplementedException();
            }

            public override void Visit(Symbol.Named namedSymbol)
            {
                throw new NotImplementedException();
            }
            public override void Visit(Node element)
            {

            }
            public virtual void Visit(TemplateDeclaration element)
            {

            }

        }
        public class Builder
            : IBuildTemplateObjects
        {
            public Node Node()
            {
                throw new NotImplementedException();
            }

            public TemplateDeclaration TemplateDeclaration()
            {
                throw new NotImplementedException();
            }
        }
        public class Copier
            : ICopyTemplateObjects
        {
            public Copier(TextTemplate.Node element)
            {

            }
            public Node Visit(Node node)
            {
                throw new NotImplementedException();
            }

            public TemplateDeclaration Visit(TemplateDeclaration templateDeclaration)
            {
                throw new NotImplementedException();
            }
        }
        public class ObjectWriter
            : WriterBase<Node>
            , IWriteSymbolVisitors<Node>
        {
            public class DefaultReport
                : TextTemplate.Visitor
                , IVisitTemplateObjects
            {
                protected ObjectWriter _writer;
                public DefaultReport(ObjectWriter writer)
                    : base()
                {
                    _writer = writer;
                }
                public override void Visit(Node node)
                {
                    _writer.WriteLine("Node[{0}]", node.Name);

                }

                public override void Visit(TemplateDeclaration templateDeclaration)
                {
                    _writer.WriteLine("TemplateDeclaration[{0}]", templateDeclaration.Name);
                }
            }
            static ObjectWriter()
            {

            }
            public ObjectWriter(StreamWriter streamWriter)
                : base(streamWriter)
            {

            }
        }
        /// <summary>
        /// Unit of Work Pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static Dictionary2D<string, string> Dictionary = new Dictionary2D<string, string>();
        public static Symbol.Table SymbolTable = new Symbol.Table();
        static TextTemplate()
        {
            SymbolTable.Register<TemplateDeclaration>();
            
            // TODO: runtime optimization, decrease lookup by caching key adn style.
            // TODO:  convert string tokens to integer tokens (after implementation.. strings for ease of design only. (to token is easily automated after pattern is complete)
            // Coordinates are [Key,Style]// changed order from style,type,key to type,key,style that way, when style does not exist, we can choose the empty style without 3*? lookup.
            Dictionary["NamespaceDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\NamespaceDeclaration.txt";
            Dictionary["InterfaceDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\InterfaceDeclaration.txt";
            Dictionary["EnumerationDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\EnumerationDeclaration.txt";
            Dictionary["ClassDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\ClassDeclaration.txt";
            Dictionary["MemberDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\MemberDeclaration.txt";
            Dictionary["MethodDeclaration", ""] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\MethodDeclaration.txt";

            Dictionary["InterfaceDeclaration", "Builder"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Builder\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Builder"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Builder\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Visitor"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Visitor\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Visitor"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Visitor\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Copier"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Copier\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Copier"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Copier\ClassDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Factory"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Factory\InterfaceDeclaration.txt";
            Dictionary["InterfaceDeclaration", "Builder"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Builder\InterfaceDeclaration.txt";
            Dictionary["ClassDeclaration", "Builder"] = @"$(SolutionFolder)\GKYU.TranslationLibrary\Resources\Templates\Builder\ClassDeclaration.txt";

            //_templates["", "Template", ""] = "";
        }
    }
}
