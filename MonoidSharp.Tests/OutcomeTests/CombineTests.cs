using System.Collections.Generic;

using FluentAssertions;

using Xunit;

namespace MonoidSharp.Tests.OutcomeTests
{
    public class CombineTests
    {
        [Fact]
        public void Combine_WithoutErrors_Should_ResultSuccessullyOutcome()
        {
            // Act

            var outcomeSuccess = Outcome.Successfully();

            var resultedOutcome = Outcome.Combine(outcomeSuccess);

            resultedOutcome.Failure
                .Should()
                .BeFalse();

            resultedOutcome.ErrorMessages
                .Should()
                .BeNull();

            resultedOutcome.Success
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Combine_WithTwoErrorMessages_Should_ResultFailureOutcome()
        {
            // Arrange
            string errorMessage_1 = "It has a failure 1";
            string errorMessage_2 = "It has a failure 2";

            var expectedErrors = new List<string>
            {
                errorMessage_1,
                errorMessage_2
            };

            // Act

            var outcomeSuccess = Outcome.Successfully();
            var outcomeFailure = Outcome.Failed(errorMessage_1);
            var outcomeFailure2 = Outcome.Failed(errorMessage_2);

            var resultedOutcome = Outcome.Combine(outcomeSuccess, outcomeFailure, outcomeFailure2);

            // Assert

            resultedOutcome.Failure
                .Should()
                .BeTrue();

            resultedOutcome.ErrorMessages
                .Should()
                .HaveCount(2)
                    .And
                .Contain(expectedErrors);

            resultedOutcome.Success
                .Should()
                .BeFalse();
        }
    }
}