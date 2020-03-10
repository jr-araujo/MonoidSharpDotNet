using FluentAssertions;

using Xunit;

namespace MonoidSharp.Tests.PossibleBeTests
{
    public class InstanciationTests
    {
        [Fact]
        public void ValidateThat_IsCanAccess_Value_InPossibleBe()
        {
            // Arrange
            var expectedInstance = new object();

            // Act
            PossibleBe<object> possibleBe = expectedInstance;

            // Assert
            possibleBe.HasValue
                .Should()
                .BeTrue();

            possibleBe.HasNoValue
                .Should()
                .BeFalse();

            possibleBe.Value
                .Should()
                .Be(expectedInstance);
        }
    }
}