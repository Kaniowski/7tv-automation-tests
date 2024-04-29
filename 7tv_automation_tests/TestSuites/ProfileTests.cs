using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.Misc;
using _7tv_automation_tests.PageObjects;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace _7tv_automation_tests.Tests
{
    public class ProfileTests : IClassFixture<DriverFixture>
    {

        private readonly DriverFixture _fixture;
        private ITestOutputHelper TEST_OUTPUT;
        public ProfileTests(DriverFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = testFixture;
            TEST_OUTPUT = testOutputHelper;
        }

        [Fact]
        public void ConfirmingProfileSearchWithEnterShouldShowFullProfile()
        {
            PersistentPageElements persistentPage = new PersistentPageElements(_fixture.driver, TEST_OUTPUT);
            persistentPage.GoTo();
            persistentPage.ShowProfileSearchBox();
            IWebElement searchbox = persistentPage.UseProfileSearchBox(DataForTests.testingProfile.profileName);
            persistentPage.GetProfileSearchSuggestion();//waits till you can click enter
            searchbox.SendKeys(Keys.Return);

            UserProfilePage userProfilePage = new UserProfilePage(_fixture.driver, TEST_OUTPUT);
            Assert.True(userProfilePage.GetUserOverview() != null);
        }

        [Fact]
        public void ProfileSearchShouldNotShowDeletedUser()
        {
            PersistentPageElements persistentPage = new PersistentPageElements(_fixture.driver, TEST_OUTPUT);
            persistentPage.GoTo();
            persistentPage.ShowProfileSearchBox();
            persistentPage.UseProfileSearchBox("de");
            IWebElement suggestionBox = persistentPage.GetProfileSearchSuggestion();
            Assert.True(suggestionBox.Text != "*DeletedUser");
            suggestionBox.Click();
            Assert.True(_fixture.driver.Url != "https://7tv.app/users/000000000000000000000000");
        }
    }
}
