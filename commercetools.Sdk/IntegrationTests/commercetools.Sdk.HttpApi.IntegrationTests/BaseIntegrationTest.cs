using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using commercetools.Sdk.HttpApi.Domain.Exceptions;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class BaseIntegrationTest
    {
        public void AssertEventually(Action runnableBlock, int maxWaitTimeSecond = 180,
            int waitBeforeRetryMilliseconds = 100)
        {
            var maxWaitTime = TimeSpan.FromSeconds(maxWaitTimeSecond);
            var waitBeforeRetry = TimeSpan.FromMilliseconds(waitBeforeRetryMilliseconds);
            TestingUtility.AssertEventually(maxWaitTime, waitBeforeRetry, runnableBlock);
        }
    }
}
