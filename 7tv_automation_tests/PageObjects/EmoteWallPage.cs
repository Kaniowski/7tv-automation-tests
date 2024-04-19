using _7tv_automation_tests.Helpers;
using _7tv_automation_tests.Tests;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace _7tv_automation_tests.PageObjects
{
    public class EmoteWallPage
    {
 
        private IWebDriver _driver;
        private ITestOutputHelper TEST_OUTPUT;

        private const int PAGE_LOAD_TIME = 10;
        private const int FILTERS_LOAD_TIME = 5;
        private const int EMOTE_LOAD_TIME = 10;
        private const int SEARCH_INFO_LOAD_TIME = 3;

        private WebDriverWait waitPageLoad;
        private WebDriverWait waitShowFilters;
        private WebDriverWait waitEmotesLoad;
        private WebDriverWait waitSearchInfo;

        public string Url { get; } = "https://7tv.app/emotes?page=1";

        private readonly By loc_searchBar = By.XPath("//input[@type='text']");
        private readonly By loc_filterBtn = By.ClassName("search-filters-button");
        private readonly By loc_emoteBtn = By.XPath("//div[@class='title-banner']");
        private readonly By loc_pageNavigationBtn = By.ClassName("page-button");

        private readonly By loc_searchingInfo = By.ClassName("searching-title");
        private readonly By loc_noEmotesInfo = By.ClassName("no-emotes");
 

        public EmoteWallPage(IWebDriver driver, ITestOutputHelper testOutput)
        {
            _driver = driver;
            TEST_OUTPUT = testOutput;

            waitPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
            waitShowFilters = new WebDriverWait(_driver, TimeSpan.FromSeconds(FILTERS_LOAD_TIME));
            waitEmotesLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(EMOTE_LOAD_TIME));
            waitSearchInfo = new WebDriverWait(_driver, TimeSpan.FromSeconds(SEARCH_INFO_LOAD_TIME));
        }


        public void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
        }


        public IWebElement UseSearchBar(string searchQuery)
        {

            IWebElement searchBar = waitPageLoad.Until(drv => drv.FindElement(loc_searchBar));
            searchBar.SendKeys(searchQuery);

            RefreshUntilLoad(loc_emoteBtn, waitEmotesLoad, 10);

            return searchBar;
        }


        //the website often gets stuck searching endlessly and you have to refresh
        private void RefreshUntilLoad(By locToFind, WebDriverWait waiter, int maxRefreshCount = 10)
        {
            bool foundTheElement = false;
            for(int i = 0; i < maxRefreshCount; i++)
            {
                try
                {
                    IWebElement element = waiter.Until(drv => drv.FindElement(locToFind));
                    foundTheElement = true;
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    TEST_OUTPUT.WriteLine("REFRESHING");
                    TEST_OUTPUT.WriteLine(e.GetBaseException().Message);

                   _driver.Navigate().Refresh();
                    Thread.Sleep(3000);
                };
            }

            if(!foundTheElement)
            {
                throw new Exception($"The test timed out after it was unable to load the element, and refreshing {maxRefreshCount} times");
            }
        }

        public IWebElement RefreshUntilError(int maxRefreshCount=10)
        {
            IWebElement element = null;
            for (int i = 0; i < maxRefreshCount; i++)
            {
                _driver.Navigate().Refresh();
                Thread.Sleep(500);
                _driver.Navigate().Refresh();
                try
                {
                    element = waitSearchInfo.Until(drv => drv.FindElement(loc_noEmotesInfo));
                    //this one means it fizzled, can stop now
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    TEST_OUTPUT.WriteLine(e.GetBaseException().Message);
                };

                try
                {
                    element = waitSearchInfo.Until(drv => drv.FindElement(loc_searchingInfo));
                    Thread.Sleep(2000);
                    element = waitSearchInfo.Until(drv => drv.FindElement(loc_searchingInfo));
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    TEST_OUTPUT.WriteLine(e.GetBaseException().Message);
                };
            }
            return element;
        }


        public void ShowSearchFilters()
        {

            IWebElement filterBtn = waitShowFilters.Until(drv => drv.FindElement(loc_filterBtn));

            filterBtn.Click();
        }

        public void ToggleSearchFilter(string filterName)
        {

            IWebElement filterToggle = _driver.FindElement(By.XPath($"//span[@class='checkmark' and following-sibling::text()[normalize-space()='{filterName}']]"));

            filterToggle.Click();
        }

        public IReadOnlyList<IWebElement> GetEmotes()
        {            
            IReadOnlyList<IWebElement> emoteTitles = waitEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_emoteBtn));
            return emoteTitles;
        }

        public bool CheckIfEmoteIsInSearchResult(string emote)
        {
            IReadOnlyList<IWebElement> emoteTitles = GetEmotes();

            return emoteTitles.ToList().Any(emoteTitle => emoteTitle.Text == emote);

        }


        public IReadOnlyList<IWebElement> GetPageNavigationBtns()
        {
            IReadOnlyList<IWebElement> pageBtns = waitEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_pageNavigationBtn));
            return pageBtns;
        }
        public IWebElement GetLastPageNavigationBtn()
        {
            IReadOnlyList<IWebElement> pageBtns = waitEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_pageNavigationBtn));

            return pageBtns[pageBtns.Count-1];
        }

        //is this necessary?
        public void NavigateToLastEmotePage()
        {
            NavigatePageAndWait(GetLastPageNavigationBtn());
        }

        public void NavigatePageAndWait(IWebElement pageBtn)
        {
            pageBtn.Click();
            RefreshUntilLoad(loc_emoteBtn, waitEmotesLoad, 10);
        }

        //public void NavigateToNextEmotePage(ref IReadOnlyList<IWebElement> pageBtns, int curIndex, int dir)
        //{
        //    curIndex += dir;
        //    if (curIndex < pageBtns.Count) curIndex = pageBtns.Count-1;
        //    if (curIndex >= pageBtns.Count) curIndex = 0;

        //    pageBtns[curIndex].Click();

        //    pageBtns = GetPageNavigationBtns();
        //}

    }
}
