using Forum.Services.Pagging;
using Xunit;

namespace Forum.Services.UnitTests.Pagging
{
    public class PaggingServiceTests
    {
        private readonly PaggingService paggingService;

        public PaggingServiceTests()
        {
            this.paggingService = new PaggingService();
        }

        [Fact]
        public void GetPagesCount_returns_correct_answer()
        {
            var expectedResult = 5;

            var actualResult = this.paggingService.GetPagesCount(25);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}