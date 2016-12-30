namespace Treadmill.Tests
{
    using System;
    using NUnit.Framework;

    public class Examples
    {
        [Test]
        public void MainHappyPath()
        {
            var retry = new Tread();

            //This tries until the exception is null, or 10 tries
            retry.Try(
                action: () => { Console.WriteLine("Hello Retry"); },
                tryCount: 10,
                until: ex => ex == null
            );
            //In this case, the expectation is that only one iteration will happen.

            //This tries until the exception is null and the result of the func is not null or whitespace
            var result = retry.Try(
                func: () => DateTime.Now.Millisecond % 2 == 0 ? Guid.NewGuid().ToString("N") : string.Empty,
                until: (strResult, exception) => exception == null && !String.IsNullOrWhiteSpace(strResult),
                tryCount: 5
            );
            //In this case, we expect a random iteration count based on current milliseconds being even.

            //Note: Some care should be taken to avoid swallowing errors with this code. There are hooks to log errors that shouldn't be ignored.
            //I'm debating throwing the error, or aggregate error, when until conditions are not met by the last iteration. Thoughts?
        }
    }
}
