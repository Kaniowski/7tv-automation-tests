using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace _7tv_automation_tests.Fixtures
{
    public class DriverFixture : IDisposable
    {
        public IWebDriver driver;
        public DriverFixture()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            ChromeOptions options = new ChromeOptions();
            // options.AddArgument("--headless=new");
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

        }

        public void Dispose()
        {
            // Thread.Sleep(5000); //for simple final result checking
            driver?.Quit();
        }
    }
}
