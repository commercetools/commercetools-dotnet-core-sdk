using System.Reflection;

namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    public class UserAgentProvider : IUserAgentProvider
    {
        public UserAgentProvider()
        {
            string assemblyVersion = Assembly
                .GetExecutingAssembly().GetName().Version.ToString();

            AssemblyFileVersionAttribute attr = (AssemblyFileVersionAttribute)typeof(object).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
            this.UserAgent = $"commercetools-dotnet-core-sdk/{assemblyVersion} dotnet/{attr.Version}";
        }

        public string UserAgent { get; set; }
    }
}
