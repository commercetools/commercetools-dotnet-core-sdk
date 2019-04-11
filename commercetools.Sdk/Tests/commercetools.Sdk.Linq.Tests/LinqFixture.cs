using commercetools.Sdk.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Linq.Tests
{
    public class LinqFixture
    {
        private readonly ServiceProvider serviceProvider;

        public LinqFixture()
        {
            var services = new ServiceCollection();
            services.UseRegistration();
            services.UseLinq();
            this.serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }
    }
}
