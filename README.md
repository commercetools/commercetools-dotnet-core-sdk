# commercetools-dotnet-sdk

[![Travis Build Status](https://travis-ci.org/commercetools/commercetools-dotnet-sdk.svg?branch=master)](https://travis-ci.org/commercetools/commercetools-dotnet-sdk)

The commercetools.NET SDK allows developers to work effectively with the commercetools platform in their .NET applications by providing typesafe access to the commercetools HTTP API.

For more documentation please see [the wiki](//github.com/commercetools/commercetools-dotnet-sdk/wiki/commercetools-.NET-SDK-documentation)

## Supported Platforms

* .NET Framework 4.5 and 4.6

## Using the SDK

You will need a commercetools project to use the SDK.
If you don't already have one, you can [create a free trial project](http://dev.commercetools.com/getting-started.html) on the commercetools platform and configure the API credentials.

The namespaces in the SDK mirror the sections of the [commercetools HTTP API](http://dev.commercetools.com/http-api.html).
Access to these namespaces is provided by a fluent interface on the Client class.

```cs

using System;
using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Products;

class Program
{
    static void Main(string[] args)
    {
        new Program().Run().Wait();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private async Task Run()
    {
        Configuration config = new Configuration(
            "https://auth.sphere.io/oauth/token",
            "https://api.sphere.io",
            "[your project key]",
            "[your client ID]",
            "[your client secret]",
            ProjectScope.ManageProject);

        Client client = new Client(config);
        
        ProductQueryResult result = await client.Products().QueryProductsAsync();

        foreach (Product product in result.Results)
        {
            Console.WriteLine(product.Id);
        }
    }
}

```

Not all API sections and representations have been implemented in the SDK. If you need to work with areas of the API that have not yet been covered, you can use the Client to make JSON requests directly. 

```cs

dynamic response = await client.GetAsync("/products");

foreach (dynamic product in response.results)
{
    Console.WriteLine(product.id);
}

```

## License, Contributing

This software is licenses under the MIT License, which allows commercial use and modification as well as open source collaboration.

We are warmly welcoming contributors and are happy to help out.
To contribute changes or improvements, please fork the repository into your account on GitHub and create a pull request.  

## Developing

### Mac Users

The Visual Studio IDE [is available for Mac OS](https://www.visualstudio.com/vs/visual-studio-mac/) (preview version as of 2016)

A more lightweight Coding Environment that also manages the .NET setup automatically for you is [Microsoft Visual Studio Code](https://code.visualstudio.com/) (free). 

