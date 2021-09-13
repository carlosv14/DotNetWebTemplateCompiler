using DotNetWeb.Core;
using DotNetWeb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWeb.Lexer
{
    public class Scanner : IScanner
    {
        private Input input;
        private readonly Dictionary<string, TokenType> keywords;

        public Scanner(Input input)
        {
            this.input = input;
            this.keywords = new Dictionary<string, TokenType>
            {
                { "if", TokenType.IfKeyword  },
                { "int", TokenType.IntKeyword },
                { "float", TokenType.FloatKeyword },
                { "string", TokenType.StringKeyword },
                { "foreach", TokenType.ForEeachKeyword },
                { "IntList", TokenType.IntListKeyword },
                { "FloatList", TokenType.IntListKeyword },
                { "StringList", TokenType.StringListKeyword },
                { "endforeach", TokenType.EndForEachKeyword },
                { "init", TokenType.InitKeyword },
                { "endif", TokenType.EndIfKeyword },
                { "in", TokenType.InKeyword },
            };
        }

        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentChar = GetNextChar();
            while (true)
            {
                while (char.IsWhiteSpace(currentChar) || currentChar == '\n')
                {
                    currentChar = GetNextChar();
                }

                if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsLetterOrDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (this.keywords.ContainsKey(lexeme.ToString()))
                    {
                        return new Token
                        {
                            TokenType = this.keywords[lexeme.ToString()],
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString()
                        };
                    }

                    return new Token
                    {
                        TokenType = TokenType.Identifier,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString(),
                    };
                }
                else if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (currentChar != '.')
                    {
                        return new Token
                        {
                            TokenType = TokenType.IntConstant,
                            Column = input.Position.Column,
                            Line = input.Position.Line,
                            Lexeme = lexeme.ToString(),
                        };
                    }

                    currentChar = GetNextChar();
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }
                    return new Token
                    {
                        TokenType = TokenType.FloatConstant,
                        Column = input.Position.Column,
                        Line = input.Position.Line,
                        Lexeme = lexeme.ToString(),
                    };

                }
                else switch (currentChar)
                    {
                        case '<':
                            lexeme.Append(currentChar);
                            var nextChar = PeekNextChar();
                            switch (nextChar)
                            {
                                case '>':
                                    lexeme.Append(nextChar);
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.NotEqual,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                default:
                                    return new Token
                                    {
                                        TokenType = TokenType.LessThan,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString().Trim()
                                    };
                            }
                        case '>':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.GreaterThan,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '/':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Slash,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '{':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.OpenBrace,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '}':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.CloseBrace,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '%':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Percentage,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '-':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Hyphen,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '+':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Plus,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '(':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.LeftParens,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case ')':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.RightParens,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '*':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Asterisk,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case ';':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.SemiColon,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case ',':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Comma,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case ':':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.SemiColon,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '[':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.OpenBracket,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case ']':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.CloseBracket,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '|':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Pipe,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = lexeme.ToString()
                            };
                        case '=':
                            lexeme.Append(currentChar);
                            nextChar = PeekNextChar();
                            switch (nextChar)
                            {
                                case '=':
                                    lexeme.Append(nextChar);
                                    GetNextChar();
                                    return new Token
                                    {
                                        TokenType = TokenType.Equal,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString()
                                    };
                                default:
                                    return new Token
                                    {
                                        TokenType = TokenType.Assignation,
                                        Column = input.Position.Column,
                                        Line = input.Position.Line,
                                        Lexeme = lexeme.ToString().Trim()
                                    };
                            }
                        case '\'':
                            {
                                lexeme.Append(currentChar);
                                currentChar = GetNextChar();
                                while (currentChar != '\'')
                                {
                                    lexeme.Append(currentChar);
                                    currentChar = GetNextChar();
                                }
                                lexeme.Append(currentChar);
                                return new Token
                                {
                                    TokenType = TokenType.StringConstant,
                                    Column = input.Position.Column,
                                    Line = input.Position.Line,
                                    Lexeme = lexeme.ToString()
                                };
                            }
                        case '\0':
                            return new Token
                            {
                                TokenType = TokenType.EOF,
                                Column = input.Position.Column,
                                Line = input.Position.Line,
                                Lexeme = string.Empty
                            };
                        default:
                            lexeme.Append(currentChar);
                            throw new ApplicationException($"Caracter {lexeme} invalido en la columna: {input.Position.Column}, fila: {input.Position.Line}");
                    }
            }
        }

        private char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }
    }
}
