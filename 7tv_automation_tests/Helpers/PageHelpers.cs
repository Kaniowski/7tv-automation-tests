using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace _7tv_automation_tests.Helpers
{
    public static class PageHelpers
    {
        public static IWebElement UseAnySearchBar(WebDriverWait waiter, string searchQuery, By searchBoxLocator)
        {
            IWebElement searchBar = waiter.Until(drv => drv.FindElement(searchBoxLocator));
            searchBar.SendKeys(searchQuery);

            return searchBar;
        }
    }
}
