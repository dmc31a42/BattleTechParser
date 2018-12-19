using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleTechParser;
using Newtonsoft.Json.Linq;

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
            Assert.IsTrue(battleTechParser.AddJsonFromFolderPath("BattleTechParserCan_AddCSVFromFilePath.csv"));
        }

        [TestMethod]
        public void BattleTechParser_GetKeyFromString()
        {
            Assert.AreEqual("amercantileconvoyoperatingundera{team_empfactiondefname}charterneedstotravelthrough{team_tarfactiondefname}-controlledterritorynewlinenewlinetheconvoyiscarryingmedicalsuppliestocombatanoutbreakofarnaahighlycontagioushemorrhagicfeverendemicto{tgt_systemname}newlinenewlineweneedyoutogettheconvoytoarendezvousspotthatwellmarkonyourmapitisabsolutelyvitalthatthevehiclesmakeittotheirdestinationinonepiece!paymentwillberemitteduponreceiptofourmedicinefromtheconvoy", 
                BattleTechParser.Lib.Parser.GetKeyFromString("A mercantile convoy operating under a {TEAM_EMP.FactionDef.Name} charter needs to travel through {TEAM_TAR.FactionDef.Name}-controlled territory.\r\n\r\nThe convoy is carrying medical supplies to combat an outbreak of ARNA, a highly contagious hemorrhagic fever endemic to {TGT_SYSTEM.name}.\r\n\r\nWe need you to get the convoy to a rendezvous spot that we'll mark on your map. It is absolutely vital that the vehicles make it to their destination in one piece! Payment will be remitted upon receipt of our medicine from the convoy."));
            Assert.AreEqual("ballistic:", BattleTechParser.Lib.Parser.GetKeyFromString("\tBallistic:\r"));
            Assert.AreEqual("beforeyoujumpinwewantedtoletyouknowthatbattletechusesparadoxaccountsyoudontneedanaccounttoplaythecampaignorsingle-playerskirmishmodeshoweverherearethreegoodreasonstocreateaparadoxaccount:newlinenewline<indent=5%>1)<#f79b26ff>challengeyourfriends</color>youneedaparadoxaccounttochallengeotherplayersin1v1multiplayermatchesnewlinenewline2)<#f79b26ff>itsabiguniverseandwearejustgettingstarted</color>aparadoxaccountwillkeepyouuptodateaboutnewfeaturesandcontentastheyarereleasednewlinenewline3)<#f79b26ff>getyourjustrewards</color>areyouakickstarterbackerorsomeonewhopre-orderedthegameonastoreotherthansteam?aparadoxaccountishowyougainaccesstoyourspecialgoodiessigninthenredeemyourunlockcodeintheaccountinfowindow(upperrightofthemainmenu)newline</indent>newlinealreadyhaveaparadoxaccountfromapreviousparadoxgameorfromtheparadoxforums?youcanloginandtakeadvantageofthefeaturesaboverightawaynewlinenewlinethankyouandenjoybattletech!",
                BattleTechParser.Lib.Parser.GetKeyFromString("Before you jump in, we wanted to let you know that BATTLETECH uses Paradox Accounts. You don't need an account to play the campaign or single-player skirmish modes. However, here are three good reasons to create a Paradox Account:\r\n\r\n<indent=5%>1) <#F79B26FF>Challenge your friends.</color> You need a Paradox Account to challenge other players in 1v1 multiplayer matches.\r\n\r\n2) <#F79B26FF>It's a big universe and we are just getting started.</color> A Paradox Account will keep you up to date about new features and content as they are released.\r\n\r\n3) <#F79B26FF>Get your just rewards.</color> Are you a Kickstarter Backer or someone who pre-ordered the game on a store other than Steam? A Paradox Account is how you gain access to your special goodies. Sign in, then redeem your unlock code in the Account Info window (upper right of the Main Menu).\r\n\r</indent>\nAlready have a Paradox Account from a previous Paradox game, or from the Paradox forums? You can log in and take advantage of the features above right away.\r\n\r\nThank you, and enjoy BATTLETECH!\r")); // It's a big-universe and  을 It's a big universe and 로 변경하고 테스트 통과
        }
    }
}
