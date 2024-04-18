using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
namespace _7tv_automation_tests.PageObjects
{
    public class EmoteWallPage
    {
 
        private IWebDriver _driver;

        private const int PAGE_LOAD_TIME = 10;
        private const int FILTERS_SHOW_TIME = 5;
        private const int EMOTE_LOAD_TIME = 10;

        private WebDriverWait waitPageLoad;
        private WebDriverWait waitShowFilters;
        private WebDriverWait waitEmotesLoad;

        public string Url { get; } = "https://7tv.app/emotes?page=1";

        private readonly By loc_searchBar = By.XPath("//input[@type='text']");
        private readonly By loc_filterBtn = By.ClassName("search-filters-button");

        public EmoteWallPage(IWebDriver driver)
        {
            _driver = driver;


            waitPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
            waitShowFilters = new WebDriverWait(_driver, TimeSpan.FromSeconds(FILTERS_SHOW_TIME));
            waitEmotesLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(EMOTE_LOAD_TIME));

        }


        public void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
        }

        public void FillSearchBar(string searchQuery)
        {
           
            IWebElement searchBar = waitPageLoad.Until(drv => drv.FindElement(loc_searchBar));
            searchBar.SendKeys(searchQuery);
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
            IReadOnlyList<IWebElement> emoteTitles = waitEmotesLoad.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//div[@class='title-banner']")));
            return emoteTitles;
        }

        public bool CheckIfEmoteIsInSearchResult(string emote)
        {
            IReadOnlyList<IWebElement> emoteTitles = GetEmotes();

            return emoteTitles.ToList().Any(emoteTitle => emoteTitle.Text == emote);

        }

    }
}
