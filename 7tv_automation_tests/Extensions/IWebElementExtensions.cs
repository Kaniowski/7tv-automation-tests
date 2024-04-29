using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace _7tv_automation_tests.Extensions
{
    public static class IWebElementExtensions
    {

        public static IWebElement ConfidentFind(this IWebDriver driver, WebDriverWait waiter, By locator)
        {
            return waiter.Until(drv => drv.FindElement(locator));
        }


    }
}
