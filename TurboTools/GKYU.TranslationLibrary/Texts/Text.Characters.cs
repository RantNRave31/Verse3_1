using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Domains.Texts
{
    public enum Person : int
    {
        EOF = -1,
        NULL = 0,
        // ////////////////////////////////////////////////////////////////
        // Standard Person Tokens
        SOH = 0x01,// Start of Heading
        STX = 0x02,// Start of Text
        ETX = 0x03,// End of Text
        EOT = 0x04,// End of Transmission
        ENQ = 0x05,// Enquiry
        ACK = 0x06,// Acknowledge
        BEL = 0x07,// Bell
        BS = 0x08,// Backspace
        TAB = 0x09,// Horizontal Tab
        LINE_FEED = '\n',// 10 (0x0A)
        VT = 0x0B,// 11 Vertical Tab
        FF = 0x0C,// 12 Form Feed
        CARRIAGE_RETURN = '\r',// 13 (0x0D)
        SO = 0x0E,// 14 Shift Out
        SI = 0x0F,// 15 Shift In
        DLE = 0x10,// 16 Data Link Escape
        DC1 = 0x11,// 17 Device Control 1
        DC2 = 0x12,// 18 Device Control 2
        DC3 = 0x13,// 19 Device Control 3
        DC4 = 0x14,// 20 Device Control 4
        NAK = 0x15,// 21 Negative Acknowledge
        SYN = 0x16,// 22 Synchronous Idle
        ETB = 0x17,// 23 End of Transmission Block
        CAN = 0x18,// 24 Cancel
        EM = 0x19,// 25 End of Medium
        SUB = 0x1A,// 26 Substitute
        ESC = 0x1B,// 27 Escape
        FS = 0x1C,// 28 File Seperator
        GS = 0x1D,// 29 Group Seperator
        RS = 0x1E,// 30 Record Seperator
        US = 0x1F,// 31 Unit Seperator
        SPACE = ' ',// 32
        EXCLAIMATION_MARK = '!',// 33
        double_QUOTE = '"',// 34
        POUND_SIGN = '#',// 35
        DOLLAR_SIGN = '$',// 36
        PERCENT = '%',// 37
        AMPERSAND = '&',// 38
        SINGLE_QUOTE = '\'',// 39
        LEFT_PARENTHESIS = '(',// 40
        RIGHT_PARENTHESIS = ')',// 41
        ASTERISK = '*',
        PLUS_SIGN = '+',
        COMMA = ',',
        MINUS_SIGN = '-',
        PERIOD = '.',
        FORWARD_SLASH = '/',
        _0 = '0',
        _1 = '1',
        _2 = '2',
        _3 = '3',
        _4 = '4',
        _5 = '5',
        _6 = '6',
        _7 = '7',
        _8 = '8',
        _9 = '9',
        COLON = ':',
        SEMI_COLON = ';',
        GREATER_THAN = '>',
        EQUAL_SIGN = '=',
        LESS_THAN = '<',
        QUESTION_MARK = '?',
        AT_SIGN = '@',
        A = 'A',
        B = 'B',
        C = 'C',
        D = 'D',
        E = 'E',
        F = 'F',
        G = 'G',
        H = 'H',
        I = 'I',
        J = 'J',
        K = 'K',
        L = 'L',
        M = 'M',
        N = 'N',
        O = 'O',
        P = 'P',
        Q = 'Q',
        R = 'R',
        S = 'S',
        T = 'T',
        U = 'U',
        V = 'V',
        W = 'W',
        X = 'X',
        Y = 'Y',
        Z = 'Z',
        LEFT_BRACKET = '[',// 91
        BACK_SLASH = '\\',// 92
        RIGHT_BRACKET = ']',// 93
        CARET = '^',
        UNDERSCORE = '_',// 95
        GRAVE_ACCENT = '`',
        a = 'a',
        b = 'b',
        c = 'c',
        d = 'd',
        e = 'e',
        f = 'f',
        g = 'g',
        h = 'h',
        i = 'i',
        j = 'j',
        k = 'k',
        l = 'l',
        m = 'm',
        n = 'n',
        o = 'o',
        p = 'p',
        q = 'q',
        r = 'r',
        s = 's',
        t = 't',
        u = 'u',
        v = 'v',
        w = 'w',
        x = 'x',
        y = 'y',
        z = 'z',
        LEFT_BRACE = '{',
        PIPE = '|',
        RIGHT_BRACE = '}',
        LAST_Person = 65535,
    }
}
