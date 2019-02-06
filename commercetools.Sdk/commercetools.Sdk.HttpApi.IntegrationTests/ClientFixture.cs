using commercetools.Sdk.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using commercetools.Sdk.Client;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq;
using commercetools.Sdk.DependencyInjection;
using commercetools.Sdk.HttpApi.Tokens;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class ClientFixture
    {
        private readonly ServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public ClientFixture()
        {
            var services = new ServiceCollection();
            this.configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.test.json").
                AddJsonFile("appsettings.test.Development.json", true).
                // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
                AddEnvironmentVariables().
                Build();
            
            //Inject our custom delegating handler
            //services.AddSingleton<ITestOutputHelper,TestOutputHelper>();
            services.AddSingleton<ICustomDelegatingHandler, CustomDelegatingHandler>();
            
            services.UseCommercetools(configuration, "Client", TokenFlow.ClientCredentials);
            
            this.serviceProvider = services.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        public IClientConfiguration GetClientConfiguration(string name)
        {
            return this.configuration.GetSection(name).Get<ClientConfiguration>();
        }

        public static Random random = new Random();

        // TODO Put these random in a separate class (Create util for Testing)
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int RandomInt(int? min = null, int? max = null)
        {
            int ran;
            if (min.HasValue && max.HasValue)
            {
                ran = random.Next(min.Value, max.Value);
            }
            else
            {
                ran = Math.Abs(random.Next());
            }
            return ran;
        }
        public string RandomDoubleAsString(double min, double max)
        {
            var ran = random.NextDouble(0.1, 0.9);
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00}", ran);
        }

        public string RandomSortOrder()
        {
            int append = 5;//hack to not have a trailing 0 which is not accepted in sphere
            return "0." + RandomInt() + append;
        }
        
        
    }
}