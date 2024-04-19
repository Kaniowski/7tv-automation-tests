using _7tv_automation_tests.Fixtures;
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
            CompareResultsToSearchQuery_WithFilter("#xdddd", "OMEGALUL", "Ignore Tags");

        }


        //should add emoteid string as parameter here, to confirm that you found the right emote
        private void CompareResultsToSearchQuery_WithFilter(string searchQuery, string badResult, string filter)
        {
            EmoteWallPage emoteWallPage = new EmoteWallPage(_fixture.driver, TEST_OUTPUT);

            emoteWallPage.GoTo();
            emoteWallPage.UseSearchBar(searchQuery);

            emoteWallPage.ShowSearchFilters();
            emoteWallPage.ToggleSearchFilter(filter);

            Assert.False(emoteWallPage.CheckIfEmoteIsInSearchResult(badResult));
        }
    }
}