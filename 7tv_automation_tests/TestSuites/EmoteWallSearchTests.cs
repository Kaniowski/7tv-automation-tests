using _7tv_automation_tests.Fixtures;
using _7tv_automation_tests.Misc;
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
        public void CaseSensitiveFilterShouldIgnoreCapitalLettersWhenSearchingLowerCase()
        {
            CompareResultsToSearchQueryWithFilter("omegalul", "OMEGALUL", "Case Sensitive");
            // CompareResultsToSearchQuery_WithFilter("pog", "OMEGALUL", "Case Sensitive"); //passing result
        }

        //this test could flake if no static emotes are on the first result page due to a change in ranking
        [Fact]
        public void AnimatedFilterShouldIgnoreStaticEmote()
        {
            CompareResultsToSearchQueryWithFilter("OMEGALUL", "OMEGALUL", "Animated");
            // CompareResultsToSearchQuery_WithFilter("pog", "OMEGALUL", "Animated"); //passing result
        }

        //this test could flake if the emote with this tag is not on the first result page due to a change in ranking
        [Fact]
        public void IgnoreTagsFilterShouldIgnoreEmoteWithThisTag()
        {
          
            CompareResultsToSearchQueryWithFilter("#xdddd", "OMEGALUL", "Ignore Tags");  //https://7tv.app/emotes/6042089e77137b000de9e669
            //  CompareResultsToSearchQuery_WithFilter("pog", "OMEGALUL", "Ignore Tags"); //passing result
        }

        private void CompareResultsToSearchQueryWithFilter(string searchQuery, string badResult, string filter)
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);

            emoteWallPage.GoTo();
            emoteWallPage.ShowSearchFilters();
            emoteWallPage.ToggleSearchFilter(filter);

            emoteWallPage.UseEmoteSearchBox(searchQuery);
            emoteWallPage.RefreshUntilEmotesLoad();


            Assert.False(emoteWallPage.CheckIfEmoteIsInSearchResult(badResult));
        }



        [Fact]
        public void SearchFieldStringBoundsShouldBeTruncated()
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);

            emoteWallPage.GoTo();
            IWebElement searchBox = emoteWallPage.UseEmoteSearchBox(DataForTests.longString);

            string searchQuery = searchBox.GetAttribute("value");
            if (searchQuery.Length == 0) { throw new Exception("the query probably failed completely"); }
            Assert.False(searchQuery.Length == DataForTests.longString.Length);
        }


    }
}