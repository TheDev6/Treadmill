using System;
using System.Threading.Tasks;

namespace Treadmill
{
    public interface ITread
    {
        void Try(Action action, int tryCount, Func<Exception, bool> until, int? intervalMilliseconds = default(int?), Action<Exception> onException = null, Action<Exception> afterLastRun = null);
        T Try<T>(Func<T> func, int tryCount, Func<T, Exception, bool> until, int? intervalMilliseconds = default(int?), Action<Exception> onException = null, Action<T, Exception> afterLastRun = null);
        Task TryAsync(Func<Task> func, int tryCount, Func<Exception, bool> until, int? intervalMilliseconds = null, Action<Exception> onException = null, Action<Exception> afterLastRun = null);
        Task<T> TryAsync<T>(Func<Task<T>> func, int tryCount, Func<T, Exception, bool> until, int? intervalMilliseconds = default(int?), Action<Exception> onException = null, Action<T, Exception> afterLastRun = null);
    }
}