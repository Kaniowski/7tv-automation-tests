using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


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
