# commercetools .NET Core SDK

<img src="http://dev.commercetools.com/assets/img/CT-logo.svg" width="550px" alt="CT-logo"></img>
[![Travis Build Status](https://travis-ci.org/commercetools/commercetools-dotnet-sdk.svg?branch=master)](https://travis-ci.org/commercetools/commercetools-dotnet-sdk)

The commercetools .NET Core SDK enables developers to easily communicate with the [commercetools HTTP API](https://docs.commercetools.com/http-api.html). The developers do not need to create plain HTTP requests, but they can instead use the domain specific classes and methods to formulate valid requests. 

## Installation

 - [ ] TODO Add the package name once it has been published. 
 - Download from Nuget

## Getting Started

All operations (get, query, create, update, delete and others) are available as commands that can be passed to the client. In order to use the client object, it needs to be setup first through dependency injection setup.

### Dependency Injection Setup
 In the ConfigureServices method of Startup.cs add the following:
 
    services.UseCommercetools(
	    this.configuration, // replace with your instance of IConfiguration
	    "Client"); // replace with your name of the commercetools configuration section

This code sets the Client Credentials as the initial token flow.
If another token flow should be set, the following method overload can be used:

    services.UseCommercetools(
	    this.configuration, // replace with your instance of IConfiguration
	    "Client", // replace with your name of the commercetools configuration section
	    TokenFlow.AnonymousSession); // replace with your initial token flow

More information about token flow can be found in this document.

The previous coding examples set one client only. More information about multiple clients can be found in this document.

### Using the Client

The IClient interface can be used by injecting it and calling its ExecuteAsync method for different commands.
The following code show the constructor injection:

     private readonly IClient client;
     public CategoryController(IClient client)
     {
	     this.client = client;
     }

The following line of code gets a category by ID:

    string id = "2b327437-702e-4ab2-96fc-a98afa860b36";
    Category category = this.client.ExecuteAsync(new GetByIdCommand<Category>(id)).Result;

> Note! Not all commands are available for all domain types. 

In case the injection of the client is not possible, the client should then be retrieved from the service provider:

    IClient client = serviceProvider.GetService<IClient>();

## Token Flow

There are three different token flows available as enum:

 - TokenFlow.ClientCredentials
 - TokenFlow.Password
 - TokenFlow.AnonymousSession

The initial flow is set at the start of the application but it can be changed while the application is running (for example, from anonymous to password). The flow is changed per client (in case there are multiple clients). 

### Changing the Flow

The token flow can be changed by using the ITokenFlowMapper. This interface is used for the mapping of the clients to their token flows. It can be injected in the same way as the IClient interface. The following code changes the token flow in applications that use a single client only: 

    private readonly ITokenFlowMapper tokenFlowMapper;
    private readonly IClient client;
	...
	this.tokenFlowMapper.TokenFlowRegister.TokenFlow = TokenFlow.Password;

In case there are more clients, the following code sets the TokenFlow for a specific client.

    this.tokenFlowMapper.GetTokenFlowRegisterForClient(this.client.Name).TokenFlow = TokenFlow.Password;

## Multiple Clients
## Overriding the Defaults
## Integration Tests

