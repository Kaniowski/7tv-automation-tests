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
        protected WebDriverWait waiterPageLoad;
        public WebDriverWait waiterLong;
        public WebDriverWait waiterShort;

        protected IWebDriver driver;
        public ITestOutputHelper TEST_OUTPUT;

        public abstract string Url { get; }
        public BasePage(IWebDriver newDriver, ITestOutputHelper testOutput)
        {
            driver = newDriver;
            TEST_OUTPUT = testOutput;
            waiterPageLoad = new WebDriverWait(driver, TimeSpan.FromSeconds(PAGE_LOAD_TIME));
            waiterLong = new WebDriverWait(driver, TimeSpan.FromSeconds(LONG_TIME));
            waiterShort = new WebDriverWait(driver, TimeSpan.FromSeconds(SHORT_TIME));
        }
    }
}
