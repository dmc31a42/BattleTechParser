using System;
using BattleTechParser;

namespace BattleTechParser.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BattleTechParser.Lib.Parser battleTechParser = new Lib.Parser();
            Lib.Parser.SplitLine("test,te");
        }
    }
}
