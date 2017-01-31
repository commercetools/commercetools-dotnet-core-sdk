# commercetools-dotnet-sdk

[![Travis Build Status](https://travis-ci.org/commercetools/commercetools-dotnet-sdk.svg?branch=master)](https://travis-ci.org/commercetools/commercetools-dotnet-sdk)
[![AppVeyor Build Status](https://img.shields.io/appveyor/ci/commercetools/commercetools-dotnet-sdk.svg)](https://ci.appveyor.com/project/commercetools/commercetools-dotnet-sdk)
[![NuGet Version and Downloads count](https://buildstats.info/nuget/commercetools.NET.SDK?includePreReleases=true)](https://www.nuget.org/packages/commercetools.NET.SDK)

The commercetools.NET SDK allows developers to work effectively with the commercetools platform in their .NET applications by providing typesafe access to the commercetools HTTP API.

For more documentation please see [the wiki](//github.com/commercetools/commercetools-dotnet-sdk/wiki/commercetools-.NET-SDK-documentation)

## Supported Platforms

* .NET Framework 4.5 and 4.6

## Using the SDK

You will need a commercetools project to use the SDK.
If you don't already have one, you can [create a free trial project](http://dev.commercetools.com/getting-started.html) on the commercetools platform and configure the API credentials.

The namespaces in the SDK mirror the sections of the [commercetools HTTP API](http://dev.commercetools.com/http-api.html).
Access to these namespaces is provided by a fluent interface on the Client class.

Responses from the client are wrapped in a Reponse object so you can determine if the request was successful and view the error(s) returned from the API if it was not.

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
        
        Response<ProductQueryResult> response = await client.Products().QueryProductsAsync();

        if (response.Success)
        {
            ProductQueryResult productQueryResult = response.Result;

            foreach (Product product in productQueryResult.Results)
            {
                Console.WriteLine(product.Id);
            }
        }
        else
        {
            Console.WriteLine("{0}: {1}", response.StatusCode, response.ReasonPhrase);

            foreach (ErrorMessage errorMessage in response.Errors)
            {
                Console.WriteLine("{0}: {1}", errorMessage.Code, errorMessage.Message);
            }
        }
    }
}

```

Not all API sections and representations have been implemented in the SDK. If you need to work with areas of the API that have not yet been covered, you can use the Client to make JSON requests directly. Ask the client for a JObject and you will get the entire JSON response that is returned from the API.  

This code snippet will have the same output as the code snippet above:

```cs

Response<JObject> response = await client.GetAsync<JObject>("/products");

if (response.Success)
{
    dynamic responseObj = response.Result;

    foreach (dynamic product in responseObj.results)
    {
        Console.WriteLine(product.id);
    }
}
else
{
    Console.WriteLine("{0}: {1}", response.StatusCode, response.ReasonPhrase);

    foreach (ErrorMessage errorMessage in response.Errors)
    {
        Console.WriteLine("{0}: {1}", errorMessage.Code, errorMessage.Message);
    }
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

