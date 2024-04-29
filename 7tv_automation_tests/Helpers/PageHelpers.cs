using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace _7tv_automation_tests.Helpers
{
    public static class PageHelpers
    {
        public static IWebElement UseAnySearchBox(WebDriverWait waiter, string searchQuery, By searchBoxLocator)
        {
            IWebElement searchBox = waiter.Until(drv => drv.FindElement(searchBoxLocator));
            searchBox.SendKeys(searchQuery);

            return searchBox;
        }
    }
}
