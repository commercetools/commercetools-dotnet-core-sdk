
# commercetools .NET Core SDK

<img src="http://dev.commercetools.com/assets/img/CT-logo.svg" width="550px" alt="CT-logo"></img>

[![Travis Build Status](https://travis-ci.com/commercetools/commercetools-dotnet-core-sdk.svg?branch=master)](https://travis-ci.com/commercetools/commercetools-dotnet-core-sdk)
[![AppVeyor Build Status](https://img.shields.io/appveyor/ci/commercetools/commercetools-dotnet-core-sdk.svg)](https://ci.appveyor.com/project/commercetools/commercetools-dotnet-core-sdk)
[![NuGet Version and Downloads count](https://buildstats.info/nuget/commercetools.Sdk.All?includePreReleases=true)](https://www.nuget.org/packages/commercetools.Sdk.All)

The commercetools .NET Core SDK enables developers to easily communicate with the [commercetools HTTP API](https://docs.commercetools.com/http-api.html). The developers do not need to create plain HTTP requests, but they can instead use the domain specific classes and methods to formulate valid requests. 

## Installation

 - [ ] TODO Add the package name once it has been published. 
 - Download from Nuget
 
## Technical Overview

The SDK consists of the following projects:
* `commercetools.Sdk.Client`: Contains abstract commands which are used to create instances of HTTP API commands. Some commands are implemented as generic types and you must specify a domain object when using them, whereas others are specific to a domain object. 
* `commercetools.Sdk.DependencyInjection`: Default entry point to start using the SDK. Contains one class, `DependencyInjectionSetup`, which initializes all services the SDK uses and injects them into the service collection.
* `commercetools.Sdk.Domain`: Models commercetools domain objects.
* `commercetools.Sdk.HttpApi`: Communicates directly with the HTTP API.
	* `Client`: Has one method, `ExecuteAsync()`, which executes commands. Executes commands which calls the HTTP API. Takes commands from classes in the `commercetools.Sdk.Client` project to construct implementations of the `IHttpApiCommand` interface.
	* `IHttpApiCommand`: Classes implementing this interface are containers for a command from a `commercetools.Sdk.Client` class and a request builder which builds the request to the HTTP API endpoint. 
* `commercetools.Sdk.HttpApi.Domain`: Handles exceptions and errors from the HTTP API.
* `commercetools.Sdk.HttpApi.Linq`: Uses LINQ expressions to build queries for the `where` fields in the HTTP API. 
* `commercetools.Sdk.HttpApi.Registration`: Helper classes for things like creating service locators.
* `commercetools.Sdk.HttpApi.Serialization`: Serialization and deserialization services for responses and requests to the HTTP API.

In addition, the SDK has the following directories:
* `/Examples`: Contains examples of common SDK use cases for MVC applications. 
* `/Tests`: Integration tests for the SDK. A good way for anyone using the .NET SDK to understand it futher.

## Getting Started with the .NET SDK

All operations (get, query, create, update, delete and others) are available as commands that can be passed to the client. In order to use the client object, it needs to be setup first through dependency injection setup.

### Basic Workflow

At a high level, to make a basic call to the API, do the following: 

1. Use the dependency injection class to set things up. 
2. get a client object from the services responsible for calling requests to the API
3. If needed – Create a draft object as a required for the command, as needed.
4. Use executeAsync(Command<T>) (find commands in the Client project – specify the domain obj you're working on)
5. Receive the response as a model. 

### Dependency Injection Setup

 In the ConfigureServices method of Startup.cs add the following:

```c#
services.UseCommercetools(
    this.configuration); // replace with your instance of IConfiguration
```

This code sets "CommercetoolsClient" as the default configuration section name and the Client Credentials as the initial token flow.
If other values should be set, the following method overload can be used:

```c#
services.UseCommercetools(
    this.configuration, // replace with your instance of IConfiguration
    "Client", // replace with your name of the commercetools configuration section
    TokenFlow.AnonymousSession); // replace with your initial token flow
```

More information about token flow can be found in this document.

The previous coding examples set one client only. More information about multiple clients can be found in this document.

### Default HttpClient name

The HttpClient is added by using the built-in AddHttpClient extension. In case the name of the client is not provided, the default client names are used:

 - CommercetoolsClient
 - CommercetoolsAuth

This means that no other HttpClient can have these names.  

### Multiple Clients

It is possible to use more than one client in the same application. The following code can be used to set it up:

```c#
IDictionary<string, TokenFlow> clients = new Dictionary<string, TokenFlow>()
{
    { "client1", TokenFlow.AnonymousSession },
    { "client2", TokenFlow.ClientCredentials }
};
services.UseCommercetools(this.configuration, clients);
```

The appsettings.json then needs to contain the configuration sections named the same. 
The clients can then be injected by using IEnumerable.

    public MyController(IEnumerable<IClient> clients)
     
### Attaching Delegating Handlers

When wanting to attach further Handlers to the HttpClient this is possible by using the service collection.
E.g. adding a [Polly](http://www.thepollyproject.org/) Retry policy using [Microsoft.Extensions.Http.Polly](https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly/):

```c#
var registry = services.AddPolicyRegistry();
var policy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .RetryAsync(3);
registry.Add("retry", policy);

services.AddHttpClient("Client").AddPolicyHandlerFromRegistry("retry");
services.UseCommercetools(configuration, "Client", TokenFlow.ClientCredentials);
```

### Configuration
The client configuration needs to be added to appsettings.json in order for the client to work. The structure is as follows:

```c#
{
    "Client": {
        "ClientId": "", // replace with your client ID
        "ClientSecret": "", // replace with your client secret
        "AuthorizationBaseAddress": "https://auth.sphere.io/", // replace if needed
        "Scope": "", // replace with your scope
        "ProjectKey": "", // replace with your project key
        "ApiBaseAddress": "https://api.sphere.io/"  // replace if needed
    }
}
```

> Note! The property name "Client" can be replaced with something more specified and it can be configured in the dependency injection setup. 

### Using the Client

The IClient interface can be used by injecting it and calling its ExecuteAsync method for different commands.

> Note! Right now only the asynchronous client exists. 

The following code show the constructor injection:

```c#
private readonly IClient client;
public CategoryController(IClient client)
{
    this.client = client;
}
```

The following line of code gets a category by ID:

```c#
string id = "2b327437-702e-4ab2-96fc-a98afa860b36";
Category category = this.client.ExecuteAsync(new GetByIdCommand<Category>(new Guid(id))).Result;
```

> Note! Not all commands are available for all domain types. 

In case the injection of the client is not possible, the client should then be retrieved from the service provider:

```c#
IClient client = serviceProvider.GetService<IClient>();
```

> Note! Some working examples can be found in the Examples folder of the solution or in the integration tests project. 

## Token Flow

There are three different token flows available as enum:

 - TokenFlow.ClientCredentials
 - TokenFlow.Password
 - TokenFlow.AnonymousSession

The initial flow is set at the start of the application but it can be changed while the application is running (for example, from anonymous to password). The flow is changed per client (in case there are multiple clients). 

### Changing the Flow

The token flow can be changed by using the ITokenFlowRegister. This interface is used for the mapping of the clients to their token flows. It can be injected in the same way as the IClient interface. The following code changes the token flow in applications that use a single client only: 

```c#
private readonly ITokenFlowRegister tokenFlowRegister;
private readonly IClient client;
...
this.tokenFlowRegister.TokenFlow = TokenFlow.Password;
```

In case there are more clients, the following code sets the TokenFlow for a specific client, by using the ITokenFlowRegister through ITokenFlowMapper. 

```c#
this.tokenFlowMapper.GetTokenFlowRegisterForClient(this.client.Name).TokenFlow = TokenFlow.Password;
```

### Storing the Token

By default all token related data is saved in memory (and also retrieved from it). It is possible to change this. More information about this can be found in this document about overriding the defaults. 

## Overriding the Defaults
There are a few interfaces that can have custom implementations:

 - ITokenStoreManager
 - IUserCredentialsStoreManager
 - IAnonymousCredentialsStoreManager

### Token

If you want to store token elsewhere other than in memory (for example, to persist it in case it has a refresh token), you should create a custom class that implements ITokenStoreManager. You can override the default implementation by adding the new one in the dependency injection setup:

```c#
services.UseCommercetools(this.configuration, "Client", TokenFlow.AnonymousSession);
services.AddSingleton<ITokenStoreManager, CustomTokenStoreManager>();
```

### User credentials

The interface IUserCredentialsStoreManager should normally be implemented in a custom class. That is because the username and password are sometimes entered by users of applications and not stored in configuration and therefore it is not known by the SDK where they come from. For example, they can some from Request (in case they are entered in a form) or Session objects. You should override the default implementation by adding the new one in the dependency injection setup:

```c#
services.UseCommercetools(this.configuration, "Client", TokenFlow.Password);
services.AddHttpContextAccessor(); // custom class requires IHttpContextAccessor
services.AddSingleton<IUserCredentialsStoreManager, CustomUserCredentialsStoreManager>();
```

> Note! This interfaces only requires the getter, since the SDK does not need to store the username and password anywhere.  

### Anonymous Session ID

The interface IAnonymousCredentialsStoreManager should normally be implemented in a custom class. You can override the default implementation by adding the new one in the dependency injection setup:

```c#
services.UseCommercetools(this.configuration, "Client", TokenFlow.AnonymousSession);
services.AddSingleton<IAnonymousCredentialsStoreManager, AnonymousCredentialsStoreManager>();
```

## Predicates

There are numerous commands and objects that accept predicates. In order to facilitate the developers, allow auto-completion and reduces the amount of errors, these predicates can be defined as lambda expressions. In case a predicate definition is not supported (that is, a NotSupportedException is thrown), a manually constructed string can be passed as well. The following example shows how a query predicate can be created using lambda expressions.

```c#
string key = category.Key;
QueryPredicate<Category> queryPredicate = new QueryPredicate<Category>(c => c.Key == key);
QueryCommand<Category> queryCommand = new QueryCommand<Category>();
queryCommand.SetWhere(queryPredicate);
```

In order to use values from objects directly use the valueOf extension method:

```c#
c => c.Key == category.Key.valueOf()
```

Setting the "where" query string parameter as a string can be done in the following way:

```c#
queryCommand.Where = "key = \"c14\"";
```
    
> Note! For more examples of predicates using lambda expressions, take a look at the unit tests.

There are numerous extension methods created for domain specific operations. They can also be found by importing the commercetools.Sdk.Domain.Predicates namespace. The extension WithinCircle is one of them, for example: 

```c#
c => c.GeoLocation.WithinCircle(13.37774, 52.51627, 1000)
```

## LINQ

Experimental support for querying the API using LINQ is provided by the `commercetools.Sdk.Client.Extensions`

```c#
var query = from c in client.Query<Category>()
                where c.Key == "c14"
                select c;

foreach(Category c in query)
{
    var c = c.Key;
}
```
    
Accessing the command built using LINQ

```c#
var command = ((ClientQueryProvider<Category>) query.Provider).Command;
```
