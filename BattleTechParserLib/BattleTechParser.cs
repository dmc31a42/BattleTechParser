using System;
using System.Text.RegularExpressions;

namespace BattleTechParserLib
{
    public class BattleTechParser
    {
        public BattleTechParser() { }

        public bool AddCSVFromFilePath(string CSVFilePath)
        {
            try
            {
                string csvStr = System.IO.File.ReadAllText(CSVFilePath);
                string splitRegex = @"(.*),(.*)";
                foreach(Match match in Regex.Matches(csvStr, splitRegex))
                {
                    if (match.Groups.Count >= 2)
                    {
                        Console.WriteLine("{0},{1}", match.Groups[1], match.Groups[2]);
                    }
                    else
                    {
                        continue;
                    }
                    
                }
                return true;
            } catch (Exception ReadAllTextException)
            {
                return false;
            }
        }
    }
}
