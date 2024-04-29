using _7tv_automation_tests.Helpers;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace _7tv_automation_tests.PageObjects
{
    public class PersistentPageElements(IWebDriver driver, ITestOutputHelper testOutput) : BasePage(driver, testOutput)
    {
        private readonly By loc_profileSearchBtn = By.ClassName("nav-button");
        private readonly By loc_profileSearchBox = By.XPath("//div[@class='text-input'][.//span[text()='Search Profiles']]//input[@type='text']");
        private readonly By loc_profileSearchSuggestions = By.XPath("//div[@class='result-tray']//span[@class='user-picture-wrapper']/following-sibling::span[@class='username']");

        public override string Url { get; } = "https://7tv.app";


        public IWebElement GetProfileSearchSuggestion()
        {
            IWebElement searchSuggestions = FindAfterPageLoad(loc_profileSearchSuggestions);

            return searchSuggestions;
        }


        public IWebElement ShowProfileSearchBox()
        {
            IWebElement searchBtn = FindAfterPageLoad(loc_profileSearchBtn);
            searchBtn.Click();
            return searchBtn;
        }


        public IWebElement UseProfileSearchBox(string searchQuery)
        {
            return PageHelpers.UseAnySearchBox(_waiterPageLoad, searchQuery, loc_profileSearchBox);
        }


    }
}
