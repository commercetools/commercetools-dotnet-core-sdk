namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ClientFixture
    {
        public ServiceProviderFixture ServiceProviderFixture { get; }

        public ClientFixture(ServiceProviderFixture serviceProviderFixture)
        {
            this.ServiceProviderFixture = serviceProviderFixture;
        }

        public T GetService<T>()
        {
            return this.ServiceProviderFixture.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.ServiceProviderFixture.GetClientConfiguration(name);
        }
    }
}
