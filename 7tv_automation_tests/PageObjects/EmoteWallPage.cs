using _7tv_automation_tests.Extensions;
using _7tv_automation_tests.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;

namespace _7tv_automation_tests.PageObjects
{
    public class EmoteWallPage : BasePage
    {
        private const int EMOTE_LOAD_TIME = 10;
        private const int SEARCH_INFO_LOAD_TIME = 3;

        private const int MAX_REFRESH_COUNT = 10;

        private readonly By loc_emoteSearchBox = By.XPath("//div[@class='text-input'][.//span[text()='Search']]//input[@type='text']");
        private readonly By loc_filterBtn = By.ClassName("search-filters-button");
        private readonly By loc_emoteBtn = By.XPath("//div[@class='title-banner']");
        private readonly By loc_pageNavigationBtn = By.ClassName("page-button");

        private readonly By loc_searchingInfo = By.ClassName("searching-title");
        private readonly By loc_noEmotesInfo = By.ClassName("no-emotes");

        private WebDriverWait waiterEmotesLoad;
        private WebDriverWait waiterSearchInfo;

        public override string Url { get; } = "https://7tv.app/emotes?page=1";

        public EmoteWallPage(IWebDriver driver, ITestOutputHelper testOutput) : base(driver, testOutput)
        {
            waiterEmotesLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(EMOTE_LOAD_TIME));
            waiterSearchInfo = new WebDriverWait(_driver, TimeSpan.FromSeconds(SEARCH_INFO_LOAD_TIME));
        }


        public override void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
            RefreshUntilLoad(loc_emoteBtn, _waiterPageLoad, MAX_REFRESH_COUNT);
        }


        public IWebElement UseEmoteSearchBox(string searchQuery)
        {
            IWebElement result = PageHelpers.UseAnySearchBox(_waiterPageLoad, searchQuery, loc_emoteSearchBox);
            return result;
        }

        public void RefreshUntilEmotesLoad()
        {
            RefreshUntilLoad(loc_emoteBtn, waiterEmotesLoad, MAX_REFRESH_COUNT);
        }


        //the website often gets stuck searching endlessly and you have to refresh
        private void RefreshUntilLoad(By locToFind, WebDriverWait waiter, int maxRefreshCount = 10)
        {
            bool foundTheElement = false;
            for (int i = 0; i < maxRefreshCount; i++)
            {
                try
                {
                    _driver.ConfidentFind(waiter, locToFind);
                    foundTheElement = true;
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    _testOutput.WriteLine("REFRESHING");
                    _testOutput.WriteLine(e.GetBaseException().Message);

                    _driver.Navigate().Refresh();
                    Thread.Sleep(3000);
                };
            }

            if (!foundTheElement)
            {
                throw new Exception($"The test timed out after it was unable to load the element, and refreshing {maxRefreshCount} times");
            }
        }


        //refreshing can cause the emote wall to load infinitely
        public IWebElement RefreshUntilError(int maxRefreshCount = 10)
        {
            IWebElement element = null;
            for (int i = 0; i < maxRefreshCount; i++)
            {
                _driver.Navigate().Refresh();
                Thread.Sleep(500);
                _driver.Navigate().Refresh();
                try
                {
                    element = _driver.ConfidentFind(waiterSearchInfo, loc_noEmotesInfo);
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    _testOutput.WriteLine(e.GetBaseException().Message);
                };

                try
                {
                    element = _driver.ConfidentFind(waiterSearchInfo, loc_searchingInfo);
                    Thread.Sleep(2000);
                    element = _driver.ConfidentFind(waiterSearchInfo, loc_searchingInfo);
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    _testOutput.WriteLine(e.GetBaseException().Message);
                };
            }
            return element;
        }


        public void ShowSearchFilters()
        {

            IWebElement filterBtn = _driver.ConfidentFind(_waiterShort, loc_filterBtn);

            filterBtn.Click();
        }

        public void ToggleSearchFilter(string filterName)
        {
            By loc = By.XPath($"//span[@class='checkmark' and following-sibling::text()[normalize-space()='{filterName}']]");
            IWebElement filterToggle = _driver.ConfidentFind(_waiterShort, loc);

            filterToggle.Click();
        }

        public IReadOnlyList<IWebElement> GetEmotes()
        {
            IReadOnlyList<IWebElement> emoteTitles = waiterEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_emoteBtn));
            return emoteTitles;
        }
        public List<string> GetEmoteStrings()
        {
            return ConvertElementListToTextList(GetEmotes());
        }
        public List<string> ConvertElementListToTextList(IReadOnlyList<IWebElement> emotes)
        {
            return emotes.Select(emote => emote.Text).ToList();
        }

        public void WaitUntilElementBecomesStale(IWebElement element)
        {
            try
            {
                waiterEmotesLoad.Until(ExpectedConditions.StalenessOf(element));
            }
            catch (WebDriverTimeoutException e)
            {
                //if it times out it should mean that the element went stale before this method was called
            }
        }

        public bool CheckIfEmoteIsInSearchResult(string emote)
        {
            IReadOnlyList<IWebElement> emoteTitles = GetEmotes();

            return emoteTitles.ToList().Any(emoteTitle => emoteTitle.Text == emote);

        }


        public IReadOnlyList<IWebElement> GetPageNavigationBtns()
        {
            IReadOnlyList<IWebElement> pageBtns = waiterEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_pageNavigationBtn));
            return pageBtns;
        }
        public IWebElement GetLastPageNavigationBtn()
        {
            IReadOnlyList<IWebElement> pageBtns = GetPageNavigationBtns();

            return pageBtns[pageBtns.Count - 1];
        }

        public void NavigateToLastEmotePage()
        {
            NavigatePageAndWait(GetLastPageNavigationBtn());
        }

        public void NavigatePageAndWait(IWebElement pageBtn)
        {
            pageBtn.Click();
            RefreshUntilLoad(loc_emoteBtn, waiterEmotesLoad, 10);
        }

    }
}
