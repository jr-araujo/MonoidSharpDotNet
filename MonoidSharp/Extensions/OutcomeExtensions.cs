using System;

namespace MonoidSharp.Extensions
{
    public static class OutcomeExtensions
    {
        public static Outcome<TIn> CompensateWhenFail<TIn>(
            this Outcome<TIn> taskInput,
            Func<Outcome<TIn>> func)
        {
            if (taskInput.Failure)
                return func();

            return taskInput;
        }

        public static Outcome<TOut> Bind<TIn, TOut>(
            this Outcome<TIn> input,
            Func<TIn, Outcome<TOut>> func)
        {
            return input.Failure ?
                Outcome.Failed<TOut>(input.ErrorMessages) :
                func(input.Value);
        }

        public static K Return<T, K>(
                    this Outcome<T> outcomer,
                    Func<Outcome<T>, K> func)
        {
            return func(outcomer);
        }

        public static Outcome<TIn> WhenFailDo<TIn>(
            this Outcome<TIn> input,
            Action<Outcome<TIn>> action)
        {
            if (input.Failure)
            {
                action(input);
            }

            return input;
        }

        public static Outcome<TIn> WhenFailDo<TIn>(
            this Outcome<TIn> input,
            Func<Outcome<TIn>> func)
        {
            return CompensateWhenFail(input, func);
        }
    }
}