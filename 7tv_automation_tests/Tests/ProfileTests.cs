using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.Helpers;
using _7tv_automation_tests.PageObjects;
using Xunit.Abstractions;
using OpenQA.Selenium;

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


        //the tests should be grouped differently maybe, this should go into some 'navigationtests' maybe
        //you should restructure this, so that you wait HERE, and get the overview INSTANTLY once you've waited (to move some of the logic to the test instead of the page)
        [Fact]
        public void ConfirmingProfileSearchWithEnter_ShouldShowFullProfile()
        {
            PersistentPageElements persistentPage = new PersistentPageElements(_fixture.driver, TEST_OUTPUT);
            persistentPage.GoTo();
            persistentPage.ShowProfileSearchBar();
            IWebElement searchbox = persistentPage.UseProfileSearchBar(DataForTests.testingProfile.profileName);
            persistentPage.GetProfileSearchSuggestion();//wait till you can click enter
            searchbox.SendKeys(Keys.Return);//might need to click first

            UserProfilePage userProfilePage = new UserProfilePage(_fixture.driver, TEST_OUTPUT);
            Assert.True(userProfilePage.GetUserOverview() != null);
        }

        [Fact(Skip ="not finished")]
        public void EmoteSetShouldNotLoadEveryEmote()
        {
            UserProfilePage userProfilePage = new UserProfilePage(_fixture.driver, TEST_OUTPUT);
            userProfilePage.GoTo(DataForTests.testingProfile.profileId);
            //using 'DataHelper.testingProfile.emoteSetId' locate the dataset on the profile, click it
            //then get a list of loaded emotes and if it's bigger than X big number then not good
            //technically you don't need to do navigation like this for this one, just go straight to the emoteset page
            //and this one could just end at navigation to the emoteset, and stop there (simply checking if navigation works)


           
        }


        [Fact]
        public void ProfileSearchShouldNotShowDeletedUser()
        {
            PersistentPageElements persistentPage = new PersistentPageElements(_fixture.driver, TEST_OUTPUT);
            persistentPage.GoTo();
            persistentPage.ShowProfileSearchBar();
            persistentPage.UseProfileSearchBar("de");
            IWebElement suggestionBox= persistentPage.GetProfileSearchSuggestion();
            Assert.True(suggestionBox.Text != "*DeletedUser");
            suggestionBox.Click();
            Assert.True(_fixture.driver.Url != "https://7tv.app/users/000000000000000000000000");//should maybe wait to load first?
           
        }
    }
}
