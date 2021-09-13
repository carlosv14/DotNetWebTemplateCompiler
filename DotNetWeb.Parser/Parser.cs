using DotNetWeb.Core;
using DotNetWeb.Core.Interfaces;
using System;

namespace DotNetWeb.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner scanner;
        private Token lookAhead;
        public Parser(IScanner scanner)
        {
            this.scanner = scanner;
            this.Move();
        }
        public void Parse()
        {
            Program();
        }

        private void Program()
        {
            Init();
            Template();
        }

        private void Template()
        {
            Tag();
            InnerTemplate();
        }
        
        private void InnerTemplate()
        {
            if (this.lookAhead.TokenType == TokenType.LessThan)
            {
                Template();
            }
        }
        private void Tag()
        {
            Match(TokenType.LessThan);
            Match(TokenType.Identifier);
            Match(TokenType.GreaterThan);
            Stmts();
            Match(TokenType.LessThan);
            Match(TokenType.Slash);
            Match(TokenType.Identifier);
            Match(TokenType.GreaterThan);
        }

        private void Stmts()
        {
            if (this.lookAhead.TokenType == TokenType.OpenBrace)
            {
                Stmt();
                Stmts();
            }
        }

        private void Stmt()
        {
            Match(TokenType.OpenBrace);
            switch (this.lookAhead.TokenType)
            {
                case TokenType.OpenBrace:
                    Match(TokenType.OpenBrace);
                    Eq();
                    Match(TokenType.CloseBrace);
                    Match(TokenType.CloseBrace);
                    break;
                case TokenType.Percentage:
                    IfStmt();
                    break;
                case TokenType.Hyphen:
                    ForeachStatement();
                    break;
                default:
                    throw new ApplicationException("Unrecognized statement");
            }
        }

        private void ForeachStatement()
        {
            Match(TokenType.Hyphen);
            Match(TokenType.Percentage);
            Match(TokenType.ForEeachKeyword);
            Match(TokenType.Identifier);
            Match(TokenType.InKeyword);
            Match(TokenType.Identifier);
            Match(TokenType.Percentage);
            Match(TokenType.CloseBrace);
            Template();
            Match(TokenType.OpenBrace);
            Match(TokenType.Percentage);
            Match(TokenType.EndForEachKeyword);
            Match(TokenType.Percentage);
            Match(TokenType.CloseBrace);
        }

        private void IfStmt()
        {
            Match(TokenType.Percentage);
            Match(TokenType.IfKeyword);
            Eq();
            Match(TokenType.Percentage);
            Match(TokenType.CloseBrace);
            Template();
            Match(TokenType.OpenBrace);
            Match(TokenType.Percentage);
            Match(TokenType.EndIfKeyword);
            Match(TokenType.Percentage);
            Match(TokenType.CloseBrace);
        }

        private void Eq()
        {
            Rel();
            while (this.lookAhead.TokenType == TokenType.Equal || this.lookAhead.TokenType == TokenType.NotEqual)
            {
                Move();
                Rel();
            }
        }

        private void Rel()
        {
            Expr();
            if (this.lookAhead.TokenType == TokenType.LessThan
                || this.lookAhead.TokenType == TokenType.GreaterThan)
            {
                Move();
                Expr();
            }
        }

        private void Expr()
        {
            Term();
            while (this.lookAhead.TokenType == TokenType.Plus || this.lookAhead.TokenType == TokenType.Hyphen)
            {
                Move();
                Term();
            }
        }

        private void Term()
        {
            Factor();
            while (this.lookAhead.TokenType == TokenType.Asterisk || this.lookAhead.TokenType == TokenType.Slash)
            {
                Move();
                Factor();
            }
        }

        private void Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftParens:
                    {
                        Match(TokenType.LeftParens);
                        Eq();
                        Match(TokenType.RightParens);
                    }
                    break;
                case TokenType.IntConstant:
                    Match(TokenType.IntConstant);
                    break;
                case TokenType.FloatConstant:
                    Match(TokenType.FloatConstant);
                    break;
                case TokenType.StringConstant:
                    Match(TokenType.StringConstant);
                    break;
                case TokenType.OpenBracket:
                    Match(TokenType.OpenBracket);
                    ExprList();
                    Match(TokenType.CloseBracket);
                    break;
                default:
                    Match(TokenType.Identifier);
                    break;
            }
        }

        private void ExprList()
        {
            Eq();
            if (this.lookAhead.TokenType != TokenType.Comma)
            {
                return;
            }
            Match(TokenType.Comma);
            ExprList();
        }

        private void Init()
        {
            Match(TokenType.OpenBrace);
            Match(TokenType.Percentage);
            Match(TokenType.InitKeyword);
            Code();
            Match(TokenType.Percentage);
            Match(TokenType.CloseBrace);
        }

        private void Code()
        {
            Decls();
            Assignations();
        }

        private void Assignations()
        {
            if (this.lookAhead.TokenType == TokenType.Identifier)
            {
                Assignation();
                Assignations();
            }
        }

        private void Assignation()
        {
            Match(TokenType.Identifier);
            Match(TokenType.Assignation);
            Eq();
            Match(TokenType.SemiColon);
        }

        private void Decls()
        {
            Decl();
            InnerDecls();
        }

        private void InnerDecls()
        {
            if (this.LookAheadIsType())
            {
                Decls();
            }
        }

        private void Decl()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.FloatKeyword:
                    Match(TokenType.FloatKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.StringKeyword:
                    Match(TokenType.StringKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.IntKeyword:
                    Match(TokenType.IntKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.FloatListKeyword:
                    Match(TokenType.FloatListKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.IntListKeyword:
                    Match(TokenType.IntListKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                case TokenType.StringListKeyword:
                    Match(TokenType.StringListKeyword);
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    break;
                default:
                    throw new ApplicationException($"Unsupported type {this.lookAhead.Lexeme}");
            }
        }

        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        private void Match(TokenType tokenType)
        {
            if (this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax error! expected token {tokenType} but found {this.lookAhead.TokenType}. Line: {this.lookAhead.Line}, Column: {this.lookAhead.Column}");
            }
            this.Move();
        }

        private bool LookAheadIsType()
        {
            return this.lookAhead.TokenType == TokenType.IntKeyword ||
                this.lookAhead.TokenType == TokenType.StringKeyword ||
                this.lookAhead.TokenType == TokenType.FloatKeyword ||
                this.lookAhead.TokenType == TokenType.IntListKeyword ||
                this.lookAhead.TokenType == TokenType.FloatListKeyword ||
                this.lookAhead.TokenType == TokenType.StringListKeyword;

        }
    }
}
