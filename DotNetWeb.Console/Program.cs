using DotNetWeb.Core;
using DotNetWeb.Lexer;
using System;
using System.IO;

namespace DotNetWeb.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("Code.txt").Replace(Environment.NewLine, "\n");
            var input = new Input(code);
            var scanner = new Scanner(input);
            var parser = new Parser.Parser(scanner);
            parser.Parse();
        }
    }
}
