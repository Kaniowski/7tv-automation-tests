using _7tv_automation_tests.Helpers;
using _7tv_automation_tests.Tests;
using AngleSharp.Dom;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace _7tv_automation_tests.PageObjects
{
    public class PersistentPageElements
    {
        private const int PAGE_LOAD_TIME = 10;
        private IWebDriver _driver;
        private ITestOutputHelper TEST_OUTPUT;

        public string Url { get; } = "https://7tv.app";


        private readonly By loc_profileSearchBtn = By.ClassName("nav-button");
        private readonly By loc_profileSearchBar = By.XPath("//div[@class='text-input'][.//span[text()='Search Profiles']]//input[@type='text']");
        private readonly By loc_profileSearchSuggestions = By.XPath("//div[@class='result-tray']//span[@class='user-picture-wrapper']/following-sibling::span[@class='username']");

        private WebDriverWait waitPageLoad;
        public PersistentPageElements(IWebDriver driver, ITestOutputHelper testOutput)
        {
            _driver = driver;
            TEST_OUTPUT = testOutput;

            waitPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
        }

        public void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
        }


        public IWebElement GetProfileSearchSuggestion()
        {
            IWebElement searchSuggestions = waitPageLoad.Until(drv => drv.FindElement(loc_profileSearchSuggestions));

            return searchSuggestions;
        }

      
        public IWebElement ShowProfileSearchBar()
        {
            IWebElement searchBtn = waitPageLoad.Until(drv => drv.FindElement(loc_profileSearchBtn));
            searchBtn.Click();
            return searchBtn;
        }

        
        public IWebElement UseProfileSearchBar(string searchQuery)
        {
            return PageHelpers.UseAnySearchBar(waitPageLoad, searchQuery, loc_profileSearchBar);
        }


    }
}
