using FluentAssertions;

using MonoidSharp.Extensions;

using Xunit;

namespace MonoidSharp.Tests.PossibleBeTests
{
    public class CustomerTest
    {
    }

    public class TranslationToPossibleBeTests
    {
        [Fact]
        public void TranslatePossibleBe_NullType_ToOutcome_T_ShouldBeSuccessfully()
        {
            PossibleBe<CustomerTest> possibleBeCustomer = new CustomerTest();
            Outcome<CustomerTest> outcomeCustomer = possibleBeCustomer.TranslateToOutcome("error");

            outcomeCustomer.Success
                .Should()
                .BeTrue();

            outcomeCustomer.Value
                .Should()
                .NotBeNull();

            outcomeCustomer.ErrorMessages
                .Should()
                .HaveCount(0);
        }

        [Fact]
        public void TranslatePossibleBe_T_ToOutcome_T_ShouldBeResultFailure()
        {
            PossibleBe<CustomerTest> possibleBeCustomer = null;
            Outcome<CustomerTest> outcomeCustomer = possibleBeCustomer.TranslateToOutcome("error");

            outcomeCustomer.Failure
                .Should()
                .BeTrue();

            outcomeCustomer.Value
                .Should()
                .BeNull();

            outcomeCustomer.ErrorMessages
                .Should()
                .HaveCount(1);
        }
    }
}