namespace Treadmill.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;


    [TestFixture]
    public class TreadTests
    {
        private static readonly object[] TrySource =
        {
            new object[]
            {
                3,
                4
            },
            new object[]
            {
                1,
                2
            },
            new object[]
            {
                0,
                1
            },
        };

        [Test, TestCaseSource(nameof(TrySource))]
        public void Try_T(int exceptionsUntil, int tryCount)
        {
            var tread = new Tread();

            var runCount = 0;
            var myStr = "My Test String";
            Func<string> myFunc = () =>
            {
                runCount++;
                if (runCount <= exceptionsUntil)
                {
                    throw new Exception("boom");
                }
                return myStr;
            };



            var exceptionList = new List<Exception>();
            string lastStr = null;
            Exception lastException = null;

            var result = tread.Try(
                func: myFunc,
                tryCount: tryCount,
                until: (str, exception) => str != null && exception == null,
                intervalMilliseconds: 25,
                onException: exception => { exceptionList.Add(exception); },
                afterLastRun: (s, exception) =>
                {
                    lastStr = s;
                    lastException = exception;
                }
                );


            result.ShouldBeEquivalentTo(myStr);
            lastStr.ShouldBeEquivalentTo(myStr);
            lastException.ShouldBeEquivalentTo(null);
            exceptionList.Count.ShouldBeEquivalentTo(exceptionsUntil);
            runCount.ShouldBeEquivalentTo(tryCount);
        }

        [Test, TestCaseSource(nameof(TrySource))]
        public void Try(int exceptionsUntil, int tryCount)
        {
            var tread = new Tread();

            var runCount = 0;
            var myStr = "My Test String";
            Action myAction = () =>
            {
                runCount++;
                if (runCount <= exceptionsUntil)
                {
                    throw new Exception("boom");
                }
            };



            var exceptionList = new List<Exception>();
            Exception lastException = null;

            tread.Try(
                action: myAction,
                tryCount: 10,
                until: (exception) => exception == null,
                intervalMilliseconds: 25,
                onException: exception => { exceptionList.Add(exception); },
                afterLastRun: (exception) =>
                {
                    lastException = exception;
                }
                );

            lastException.ShouldBeEquivalentTo(null);
            exceptionList.Count.ShouldBeEquivalentTo(exceptionsUntil);
            runCount.ShouldBeEquivalentTo(tryCount);
        }


        [Test, TestCaseSource(nameof(TrySource))]
        public async Task Try_TaskT(int exceptionsUntil, int tryCount)
        {
            var tread = new Tread();

            var runCount = 0;
            var myStr = "My Test String";
            Func<Task<string>> myFunc = async () =>
            {
                runCount++;
                if (runCount <= exceptionsUntil)
                {
                    throw new Exception("boom");
                }
                return await Task.FromResult(myStr).ConfigureAwait(false);
            };

            var exceptionList = new List<Exception>();
            string lastStr = null;
            Exception lastException = null;

            var result = await tread.TryAsync(
                func: myFunc,
                tryCount: tryCount,
                until: (str, exception) => str != null && exception == null,
                intervalMilliseconds: 25,
                onException: exception => { exceptionList.Add(exception); },
                afterLastRun: (s, exception) =>
                {
                    lastStr = s;
                    lastException = exception;
                }
                ).ConfigureAwait(false);

            result.ShouldBeEquivalentTo(myStr);
            lastStr.ShouldBeEquivalentTo(myStr);
            lastException.ShouldBeEquivalentTo(null);
            exceptionList.Count.ShouldBeEquivalentTo(exceptionsUntil);
            runCount.ShouldBeEquivalentTo(tryCount);
        }

        [Test, TestCaseSource(nameof(TrySource))]
        public async Task Try_Task(int exceptionsUntil, int tryCount)
        {
            var tread = new Tread();

            var runCount = 0;
            Func<Task> myFunc = async () =>
            {
                runCount++;
                if (runCount <= exceptionsUntil)
                {
                    throw new Exception("boom");
                }
                await Task.Run(() => { }).ConfigureAwait(false);
            };

            var exceptionList = new List<Exception>();
            Exception lastException = null;

            await tread.TryAsync(
                func: myFunc,
                tryCount: 10,
                until: (exception) => exception == null,
                intervalMilliseconds: 25,
                onException: exception => { exceptionList.Add(exception); },
                afterLastRun: (exception) =>
                {
                    lastException = exception;
                }
                ).ConfigureAwait(false);

            lastException.ShouldBeEquivalentTo(null);
            exceptionList.Count.ShouldBeEquivalentTo(exceptionsUntil);
            runCount.ShouldBeEquivalentTo(tryCount);
        }
    }
}
