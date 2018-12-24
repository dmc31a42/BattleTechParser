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
            BattleTechParser.Lib.Parser parserFromGameTips = new Lib.Parser();
            BattleTechParser.Lib.Parser parserFromSQLs = new Lib.Parser();
            parserFromOriginalCSV.AddCSVFromFilePath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data\localization\strings_fr-FR - 복사본.csv");
            parserFromJsonFiles.AddJsonFromFolderPath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\AllTextRelatedAsset");
            parserFromJsonFiles.AddJsonFromFolderPath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data");
            parserFromGameTips.AddGameTipsFromFilePath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\GameTips\general.txt");
            parserFromSQLs.AddSQLFromFilePath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\MDD\data\transactions.sql");
            parserFromSQLs.AddSQLFromFilePath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\MDD\data\tagdata.sql");
            BattleTechParser.Lib.Parser parserEnglish = (parserFromJsonFiles + parserFromGameTips + parserFromSQLs);
            parserFromTranslatedCSV.AddCSVFromFilePath(@"d:\Downloads\strings_fr-FR.csv");
            BattleTechParser.Lib.Parser mergedParser = parserFromOriginalCSV.Merge(parserEnglish);
            BattleTechParser.Lib.Parser mergedParser2 = mergedParser.Merge(parserFromTranslatedCSV);
            string tempCSV = mergedParser2.ToCSVFormat();
            string tempCSV2 = parserEnglish.ToCSVFormat();
            string tempCSV3 = (parserFromTranslatedCSV - parserFromOriginalCSV).ToCSVFormat();
            string tempCSV4 = (parserFromOriginalCSV - parserEnglish - parserFromTranslatedCSV).ToCSVFormat();
            string tempCSV5 = (parserEnglish - parserFromOriginalCSV).ToCSVFormat();
            Console.WriteLine("Hello World!");

        }
    }
}
