using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.Helpers;
using _7tv_automation_tests.PageObjects;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace _7tv_automation_tests.Tests
{
    public class EmoteWallSearchTests : IClassFixture<DriverFixture>
    {
        private readonly DriverFixture _fixture;

        private ITestOutputHelper TEST_OUTPUT;
        public EmoteWallSearchTests(DriverFixture testFixture, ITestOutputHelper testOutputHelper) 
        {
            _fixture = testFixture;
            TEST_OUTPUT = testOutputHelper;
        }




        //this test could flake if no emotes with the failing keyword are on the first result page due to a change in ranking
        [Fact]
        public void CaseSensitiveFilter_ShouldNotShowCapitalLetters_WhenSearchingLowerCase()
        {
            CompareResultsToSearchQuery_WithFilter("omegalul", "OMEGALUL", "Case Sensitive");
        }

        //this test could flake if no static emotes are on the first result page due to a change in ranking
        [Fact]
        public void AnimatedFilter_ShouldNotShowStaticEmote()
        {
            CompareResultsToSearchQuery_WithFilter("OMEGALUL", "OMEGALUL", "Animated");
        }

        //this test could flake if the emote with this tag is not on the first result page due to a change in ranking
        [Fact]
        public void IgnoreTagsFilter_ShouldNotShowEmoteWithThisTag()
        {
            //https://7tv.app/emotes/6042089e77137b000de9e669
            CompareResultsToSearchQuery_WithFilter("#xdddd", "OMEGALUL", "Ignore Tags");

        }


        //should add emoteid string as parameter here, to confirm that you found the right emote
        private void CompareResultsToSearchQuery_WithFilter(string searchQuery, string badResult, string filter)
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);

            emoteWallPage.GoTo();
            emoteWallPage.ShowSearchFilters();
            emoteWallPage.ToggleSearchFilter(filter);

            emoteWallPage.UseEmoteSearchBar(searchQuery);



            Assert.False(emoteWallPage.CheckIfEmoteIsInSearchResult(badResult));
        }



        [Fact]
        public void SearchFieldStringBounds_ShouldBeTruncated()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);
           
            emoteWallPage.GoTo();
            IWebElement searchBar= emoteWallPage.UseEmoteSearchBar(DataForTests.longString);

            //Thread.Sleep(3000);
            //TEST_OUTPUT.WriteLine("DataHelper.longString.Length: " + DataHelper.longString.Length);
            //TEST_OUTPUT.WriteLine("searchbar.value: " + searchBar.GetAttribute("value").Length);
            //TEST_OUTPUT.WriteLine("url: " + _fixture.driver.Url);

            //    Assert.False(searchBar.GetAttribute("value") == DataHelper.longString);
            string searchQuery = searchBar.GetAttribute("value");
            if (searchQuery.Length == 0) { throw new Exception("the query probably failed completely"); }
            Assert.False(searchQuery.Length == DataForTests.longString.Length);
        }

        
    }
}