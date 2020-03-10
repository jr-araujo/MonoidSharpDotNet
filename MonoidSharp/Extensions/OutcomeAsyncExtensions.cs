using System;
using System.Threading.Tasks;

namespace MonoidSharp.Extensions
{
    public static class OutcomeAsyncExtensions
    {
        public static async Task<Outcome<TIn>> CompensateWhenFailAsync<TIn>(
            this Task<Outcome<TIn>> taskInput,
            Func<Task<Outcome<TIn>>> func)
        {
            var taskResult = await taskInput.ConfigureAwait(false);

            if (taskResult.Failure)
                return await func().ConfigureAwait(false);

            return taskResult;
        }

        public static async Task<Outcome<TOut>> ComposeAsync<TIn, TOut>(
            this Task<Outcome<TIn>> taskInput,
            Func<TIn, Outcome<TOut>> func)
        {
            var taskResult = await taskInput.ConfigureAwait(false);
            return taskResult.Bind(func);
        }

        public static async Task<K> OutcomeBetweenThemAsync<T, K>(
            this Task<Outcome<T>> taskInput,
            Func<Outcome<T>, K> func)
        {
            var taskResult = await taskInput.ConfigureAwait(false);
            return taskResult.Return(func);
        }

        public static async Task<Outcome<TIn>> WhenFailDoAsync<TIn>(
            this Task<Outcome<TIn>> taskInput,
            Action<Outcome<TIn>> action)
        {
            var taskResult = await taskInput.ConfigureAwait(false);
            return taskResult.WhenFailDo(action);
        }
    }
}