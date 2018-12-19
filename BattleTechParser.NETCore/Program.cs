using System;
using BattleTechParser;

namespace BattleTechParser.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BattleTechParser.Lib.Parser parserFromOriginalCSV = new Lib.Parser();
            BattleTechParser.Lib.Parser parserFromJsonFiles = new Lib.Parser();
            BattleTechParser.Lib.Parser parserFromTranslatedCSV = new Lib.Parser();
            parserFromOriginalCSV.AddCSVFromFilePath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data\localization\strings_fr-FR - 복사본.csv");
            parserFromJsonFiles.AddJsonFromFolderPath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\TMPro.TextMeshProUGUI");
            parserFromJsonFiles.AddJsonFromFolderPath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data");
            parserFromTranslatedCSV.AddCSVFromFilePath(@"d:\Downloads\strings_fr-FR.csv");
            BattleTechParser.Lib.Parser mergedParser = parserFromOriginalCSV.Merge(parserFromJsonFiles);
            BattleTechParser.Lib.Parser mergedParser2 = mergedParser.Merge(parserFromTranslatedCSV);
            string tempCSV = mergedParser2.ToCSVFormat();
            string tempCSV2 = parserFromJsonFiles.ToCSVFormat();
            Console.WriteLine("Hello World!");

        }
    }
}
