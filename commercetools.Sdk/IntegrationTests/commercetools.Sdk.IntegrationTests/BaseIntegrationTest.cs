using System;

namespace commercetools.Sdk.IntegrationTests
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
