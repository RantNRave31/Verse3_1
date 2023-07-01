using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.CoreLibrary.ErrorHandling;

namespace GKYU.PresentationLogicLibrary.ErrorHandling
{
	public partial class ErrorViewModel
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
        public static Dictionary<ErrorViewModel.CODE, ErrorCode> ErrorCodes = new Dictionary<ErrorViewModel.CODE, ErrorCode>()
        {
            {ErrorViewModel.CODE.UNDEFINED, new ErrorCode((int)CODE.UNDEFINED, "UNDEFINED", "") },
            {ErrorViewModel.CODE.OK, new ErrorCode((int)CODE.OK, "OK", "0") },
            {ErrorViewModel.CODE.PINPAD_ERROR, new ErrorCode((int)CODE.PINPAD_ERROR, "PINPAD_ERROR", "") },
            {ErrorViewModel.CODE.PINPAD_FAILED_INITIALIZE, new ErrorCode((int)CODE.PINPAD_FAILED_INITIALIZE, "PINPAD_FAILED_INITIALIZE", "") },
            {ErrorViewModel.CODE.PINPAD_FAILED_OPEN, new ErrorCode((int)CODE.PINPAD_FAILED_OPEN, "PINPAD_FAILED_OPEN", "") },
            {ErrorViewModel.CODE.PINPAD_FAILED_EXECUTE, new ErrorCode((int)CODE.PINPAD_FAILED_EXECUTE, "PINPAD_FAILED_EXECUTE", "") },
            {ErrorViewModel.CODE.PINPAD_ERROR_PARSE, new ErrorCode((int)CODE.PINPAD_ERROR_PARSE, "PINPAD_ERROR_PARSE", "") },
            {ErrorViewModel.CODE.NoLexerDeclared, new ErrorCode((int)CODE.NoLexerDeclared, "NoLexerDeclared", "") },
            {ErrorViewModel.CODE.MultipleLexerDeclarations, new ErrorCode((int)CODE.MultipleLexerDeclarations, "MultipleLexerDeclarations", "") },
            {ErrorViewModel.CODE.NFA_NoAcceptStates, new ErrorCode((int)CODE.NFA_NoAcceptStates, "NFA_NoAcceptStates", "") },
            {ErrorViewModel.CODE.NoParserDeclared, new ErrorCode((int)CODE.NoParserDeclared, "NoParserDeclared", "") },
            {ErrorViewModel.CODE.MultipleParserDeclarations, new ErrorCode((int)CODE.MultipleParserDeclarations, "MultipleParserDeclarations", "") },
        };
	}
}