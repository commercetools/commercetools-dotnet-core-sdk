using commercetools.Sdk.Util;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Linq.Tests
{
    public class LinqFixture
    {
        private readonly ServiceProvider serviceProvider;
       
        public LinqFixture()
        {
            var services = new ServiceCollection();
            services.UseUtil();
            services.UseLinq();
            this.serviceProvider = services.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(this.serviceProvider);
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}