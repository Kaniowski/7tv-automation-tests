using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.Helpers;
using _7tv_automation_tests.PageObjects;
using Xunit.Abstractions;
using OpenQA.Selenium;

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

            //click last page button, get the emotes
            //click second to last pagebutton, get the emotes again
            emoteWallPage.NavigateToLastEmotePage();
            IReadOnlyList<IWebElement> lastPageEmotes = emoteWallPage.GetEmotes();

            IReadOnlyList<IWebElement> navigationBtns = emoteWallPage.GetPageNavigationBtns();
            emoteWallPage.NavigatePageAndWait(navigationBtns[navigationBtns.Count - 2]);

            IReadOnlyList<IWebElement> penultimatePageEmotes = emoteWallPage.GetEmotes();

            TEST_OUTPUT.WriteLine("last page" + lastPageEmotes.Count);
            TEST_OUTPUT.WriteLine("penultimate page" + penultimatePageEmotes.Count);
            lastPageEmotes.ToList().ForEach(emote => TEST_OUTPUT.WriteLine("last page: " + emote.Text));
            penultimatePageEmotes.ToList().ForEach(emote => TEST_OUTPUT.WriteLine("penultimate page: " + emote.Text));

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


        //there are multiple weird behaviours here but this one should be enough to check for it
        [Fact]
        public void WhenChangingResolutionOnLastPageThePageShouldNotChange()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
            emoteWallPage.GoTo();
            emoteWallPage.NavigateToLastEmotePage();

            bool initialLastPageBtnSelectedState= emoteWallPage.GetLastPageNavigationBtn().GetAttribute("selected") =="true";
            string initialLastPageBtnColor = emoteWallPage.GetLastPageNavigationBtn().GetCssValue("background-color");

            _fixture.driver.Manage().Window.Size = new System.Drawing.Size(720, 480);



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
