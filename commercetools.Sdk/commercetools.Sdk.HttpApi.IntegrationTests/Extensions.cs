using System;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public static class Extensions
    {
        public static double NextDouble(this Random rnd, double min, double max)
        {
            return rnd.NextDouble() * (max-min) + min;
        }
    }
}