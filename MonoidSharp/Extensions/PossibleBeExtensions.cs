namespace MonoidSharp.Extensions
{
    public static class PossibleBeExtensions
    {
        public static Outcome<T> TranslateToOutcome<T>(
            this PossibleBe<T> possibleBe,
            string errorMessageOnTranslation)
        {
            return possibleBe.HasNoValue ?
                Outcome.Failed<T>(errorMessageOnTranslation) :
                Outcome.Successfully(possibleBe.Value);
        }
    }
}