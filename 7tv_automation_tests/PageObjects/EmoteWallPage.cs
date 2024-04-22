using _7tv_automation_tests.Extensions;
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
    public class EmoteWallPage : BasePage
    {
 
        private IWebDriver _driver;
      //  private ITestOutputHelper TEST_OUTPUT;

       // private const int PAGE_LOAD_TIME = 10;
        private const int FILTERS_LOAD_TIME = 5;
        private const int EMOTE_LOAD_TIME = 10;
        private const int SEARCH_INFO_LOAD_TIME = 3;

       // private WebDriverWait waiterPageLoad;
        private WebDriverWait waiterShowFilters;
        private WebDriverWait waiterEmotesLoad;
        private WebDriverWait waiterSearchInfo;

        public override string Url { get; } = "https://7tv.app/emotes?page=1";

        private readonly By loc_emoteSearchBar = By.XPath("//div[@class='text-input'][.//span[text()='Search']]//input[@type='text']");// [.//text()='Search']");
        private readonly By loc_filterBtn = By.ClassName("search-filters-button");
        private readonly By loc_emoteBtn = By.XPath("//div[@class='title-banner']");//why not just by class?
        private readonly By loc_pageNavigationBtn = By.ClassName("page-button");

        private readonly By loc_searchingInfo = By.ClassName("searching-title");
        private readonly By loc_noEmotesInfo = By.ClassName("no-emotes");

        private readonly By loc_filterWidth = By.XPath("//div[@class='text-input'][.//span[text()='Ratio Width']]//input[@type='text']");
        private readonly By loc_filterHeight = By.XPath("//div[@class='text-input'][.//span[text()='Ratio Height']]//input[@type='text']");


        private const int MAX_REFRESH_COUNT = 10;

        public EmoteWallPage(IWebDriver driver, ITestOutputHelper testOutput) : base(driver, testOutput)
        {
            _driver = driver;
           // TEST_OUTPUT = testOutput;

           // waiterPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
            waiterShowFilters = new WebDriverWait(_driver, TimeSpan.FromSeconds(FILTERS_LOAD_TIME));
            waiterEmotesLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(EMOTE_LOAD_TIME));
            waiterSearchInfo = new WebDriverWait(_driver, TimeSpan.FromSeconds(SEARCH_INFO_LOAD_TIME));
        }


        public void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
            RefreshUntilLoad(loc_emoteBtn, waiterPageLoad, MAX_REFRESH_COUNT);
        }

        public IWebElement UseEmoteSearchBar(string searchQuery)
        {
            IWebElement result= PageHelpers.UseAnySearchBar(waiterPageLoad, searchQuery, loc_emoteSearchBar);
            RefreshUntilLoad(loc_emoteBtn, waiterEmotesLoad, MAX_REFRESH_COUNT);
            return result;
        }


        //the website often gets stuck searching endlessly and you have to refresh
        private void RefreshUntilLoad(By locToFind, WebDriverWait waiter, int maxRefreshCount = 10)
        {
            bool foundTheElement = false;
            for(int i = 0; i < maxRefreshCount; i++)
            {
                try
                {
                    waiter.Until(drv => drv.FindElement(locToFind).Displayed);
                    //waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locToFind));
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
                    element = waiterSearchInfo.Until(drv => drv.FindElement(loc_noEmotesInfo));
                    //this one means it fizzled, can stop now
                    break;
                }
                catch (WebDriverTimeoutException e)
                {
                    TEST_OUTPUT.WriteLine(e.GetBaseException().Message);
                };

                try
                {
                    element = waiterSearchInfo.Until(drv => drv.FindElement(loc_searchingInfo));
                    Thread.Sleep(2000);
                    element = waiterSearchInfo.Until(drv => drv.FindElement(loc_searchingInfo));
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

            IWebElement filterBtn = waiterShowFilters.Until(drv => drv.FindElement(loc_filterBtn));
            //IWebElement filterBtn = _driver.ConfidentFind(waiterShowFilters, loc_filterBtn);


            filterBtn.Click();
        }

        public void ToggleSearchFilter(string filterName)
        {

            IWebElement filterToggle = waiterShowFilters.Until(drv => drv.FindElement(By.XPath($"//span[@class='checkmark' and following-sibling::text()[normalize-space()='{filterName}']]")));

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



        //problem with this is if it updates before you call this, it will wait till timeout
        //public void WaitUntilElementTextUpdates(IWebElement element)
        //{
        //    string b = element.Text;
        //    waitEmotesLoad.Until(drv =>element.Text!=b);
        //}
        public void WaitUntilElementBecomesStale(IWebElement element)
        {
            try{
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
            IReadOnlyList<IWebElement> pageBtns = waiterEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(loc_pageNavigationBtn));

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
            RefreshUntilLoad(loc_emoteBtn, waiterEmotesLoad, 10);
        }

    }
}
