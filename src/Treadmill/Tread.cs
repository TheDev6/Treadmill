namespace Treadmill
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Tread : ITread
    {
        public virtual T Try<T>(Func<T> func, int tryCount, Func<T, Exception, bool> until, int? intervalMilliseconds = null, Action<Exception> onException = null, Action<T, Exception> afterLastRun = null)
        {
            var result = default(T);
            Exception runningException;
            var runCount = 0;

            do
            {
                runningException = null;
                if (runCount > 0)
                {
                    if (intervalMilliseconds.HasValue)
                    {
                        Thread.Sleep(intervalMilliseconds.Value);
                    }
                }

                try
                {
                    runCount++;
                    result = func();
                }
                catch (Exception ex)
                {
                    runningException = ex;
                    onException?.Invoke(ex);
                }

            } while (!until(result, runningException) && runCount < tryCount);

            afterLastRun?.Invoke(result, runningException);

            return result;
        }

        public virtual void Try(Action action, int tryCount, Func<Exception, bool> until, int? intervalMilliseconds = null, Action<Exception> onException = null, Action<Exception> afterLastRun = null)
        {
            Exception runningException;
            var runCount = 0;

            do
            {
                runningException = null;
                if (runCount > 0)
                {
                    if (intervalMilliseconds.HasValue)
                    {
                        Thread.Sleep(intervalMilliseconds.Value);
                    }
                }

                try
                {
                    runCount++;
                    action();
                }
                catch (Exception ex)
                {
                    runningException = ex;
                    onException?.Invoke(ex);
                }

            } while (!until(runningException) && runCount < tryCount);

            afterLastRun?.Invoke(runningException);
        }

        public virtual async Task<T> TryAsync<T>(Func<Task<T>> func, int tryCount, Func<T, Exception, bool> until, int? intervalMilliseconds = null, Action<Exception> onException = null, Action<T, Exception> afterLastRun = null)
        {
            var result = default(T);
            Exception runningException;
            var runCount = 0;

            do
            {
                runningException = null;
                if (runCount > 0)
                {
                    if (intervalMilliseconds.HasValue)
                    {
                        Thread.Sleep(intervalMilliseconds.Value);
                    }
                }

                try
                {
                    runCount++;
                    result = await func().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    runningException = ex;
                    onException?.Invoke(ex);
                }

            } while (!until(result, runningException) && runCount < tryCount);

            afterLastRun?.Invoke(result, runningException);

            return result;
        }

        public virtual async Task TryAsync(Func<Task> func, int tryCount, Func<Exception, bool> until, int? intervalMilliseconds = null, Action<Exception> onException = null, Action<Exception> afterLastRun = null)
        {
            Exception runningException;
            var runCount = 0;

            do
            {
                runningException = null;
                if (runCount > 0)
                {
                    if (intervalMilliseconds.HasValue)
                    {
                        Thread.Sleep(intervalMilliseconds.Value);
                    }
                }

                try
                {
                    runCount++;
                    await func().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    runningException = ex;
                    onException?.Invoke(ex);
                }

            } while (!until(runningException) && runCount < tryCount);

            afterLastRun?.Invoke(runningException);
        }
    }
}
