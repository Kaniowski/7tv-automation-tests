using _7tv_automation_tests.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace _7tv_automation_tests.PageObjects
{
    public abstract class BasePage
    {
        private const int PAGE_LOAD_TIME = 10;
        private const int LONG_TIME = 30;
        private const int SHORT_TIME = 5;

        public abstract string Url { get; }


        protected ITestOutputHelper TEST_OUTPUT;

        protected WebDriverWait _waiterLong;
        protected WebDriverWait _waiterShort;
        protected WebDriverWait _waiterPageLoad;

        protected IWebDriver _driver;



        public BasePage(IWebDriver newDriver, ITestOutputHelper testOutput)
        {
            _driver = newDriver;
            TEST_OUTPUT = testOutput;
            _waiterPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
            _waiterLong = new WebDriverWait(_driver, TimeSpan.FromSeconds(LONG_TIME));
            _waiterShort = new WebDriverWait(_driver, TimeSpan.FromSeconds(SHORT_TIME));
        }


        public virtual void GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
        }

        public IWebElement FindAfterPageLoad(By locator)
        {
            return _driver.ConfidentFind(_waiterPageLoad, locator);
        }

        public IWebElement FindAfterShortLoad(By locator)
        {
            return _driver.ConfidentFind(_waiterShort, locator);
        }

        public IWebElement FindAfterLongLoad(By locator)
        {
            return _driver.ConfidentFind(_waiterLong, locator);
        }
    }
}
