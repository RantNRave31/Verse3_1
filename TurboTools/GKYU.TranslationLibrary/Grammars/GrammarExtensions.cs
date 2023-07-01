using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections;
    public static class GrammarExtensions
    {
        public static Syntax.CharacterDeclaration DeclareCharacter(this Syntax.SymbolTable symbolTable, char characterCode)
        {
            string name = symbolTable.GetCharacterName(characterCode);
            if (symbolTable.CharacterDeclarations.ContainsKey(name))
                return symbolTable.CharacterDeclarations[name];
            else
                return symbolTable.DeclareCharacter(name, (int)characterCode);
        }
        public static Syntax.CharacterDeclaration DeclareCharacter(this Syntax.SymbolTable symbolTable, string name, int characterCode)
        {
            if (symbolTable.CharacterDeclarations.ContainsKey(name))
                return symbolTable.CharacterDeclarations[name];
            return symbolTable.CreateCharacterDeclaration(name, characterCode);
        }
        public static Syntax.CharacterSetDeclaration DeclareCharacterSet(this Syntax.SymbolTable symbolTable, string name, CharacterSet set)
        {
            if (symbolTable.CharacterSetDeclarations.ContainsKey(name))
                return symbolTable.CharacterSetDeclarations[name];
            return symbolTable.CreateCharacterSet(name, set);
        }
        public static Syntax.TokenDeclaration DeclareToken(this Syntax.SymbolTable symbolTable, string name)
        {
            return symbolTable.CreateTokenDeclaration(name);
        }
        public static Syntax.ExpressionNode Or(this Syntax.ExpressionNode a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeChoice(a, b);
        }
        public static Syntax.ExpressionNode Or(this char a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)b.SymbolTable).MakeChoice(((Syntax.SymbolTable)b.SymbolTable).DeclareCharacter(a), b);
        }
        public static Syntax.ExpressionNode Or(this Syntax.ExpressionNode a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeChoice(a, ((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b));
        }
        public static Syntax.ExpressionNode Or(this Syntax.TypeDeclaration a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeChoice(a, b);
        }
        public static Syntax.ExpressionNode Or(this Syntax.TypeDeclaration a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeChoice(a, ((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b));
        }
        public static Syntax.ExpressionNode Then(this char a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)b.SymbolTable).MakeSequence(((Syntax.SymbolTable)b.SymbolTable).DeclareCharacter(a), b);
        }
        public static Syntax.ExpressionNode ThenRepeat(this char a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)b.SymbolTable).MakeSequence(((Syntax.SymbolTable)b.SymbolTable).DeclareCharacter(a), ((Syntax.SymbolTable)b.SymbolTable).MakeRepitition(b));
        }
        public static Syntax.ExpressionNode Then(this Syntax.ExpressionNode a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b));
        }
        public static Syntax.ExpressionNode ThenRepeat(this Syntax.ExpressionNode a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).MakeRepitition(((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b)));
        }
        public static Syntax.ExpressionNode Then(this Syntax.ExpressionNode a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, b);
        }
        public static Syntax.ExpressionNode Then(this Syntax.TypeDeclaration a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, b);
        }
        public static Syntax.ExpressionNode Then(this Syntax.TypeDeclaration a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b));
        }
        public static Syntax.ExpressionNode ThenRepeat(this Syntax.ExpressionNode a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).MakeRepitition(b));
        }
        public static Syntax.ExpressionNode ThenRepeat(this Syntax.TypeDeclaration a, Syntax.ExpressionNode b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).MakeRepitition(b));
        }
        public static Syntax.ExpressionNode ThenRepeat(this Syntax.TypeDeclaration a, char b)
        {
            return ((Syntax.SymbolTable)a.SymbolTable).MakeSequence(a, ((Syntax.SymbolTable)a.SymbolTable).MakeRepitition(((Syntax.SymbolTable)a.SymbolTable).DeclareCharacter(b)));
        }
    }
}
