using System;

namespace DotNetWeb.Core
{
    public enum TokenType
    {
        Slash,
        OpenBrace,
        CloseBrace,
        ForEeachKeyword,
        IfKeyword,
        Percentage,
        Hyphen,
        Asterisk,
        Plus,
        LeftParens,
        RightParens,
        SemiColon,
        Equal,
        LessThan,
        NotEqual,
        GreaterThan,
        IntKeyword,
        Identifier,
        IntConstant,
        FloatConstant,
        Assignation,
        StringConstant,
        EOF,
        FloatKeyword,
        StringKeyword,
        IntListKeyword,
        FloatListKeyword,
        StringListKeyword,
        EndForEachKeyword,
        Colon,
        OpenBracket,
        CloseBracket,
        Comma,
        Pipe,
        InitKeyword,
        EndIfKeyword,
        InKeyword,
        BasicType
    }
}
