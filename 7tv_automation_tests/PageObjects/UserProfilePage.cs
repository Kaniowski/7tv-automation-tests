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
    public class UserProfilePage(IWebDriver driver, ITestOutputHelper testOutput) : BasePage(driver, testOutput)
    {
        public override string Url { get; } = "https://7tv.app/users/";

        private readonly By loc_userOverview = By.ClassName("user-overview");

        public void GoTo(string profileID)
        {
            _driver.Navigate().GoToUrl(Url + profileID);
        }


        public IWebElement GetUserOverview()
        {
            try
            {
                IWebElement userOverview = FindAfterPageLoad(loc_userOverview);
                return userOverview;
            }
            catch
            {
                //if it times out, means you could not find the object, meaning it did not load
                return null;
            }
           
        }


    }
}
