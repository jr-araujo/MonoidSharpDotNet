using System.Collections.Generic;
using System.Linq;

namespace MonoidSharp
{
    public class Outcome
    {
        internal IEnumerable<string> _errorMessages;
        private static readonly Outcome SuccessfullyResult = new Outcome(true, null);
        private readonly InternalOutcome _internalResult;

        private Outcome(bool isSuccess, string errorMessage)
        {
            _internalResult = isSuccess ?
                                InternalOutcome.CreateForSuccess() :
                                InternalOutcome.CreateForFailure(errorMessage);
        }

        public IEnumerable<string> ErrorMessages => _errorMessages;

        public bool Failure => _internalResult.Failure;

        public bool Success => _internalResult.Success;

        public static Outcome Combine(params Outcome[] results)
        {
            var resultsWithFailure = results.Where(x => x.Failure).ToArray();

            if (!resultsWithFailure.Any())
                return Successfully();

            var errors = resultsWithFailure.Select(x => x._internalResult.ErrorMessage);

            return FailedWithErrorList(errors);
        }

        public static Outcome Failed(string errorMessage)
        {
            var failedOutcome = new Outcome(false, errorMessage);
            failedOutcome._errorMessages = new List<string> { failedOutcome._internalResult.ErrorMessage };

            return failedOutcome;
        }

        public static Outcome<T> Failed<T>(string errorMessage)
        {
            return Outcome<T>.Create(false,
                                     default(T),
                                     new List<string>(1) { errorMessage });
        }

        public static Outcome<T> Failed<T>(IEnumerable<string> errorMessages)
        {
            return Outcome<T>.Create(false, default(T), errorMessages);
        }

        public static Outcome Successfully()
        {
            return SuccessfullyResult;
        }

        public static Outcome<T> Successfully<T>(T value)
        {
            return Outcome<T>.Create(true, value, new List<string>(1) { "" });
        }

        private static Outcome FailedWithErrorList(IEnumerable<string> errorMessages)
        {
            var failedOutcome = new Outcome(false, "ERROR MESSAGE");
            failedOutcome._errorMessages = errorMessages;

            return failedOutcome;
        }
    }

    public class Outcome<T>
    {
        private readonly InternalOutcome<T> _internalOutcome;

        private Outcome(bool isSuccess, T value, IEnumerable<string> errorMessages)
        {
            _internalOutcome = isSuccess ?
                                InternalOutcome<T>.CreateForSuccess(value) :
                                InternalOutcome<T>.CreateForFailure(errorMessages.First());

            ErrorMessages = _internalOutcome.Failure ?
                new List<string> { _internalOutcome.ErrorMessage } :
                Enumerable.Empty<string>();

            Value = _internalOutcome.Value;
        }

        public IEnumerable<string> ErrorMessages { get; }

        public bool Failure => _internalOutcome.Failure;

        public bool Success => _internalOutcome.Success;

        public T Value { get; }

        internal static Outcome<T> Create(
            bool isSuccess,
            T value,
            IEnumerable<string> errorMessages)
        {
            return new Outcome<T>(isSuccess, value, errorMessages);
        }

        public static implicit operator Outcome(Outcome<T> outcome)
        {
            if (outcome.Success)
                return Outcome.Successfully();
            else
                return Outcome.Failed(outcome.ErrorMessages.First());
        }
    }
}