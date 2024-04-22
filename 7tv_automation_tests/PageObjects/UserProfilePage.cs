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
    public class UserProfilePage
    {

        private IWebDriver _driver;
        private ITestOutputHelper TEST_OUTPUT;

        private const int PAGE_LOAD_TIME = 10;

        private WebDriverWait waitPageLoad;

        public string Url { get; } = "https://7tv.app/users/";

        private readonly By loc_userOverview = By.ClassName("user-overview");


        public UserProfilePage(IWebDriver driver, ITestOutputHelper testOutput)
        {
            _driver = driver;
            TEST_OUTPUT = testOutput;

            waitPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
        }


        public void GoTo(string profileID)
        {
            _driver.Navigate().GoToUrl(Url + profileID);
        }



        public IWebElement GetUserOverview()
        {
            try
            {
                IWebElement userOverview = waitPageLoad.Until(drv => drv.FindElement(loc_userOverview));
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
