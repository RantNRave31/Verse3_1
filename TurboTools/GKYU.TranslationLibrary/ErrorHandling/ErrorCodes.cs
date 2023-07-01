using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.CoreLibrary.ErrorHandling;

namespace GKYU.TranslationLibrary.ErrorHandling
{
	public partial class Error
	{
        public enum CODE : int
        {
            UNDEFINED = -1,
            OK = 0,
            PINPAD_ERROR = -4000,
            PINPAD_FAILED_INITIALIZE = -4001,
            PINPAD_FAILED_OPEN = -4002,
            PINPAD_FAILED_EXECUTE = -4003,
            PINPAD_ERROR_PARSE = -4004,
            NoLexerDeclared = 5000,
            MultipleLexerDeclarations = 5001,
            NFA_NoAcceptStates = 5002,
            NoParserDeclared = 5003,
            MultipleParserDeclarations = 5004,
        }
        public static Dictionary<Error.CODE, ErrorCode> ErrorCodes = new Dictionary<Error.CODE, ErrorCode>()
        {
            {Error.CODE.UNDEFINED, new ErrorCode((int)CODE.UNDEFINED, "UNDEFINED", "") },
            {Error.CODE.OK, new ErrorCode((int)CODE.OK, "OK", "0") },
            {Error.CODE.PINPAD_ERROR, new ErrorCode((int)CODE.PINPAD_ERROR, "PINPAD_ERROR", "") },
            {Error.CODE.PINPAD_FAILED_INITIALIZE, new ErrorCode((int)CODE.PINPAD_FAILED_INITIALIZE, "PINPAD_FAILED_INITIALIZE", "") },
            {Error.CODE.PINPAD_FAILED_OPEN, new ErrorCode((int)CODE.PINPAD_FAILED_OPEN, "PINPAD_FAILED_OPEN", "") },
            {Error.CODE.PINPAD_FAILED_EXECUTE, new ErrorCode((int)CODE.PINPAD_FAILED_EXECUTE, "PINPAD_FAILED_EXECUTE", "") },
            {Error.CODE.PINPAD_ERROR_PARSE, new ErrorCode((int)CODE.PINPAD_ERROR_PARSE, "PINPAD_ERROR_PARSE", "") },
            {Error.CODE.NoLexerDeclared, new ErrorCode((int)CODE.NoLexerDeclared, "NoLexerDeclared", "") },
            {Error.CODE.MultipleLexerDeclarations, new ErrorCode((int)CODE.MultipleLexerDeclarations, "MultipleLexerDeclarations", "") },
            {Error.CODE.NFA_NoAcceptStates, new ErrorCode((int)CODE.NFA_NoAcceptStates, "NFA_NoAcceptStates", "") },
            {Error.CODE.NoParserDeclared, new ErrorCode((int)CODE.NoParserDeclared, "NoParserDeclared", "") },
            {Error.CODE.MultipleParserDeclarations, new ErrorCode((int)CODE.MultipleParserDeclarations, "MultipleParserDeclarations", "") },
        };
	}
}