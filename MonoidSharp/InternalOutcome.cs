using System;

namespace MonoidSharp
{
    internal class InternalOutcome
    {
        private InternalOutcome(bool isSuccess, string errorMessage)
        {
            if (isSuccess)
            {
                Success = isSuccess;
            }
            else
            {
                if (!isSuccess && string.IsNullOrWhiteSpace(errorMessage))
                    throw new ArgumentException("Failed results MUST have to inform an Error Message");

                ErrorMessage = errorMessage;
            }
        }

        internal string ErrorMessage { get; }

        internal bool Failure => !Success;

        internal bool Success { get; }

        internal static InternalOutcome Create(bool isSuccess, string errorMessage)
        {
            return new InternalOutcome(isSuccess, errorMessage);
        }

        internal static InternalOutcome CreateForFailure(string errorMessage)
        {
            return new InternalOutcome(false, errorMessage);
        }

        internal static InternalOutcome CreateForSuccess()
        {
            return new InternalOutcome(true, null);
        }
    }

    internal class InternalOutcome<T>
    {
        private InternalOutcome _simpleInternalOutcome;

        private InternalOutcome(bool isSuccess, T value, string errorMessage)
        {
            _simpleInternalOutcome = InternalOutcome.Create(isSuccess, errorMessage);
            Value = value;
        }

        internal string ErrorMessage => _simpleInternalOutcome.ErrorMessage;

        internal bool Failure => _simpleInternalOutcome.Failure;
        internal bool Success => _simpleInternalOutcome.Success;
        internal T Value { get; }

        internal static InternalOutcome<T> CreateForFailure(string errorMessage)
        {
            return new InternalOutcome<T>(false, default(T), errorMessage);
        }

        internal static InternalOutcome<T> CreateForSuccess(T value)
        {
            return new InternalOutcome<T>(true, value, null);
        }
    }
}