using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.PageObjects;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace _7tv_automation_tests.Tests
{
    public class EmoteWallNavigationTests : IClassFixture<DriverFixture>
    {

        private readonly DriverFixture _fixture;
        private ITestOutputHelper TEST_OUTPUT;
        public EmoteWallNavigationTests(DriverFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = testFixture;
            TEST_OUTPUT = testOutputHelper;
        }

        [Fact]
        public void LastAndPenultimatePageShouldBeDifferent()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
            emoteWallPage.GoTo();
            IReadOnlyList<IWebElement> firstPageEmotes = emoteWallPage.GetEmotes();
            emoteWallPage.NavigateToLastEmotePage();
            emoteWallPage.WaitUntilElementBecomesStale(firstPageEmotes[0]);
            Thread.Sleep(3000);//need to do this because of weird desync issues with staleness
            List<string> lastPageEmotes = emoteWallPage.GetEmoteStrings();

            IReadOnlyList<IWebElement> navigationBtns = emoteWallPage.GetPageNavigationBtns();
            Assert.True(navigationBtns.Count >= 2);
            emoteWallPage.NavigatePageAndWait(navigationBtns[navigationBtns.Count - 2]);
            List<string> penultimatePageEmotes = emoteWallPage.GetEmoteStrings();


            //for debugging:
            //TEST_OUTPUT.WriteLine("last page" + lastPageEmotes.Count);
            //TEST_OUTPUT.WriteLine("penultimate page" + penultimatePageEmotes.Count);
            //lastPageEmotes.ForEach(emote => TEST_OUTPUT.WriteLine("last page: " + emote));
            //penultimatePageEmotes.ForEach(emote => TEST_OUTPUT.WriteLine("penultimate page: " + emote));

            Assert.False(lastPageEmotes.SequenceEqual(penultimatePageEmotes));
        }


        [Fact]
        public void WhenMovingToSecondPageTheButtonCountShouldNotChange()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
            emoteWallPage.GoTo();
            IReadOnlyList<IWebElement> navigationBtnsOnFirstPage = emoteWallPage.GetPageNavigationBtns();
            emoteWallPage.NavigatePageAndWait(navigationBtnsOnFirstPage[1]);
            IReadOnlyList<IWebElement> navigationBtnsOnSecondPage = emoteWallPage.GetPageNavigationBtns();

            Assert.True(navigationBtnsOnFirstPage.Count == navigationBtnsOnSecondPage.Count);

        }

        [Fact]
        public void WhenChangingResolutionOnLastPageThePageShouldNotChange()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
            emoteWallPage.GoTo();
            emoteWallPage.NavigateToLastEmotePage();

            bool initialLastPageBtnSelectedState = emoteWallPage.GetLastPageNavigationBtn().GetAttribute("selected") == "true";
            string initialLastPageBtnColor = emoteWallPage.GetLastPageNavigationBtn().GetCssValue("background-color");

            _fixture.driver.Manage().Window.Size = new System.Drawing.Size(720, 480);
            Thread.Sleep(1000);//need to do this because changing the resolution desyncs it with the code and the element becomes stale


            bool finalLastPageBtnSelectedState = emoteWallPage.GetLastPageNavigationBtn().GetAttribute("selected") == "true";
            string finalLastPageBtnColor = emoteWallPage.GetLastPageNavigationBtn().GetCssValue("background-color");

            TEST_OUTPUT.WriteLine($"prev last page {initialLastPageBtnColor}, {initialLastPageBtnSelectedState}");
            TEST_OUTPUT.WriteLine($"prev last page {finalLastPageBtnColor}, {finalLastPageBtnSelectedState}");

            Assert.True(initialLastPageBtnSelectedState == finalLastPageBtnSelectedState, "Last button's selected state should remain the same after changing resolution.");
            Assert.True(initialLastPageBtnColor == finalLastPageBtnColor, "Last button's color should remain the same after changing resolution.");
        }


        [Fact]
        public void FailedLoadShouldNotFizzle()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
            emoteWallPage.GoTo();
            IWebElement endlessSearch = emoteWallPage.RefreshUntilError();
            Assert.True(endlessSearch == null);
        }

    }
}
