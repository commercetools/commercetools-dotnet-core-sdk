using System;
using commercetools.Sdk.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace commercetools.Sdk.Framework461Example
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var configuration = new ConfigurationBuilder().
        AddJsonFile("appsettings.test.json").
        AddJsonFile("appsettings.test.Development.json", true).
        // https://www.jerriepelser.com/blog/aspnet-core-no-more-worries-about-checking-in-secrets/
        AddEnvironmentVariables("CTP_").
        Build();
      var services = new ServiceCollection();
      services.UseCommercetools(configuration, "Client");

      var c = services.BuildServiceProvider();

      var client = c.GetService<IClient>();

      var project = client.ExecuteAsync(new GetProjectCommand()).Result;
      Console.Out.WriteLine($"Project key: {project.Key}");
    }
  }
}