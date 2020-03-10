using System;

using FluentAssertions;

using Xunit;

namespace MonoidSharp.Tests.OutcomeTests
{
    public class OutcomeTests
    {
        private static string _errorMessageTest = "Error message just for testing";

        [Fact]
        public void Create_GenericOutcome_WithFailure_ShouldBeSuccessfully()
        {
            var result = Outcome.Failed<object>(_errorMessageTest);

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
            result.ErrorMessages
                .Should()
                .NotBeNullOrEmpty()
                    .And
                .HaveCount(1)
                    .And
                .OnlyContain(x => x.Equals(_errorMessageTest));
        }

        [Fact]
        public void Create_GenericOutcome_WithSuccess_ShouldBeSuccessfully()
        {
            var result = Outcome.Successfully<object>(null);

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
            result.ErrorMessages
                .Should()
                .BeNullOrEmpty();
        }

        [Fact]
        public void Create_Outcome_WithFailure_ShouldBeSuccessfully()
        {
            Outcome result = Outcome.Failed(_errorMessageTest);

            result.Failure.Should().BeTrue();
            result.Success.Should().BeFalse();
            result.ErrorMessages
                .Should()
                .NotBeNullOrEmpty()
                    .And
                .HaveCount(1)
                    .And
                .OnlyContain(x => x.Equals(_errorMessageTest));
        }

        [Fact]
        public void Create_Outcome_WithFailure_ShouldThrowException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Outcome.Failed(""));

            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Failed results MUST have to inform an Error Message");
        }

        [Fact]
        public void Create_Outcome_WithSuccess_ShouldBeSuccessfully()
        {
            Outcome result = Outcome.Successfully();

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
            result.ErrorMessages
                .Should()
                .BeNullOrEmpty();
        }
    }
}