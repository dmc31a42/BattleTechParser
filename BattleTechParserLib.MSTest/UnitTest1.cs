using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleTechParser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BattleTechParserLib.MSTest
{
    [TestClass]
    public class UnitTest1
    {
        private BattleTechParser.Lib.Parser battleTechParserFull = new BattleTechParser.Lib.Parser();
        private BattleTechParser.Lib.Parser battleTechParserFullMinus2 = new BattleTechParser.Lib.Parser();
        private BattleTechParser.Lib.Parser battleTechParserOnly2 = new BattleTechParser.Lib.Parser();
        private BattleTechParser.Lib.Parser battleTechParserOnly2SomeDiff = new BattleTechParser.Lib.Parser();

        public UnitTest1()
        {
            battleTechParserFull.AddCSVFromFilePath("BattleTechParserCan_AddCSVFromFilePath.csv");
            battleTechParserFullMinus2.AddCSVFromFilePath("BattleTechParser_IsEqualItSelf.csv");
            battleTechParserOnly2.AddCSVFromFilePath("BattleTechParser_OperatorMinus.csv");
            battleTechParserOnly2SomeDiff.AddCSVFromFilePath("BattleTechParser_OperatorMinusSomeDiff.csv");
        }

        [TestMethod]
        public void BattleTechParserCan_AddCSVFromFilePath()
        {
            BattleTechParser.Lib.Parser battleTechParser = new BattleTechParser.Lib.Parser();
            Assert.IsTrue(battleTechParser.AddCSVFromFilePath("BattleTechParserCan_AddCSVFromFilePath.csv"));
            Assert.AreEqual(battleTechParser.Count, 26);
        }

        [TestMethod]
        public void BattleTechParserCannot_AddCSVFromFilePath()
        {
            BattleTechParser.Lib.Parser battleTechParser = new BattleTechParser.Lib.Parser();
            Assert.IsFalse(battleTechParser.AddCSVFromFilePath("ndjfnqlaeiurnvoaliwjrngaoeurgbnaeljrgna.csv"));
        }

        [TestMethod]
        public void BattleTechParser_SplitLine()
        {
            CollectionAssert.AreEqual(new string[] { "KEY", "fr-FR" }, BattleTechParser.Lib.Parser.SplitLine(@"KEY,fr-FR")); // split between comma
            CollectionAssert.AreEqual(null, BattleTechParser.Lib.Parser.SplitLine(System.IO.File.ReadAllText("SplitLineTest2.txt"))); // if key is empty, return null
            CollectionAssert.AreEqual(new string[] { "####remaining", "##,## Restant" }, BattleTechParser.Lib.Parser.SplitLine(System.IO.File.ReadAllText("SplitLineTest3.txt"))); // if contain 0x1F, change it into comma
        }

        [TestMethod]
        public void BattleTechParser_Equals()
        {
            BattleTechParser.Lib.Parser battleTechParser1 = new BattleTechParser.Lib.Parser();
            battleTechParser1.AddCSVFromFilePath("BattleTechParserCan_AddCSVFromFilePath.csv");
            Assert.AreEqual(battleTechParserFull, battleTechParser1);
            Assert.AreNotEqual(battleTechParserFull, battleTechParserFullMinus2);
            Assert.AreNotEqual(battleTechParserFull, battleTechParserOnly2);
            Assert.AreNotEqual(battleTechParserOnly2, battleTechParserOnly2SomeDiff);
        }

        [TestMethod]
        public void BattleTechParser_OperatorMinus()
        {
            Assert.AreEqual(battleTechParserOnly2, battleTechParserFull - battleTechParserFullMinus2);
            Assert.AreEqual(new BattleTechParser.Lib.Parser(), new BattleTechParser.Lib.Parser() - new BattleTechParser.Lib.Parser());
            Assert.AreEqual(new BattleTechParser.Lib.Parser(), battleTechParserFull - battleTechParserFull);
            Assert.AreEqual(new BattleTechParser.Lib.Parser(), battleTechParserFullMinus2 - battleTechParserFull);
            Assert.AreEqual(battleTechParserFullMinus2, battleTechParserFull - battleTechParserOnly2);
        }

        [TestMethod]
        public void BattleTechParser_OperatorPlus()
        {
            Assert.AreEqual(new BattleTechParser.Lib.Parser(), new BattleTechParser.Lib.Parser() + new BattleTechParser.Lib.Parser());
            Assert.AreEqual(battleTechParserFull, battleTechParserFull + new BattleTechParser.Lib.Parser());
            Assert.AreEqual(battleTechParserFull, battleTechParserFull + battleTechParserFull);
            Assert.AreEqual(battleTechParserFull, battleTechParserFull + battleTechParserFullMinus2);
            Assert.AreEqual(battleTechParserFull, battleTechParserFullMinus2 + battleTechParserOnly2);
            Assert.AreEqual(battleTechParserOnly2SomeDiff, battleTechParserOnly2 + battleTechParserOnly2SomeDiff);
            Assert.AreEqual(battleTechParserOnly2, battleTechParserOnly2SomeDiff + battleTechParserOnly2);
        }

        [TestMethod]
        public void BattleTechParser_AddJsonFromFolderPath()
        {
            BattleTechParser.Lib.Parser battleTechParser = new BattleTechParser.Lib.Parser();
            Assert.IsTrue(battleTechParser.AddJsonFromFolderPath(@"D:\SteamLibrary\steamapps\common\BATTLETECH\BattleTech_Data\StreamingAssets\data"));
        }

        [TestMethod]
        public void BattleTechParser_GetKeyFromString()
        {
            Assert.AreEqual(@"amercantileconvoyoperatingundera{team_emp*factiondef*name}charterneedstotravelthrough{team_tar*factiondef*name}-controlledterritory*newlinenewlinetheconvoyiscarryingmedicalsuppliestocombatanoutbreakofarna^ahighlycontagioushemorrhagicfeverendemicto{tgt_system*name}*newlinenewlineweneedyoutogettheconvoytoarendezvousspotthatwellmarkonyourmap*itisabsolutelyvitalthatthevehiclesmakeittotheirdestinationinonepiece!paymentwillberemitteduponreceiptofourmedicinefromtheconvoy*", 
                BattleTechParser.Lib.Parser.GetKeyFromJsonString("A mercantile convoy operating under a {TEAM_EMP.FactionDef.Name} charter needs to travel through {TEAM_TAR.FactionDef.Name}-controlled territory.\r\n\r\nThe convoy is carrying medical supplies to combat an outbreak of ARNA, a highly contagious hemorrhagic fever endemic to {TGT_SYSTEM.name}.\r\n\r\nWe need you to get the convoy to a rendezvous spot that we'll mark on your map. It is absolutely vital that the vehicles make it to their destination in one piece! Payment will be remitted upon receipt of our medicine from the convoy."));
            Assert.AreEqual("ballistic:", BattleTechParser.Lib.Parser.GetKeyFromJsonString("\tBallistic:\r"));
            Assert.AreEqual("beforeyoujumpin^wewantedtoletyouknowthatbattletechusesparadoxaccounts*youdontneedanaccounttoplaythecampaignorsingle-playerskirmishmodes*however^herearethreegoodreasonstocreateaparadoxaccount:newlinenewline1)challengeyourfriends*youneedaparadoxaccounttochallengeotherplayersin1v1multiplayermatches*newlinenewline2)itsabiguniverseandwearejustgettingstarted*aparadoxaccountwillkeepyouuptodateaboutnewfeaturesandcontentastheyarereleased*newlinenewline3)getyourjustrewards*areyouakickstarterbackerorsomeonewhopre-orderedthegameonastoreotherthansteam?aparadoxaccountishowyougainaccesstoyourspecialgoodies*signin^thenredeemyourunlockcodeintheaccountinfowindow(upperrightofthemainmenu)*newlinenewlinealreadyhaveaparadoxaccountfromapreviousparadoxgame^orfromtheparadoxforums?youcanloginandtakeadvantageofthefeaturesaboverightaway*newlinenewlinethankyou^andenjoybattletech!",
                BattleTechParser.Lib.Parser.GetKeyFromJsonString("Before you jump in, we wanted to let you know that BATTLETECH uses Paradox Accounts. You don't need an account to play the campaign or single-player skirmish modes. However, here are three good reasons to create a Paradox Account:\r\n\r\n<indent=5%>1) <#F79B26FF>Challenge your friends.</color> You need a Paradox Account to challenge other players in 1v1 multiplayer matches.\r\n\r\n2) <#F79B26FF>It's a big universe and we are just getting started.</color> A Paradox Account will keep you up to date about new features and content as they are released.\r\n\r\n3) <#F79B26FF>Get your just rewards.</color> Are you a Kickstarter Backer or someone who pre-ordered the game on a store other than Steam? A Paradox Account is how you gain access to your special goodies. Sign in, then redeem your unlock code in the Account Info window (upper right of the Main Menu).\r\n\r</indent>\nAlready have a Paradox Account from a previous Paradox game, or from the Paradox forums? You can log in and take advantage of the features above right away.\r\n\r\nThank you, and enjoy BATTLETECH!\r")); // It's a big-universe and  을 It's a big universe and 로 변경하고 테스트 통과
        }

        [TestMethod]
        public void BattleTechParser_GetKeyValueFromJObject()
        {
            JObject jObject = new JObject(
                new JProperty("test", "\tBallistic:\r"));

            Assert.AreEqual(new Tuple<string, string>("ballistic:", "\tBallistic:\r"), BattleTechParser.Lib.Parser.GetKeyValueFromJObject((JToken)jObject["test"]));
            try
            {
                BattleTechParser.Lib.Parser.GetKeyValueFromJObject(jObject);
                Assert.Fail();
            } catch (Exception)
            {

            }
        }

        [TestMethod]
        public void BattleTechParser_ReadKeyValuesFromJObject()
        {
            Dictionary<string, string> tempDict = new Dictionary<string, string>(
                new KeyValuePair<string, string>[]{
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("AbilityDefCMD_FireBase"),"AbilityDefCMD_FireBase"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("FIRE BASE"),"FIRE BASE"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("DEPLOY FIRE BASE (AC/10)"),"DEPLOY FIRE BASE (AC/10)"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("uixSvgIcon_genericDiamond"),"uixSvgIcon_genericDiamond"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("CommandAbility"),"CommandAbility"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("SpawnTurret"),"SpawnTurret"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("CommandSpawnPosition"),"CommandSpawnPosition"),
                    new KeyValuePair<string, string>(BattleTechParser.Lib.Parser.GetKeyFromJsonString("turretdef_TestTurret_AC101_360"),"turretdef_TestTurret_AC101_360"),
                }
            );
            string tempJsonStr = @"{
	""Description"" : {
		""Id"" : ""AbilityDefCMD_FireBase"",
		""Name"" : ""FIRE BASE"",
		""Details"" : ""DEPLOY FIRE BASE (AC/10)"",
		""Icon"" : ""uixSvgIcon_genericDiamond""
	},
	""ActivationTime"" : ""CommandAbility"",
	""Resource"" : ""CommandAbility"",
	""ActivationCooldown"" : -1,
	""NumberOfUses"" : 1,
	""specialRules"" : ""SpawnTurret"",
	""Targeting"" : ""CommandSpawnPosition"",
	""ActorResource"" : ""turretdef_TestTurret_AC101_360"",
    ""FloatParam1"" : 50,
    ""FloatParam2"" : 50
}
";
            JObject jObject = JObject.Parse(tempJsonStr);
            CollectionAssert.AreEqual(tempDict, BattleTechParser.Lib.Parser.ReadKeyValuesFromJObject(jObject));
        }

        [TestMethod]
        public void BattleTechParser_Merge()
        {
            BattleTechParser.Lib.Parser battleTechParserMergeFrom = new BattleTechParser.Lib.Parser();
            battleTechParserMergeFrom.AddCSVFromFilePath("BattleTechParserCan_Merge.csv");
            BattleTechParser.Lib.Parser battleTechParserMergeReference = new BattleTechParser.Lib.Parser();
            battleTechParserMergeReference.AddCSVFromFilePath("BattleTechParserCan_Merge_Reference.csv");
            Assert.AreEqual(battleTechParserMergeReference, battleTechParserFull.Merge(battleTechParserMergeFrom));
        }

        [TestMethod]
        public void BattleTechParser_ToCSV()
        {
            string csvStr = battleTechParserFull.ToCSVFormat();
            BattleTechParser.Lib.Parser parser = new BattleTechParser.Lib.Parser();
            parser.AddCSVFromString(csvStr);
            Assert.AreEqual(csvStr, parser.ToCSVFormat());
        }

        [TestMethod]
        public void BattleTechParser_ToCSVStringFormat()
        {
            Assert.AreEqual("Before you jump in\u001f we wanted to let you know that BATTLETECH uses Paradox Accounts. You don't need an account to play the campaign or single-player skirmish modes. However\u001f here are three good reasons to create a Paradox Account:\\r\\n\\r\\n<indent=5%>1) <#F79B26FF>Challenge your friends.</color> You need a Paradox Account to challenge other players in 1v1 multiplayer matches.\\r\\n\\r\\n2) <#F79B26FF>It's a big universe and we are just getting started.</color> A Paradox Account will keep you up to date about new features and content as they are released.\\r\\n\\r\\n3) <#F79B26FF>Get your just rewards.</color> Are you a Kickstarter Backer or someone who pre-ordered the game on a store other than Steam? A Paradox Account is how you gain access to your special goodies. Sign in\u001f then redeem your unlock code in the Account Info window (upper right of the Main Menu).\\r\\n\\r</indent>\\nAlready have a Paradox Account from a previous Paradox game\u001f or from the Paradox forums? You can log in and take advantage of the features above right away.\\r\\n\\r\\nThank you\u001f and enjoy BATTLETECH!\\r",
                BattleTechParser.Lib.Parser.ToCSVStringFormat("Before you jump in, we wanted to let you know that BATTLETECH uses Paradox Accounts. You don't need an account to play the campaign or single-player skirmish modes. However, here are three good reasons to create a Paradox Account:\r\n\r\n<indent=5%>1) <#F79B26FF>Challenge your friends.</color> You need a Paradox Account to challenge other players in 1v1 multiplayer matches.\r\n\r\n2) <#F79B26FF>It's a big universe and we are just getting started.</color> A Paradox Account will keep you up to date about new features and content as they are released.\r\n\r\n3) <#F79B26FF>Get your just rewards.</color> Are you a Kickstarter Backer or someone who pre-ordered the game on a store other than Steam? A Paradox Account is how you gain access to your special goodies. Sign in, then redeem your unlock code in the Account Info window (upper right of the Main Menu).\r\n\r</indent>\nAlready have a Paradox Account from a previous Paradox game, or from the Paradox forums? You can log in and take advantage of the features above right away.\r\n\r\nThank you, and enjoy BATTLETECH!\r"));
        }

        [TestMethod]
        public void BattleTechParser_ToPOT()
        {
            string tempPot = "msgctxt \"KEY\"\nmsgid \"fr-FR\"\nmsgstr \"\"\n\nmsgctxt \"-\"\nmsgid \"\"\nmsgstr \"\"\n\nmsgctxt \"--\"\nmsgid \"- -\"\nmsgstr \"\"\n\nmsgctxt \"---\"\nmsgid \"\"\nmsgstr \"\"\n\nmsgctxt \"----\"\nmsgid \"----\"\nmsgstr \"\"\n\nmsgctxt \"-------\"\nmsgid \"--- - ---\"\nmsgstr \"\"\n\nmsgctxt \"-----------------------------------------------\"\nmsgid \"-----------------------------------------------\"\nmsgstr \"\"\n\nmsgctxt \"!\"\nmsgid \"!\"\nmsgstr \"\"\n\nmsgctxt \"!!!overheatalert!!!\"\nmsgid \"<nobr>!!! ALERTE DE SURCHAUFFE !</nobr>!!</nobr>\"\nmsgstr \"\"\n\nmsgctxt \"!!!shutdownalert!!!\"\nmsgid \"<nobr>!!! ALERTE D'ARRÊT !</nobr>!!</nobr>\"\nmsgstr \"\"\n\nmsgctxt \"!surrender!\"\nmsgid \"<nobr>! REDDITION !</nobr>\"\nmsgstr \"\"\n\nmsgctxt \"#\"\nmsgid \"\"\nmsgstr \"\"\n\nmsgctxt \"##\"\nmsgid \"##\"\nmsgstr \"\"\n\nmsgctxt \"###\"\nmsgid \"###\"\nmsgstr \"\"\n\nmsgctxt \"#####\"\nmsgid \"## ###\"\nmsgstr \"\"\n\nmsgctxt \"######\"\nmsgid \"### ###\"\nmsgstr \"\"\n\nmsgctxt \"#######\"\nmsgid \"# ### ###\"\nmsgstr \"\"\n\nmsgctxt \"########\"\nmsgid \"## ### ###\"\nmsgstr \"\"\n\nmsgctxt \"-##########\"\nmsgid \"-# ### ### ###\"\nmsgstr \"\"\n\nmsgctxt \"########c-bills\"\nmsgid \"## ### ### C-Bills\"\nmsgstr \"\"\n\nmsgctxt \"#####gjnewline\"\nmsgid \"## ### GJ\\n\"\nmsgstr \"\"\n\nmsgctxt \"#####m\"\nmsgid \"## ### m\"\nmsgstr \"\"\n\nmsgctxt \"####remaining\"\nmsgid \"##,## Restant\"\nmsgstr \"\"\n\nmsgctxt \"####xp\"\nmsgid \"#### EXP\"\nmsgstr \"\"\n\nmsgctxt \"###%\"\nmsgid \"<nobr>##,# %</nobr>\"\nmsgstr \"\"\n\nmsgctxt \"###0m\"\nmsgid \"#,##0,,M\"\nmsgstr \"\"\n";
            string potStr = battleTechParserFull.ToPotFormat();
            tempPot = tempPot.Replace("\n", "\r\n");
            potStr = potStr.Replace("\n", "\r\n");
            Assert.AreEqual(tempPot.Trim(), potStr.Trim());

        }

        [TestMethod]
        public void BattleTechParser_FromPot()
        {
            string potStr = battleTechParserFull.ToPotFormat();
            BattleTechParser.Lib.Parser battleTechParser1 = new BattleTechParser.Lib.Parser();
            battleTechParser1.FromPotString(potStr);
            Assert.AreEqual(battleTechParserFull, battleTechParser1);
        }
    }
}
