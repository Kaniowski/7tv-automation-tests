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
    public class PersistentPageElements(IWebDriver driver, ITestOutputHelper testOutput) : BasePage(driver, testOutput)
    {
        private readonly By loc_profileSearchBtn = By.ClassName("nav-button");
        private readonly By loc_profileSearchBar = By.XPath("//div[@class='text-input'][.//span[text()='Search Profiles']]//input[@type='text']");
        private readonly By loc_profileSearchSuggestions = By.XPath("//div[@class='result-tray']//span[@class='user-picture-wrapper']/following-sibling::span[@class='username']");

        public override string Url { get; } = "https://7tv.app";


        public IWebElement GetProfileSearchSuggestion()
        {
            IWebElement searchSuggestions = FindAfterPageLoad(loc_profileSearchSuggestions);

            return searchSuggestions;
        }

      
        public IWebElement ShowProfileSearchBar()
        {
            IWebElement searchBtn = FindAfterPageLoad(loc_profileSearchBtn);
            searchBtn.Click();
            return searchBtn;
        }

        
        public IWebElement UseProfileSearchBar(string searchQuery)
        {
            return PageHelpers.UseAnySearchBar(_waiterPageLoad, searchQuery, loc_profileSearchBar);
        }


    }
}
