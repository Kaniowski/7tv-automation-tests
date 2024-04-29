using OpenQA.Selenium;
using Xunit.Abstractions;

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
                IWebElement userOverview = FindAfterShortLoad(loc_userOverview);
                return userOverview;
            }
            catch
            {
                //if it times out, it means you could not find the object, i.e. it did not load
                return null;
            }

        }


    }
}
