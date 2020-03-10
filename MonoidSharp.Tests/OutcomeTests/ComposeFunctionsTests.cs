using System.Linq;

using FluentAssertions;

using MonoidSharp.Extensions;

using Xunit;
using Xunit.Abstractions;

namespace MonoidSharp.Tests.OutcomeTests
{
    public class ComposeFunctionsTests
    {
        private readonly ITestOutputHelper output;

        public ComposeFunctionsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Compose_Functions_ShouldResultFailureOutcome()
        {
            // Act
            PossibleBe<CustomerTest> possibleBeCustomer = new CustomerTest();
            Outcome<CustomerTest> outcomeCustomer = possibleBeCustomer.TranslateToOutcome("error");

            var outcomeResult = outcomeCustomer
                              .Bind(outcome => Test(outcome))
                              .Bind(outcome2 => Test3(outcome2))
                                    .WhenFailDo(outcome => Log(outcome.ErrorMessages.First()))
                              .Return(x => x.Success ? 1 : -1);

            outcomeResult
                .Should()
                .Be(-1);
        }

        [Fact]
        public void Compose_Functions_ShouldResultSuccessullyOutcome()
        {
            // Act
            PossibleBe<CustomerTest> possibleBeCustomer = new CustomerTest();
            Outcome<CustomerTest> outcomeCustomer = possibleBeCustomer.TranslateToOutcome("error");

            var outcomeResult = outcomeCustomer
                              .Bind(firstParameter => Test(firstParameter))
                              .Bind(parameterFromPreviousExecution => Test2(parameterFromPreviousExecution));

            outcomeResult.Failure
                .Should()
                .BeFalse();

            outcomeResult.ErrorMessages
                .Should()
                .NotBeNull()
                    .And
                .HaveCount(0);

            outcomeResult.Success
                .Should()
                .BeTrue();
        }

        private void Log(string logMessage)
        {
            output.WriteLine($"Log message: {logMessage}");
        }

        private Outcome<int> Test(PossibleBe<CustomerTest> customer)
        {
            return customer.HasNoValue ?
                Outcome.Failed<int>("There was an error in Test") :
                Outcome.Successfully<int>(10);
        }

        private Outcome<decimal> Test2(PossibleBe<int> number)
        {
            return number.HasNoValue ?
                Outcome.Failed<decimal>("There was an error in Test 2") :
                Outcome.Successfully<decimal>(10.00m);
        }

        private Outcome<decimal> Test3(PossibleBe<int> number)
        {
            return Outcome.Failed<decimal>("Error in Test 2");
        }
    }

    public class CustomerTest
    {
    }
}