## [1.0.0-rc-1 - 2020-04-03](https://github.com/commercetools/commercetools-dotnet-core-sdk/compare/1.0.0-beta-3...1.0.0-rc-1)
### Features
- Add KeyReference and Implement missing platform release updates [`#119`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/119)
- Add Command Builders Feature [`#116`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/116)
- Add Domain Models, Commands, Request Builders and Integration Tests for OrderEdits Endpoint [`#97`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/97)
- Add InStoreCommand as a decorator for other commands and applying it for Customers, Carts, Orders and OrdersImport Endpoints [`#97`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/97)
- Add MvcExample Project to List Products as Example of how to use the SDK in MVC Application [`d1ea7a8`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/d1ea7a810575e375be638fee8f3d1f0bd48e6e54)
- Add MatchingShippingMethodConverter to Deserialize `List<ShippingMethod>` as `PageQueryResult<ShippingMethod>` [`1137763`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/113776348dc44aaddb8b918de7a498e487d2dd6d)
- Create `TokenSerializerService` with SnakeCaseNamingStrategy and update token providers to use this new service [`d6b8286`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/d6b828610ddebb083d7e93b09acba299330f0c6b)
- Add a JsonConverter to Support Deserialization of IReference Types [`68e008e`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/68e008ed6eb3addb2608b4541cb14aceed254232)
- Use Docker Build for Travis Linux [`9a1cc06`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/9a1cc06b7c8f2cf9cf49e52399aa5cd684e1d3a8)
- Use ConcurrentDictionary Instead of Dictionary to be Thread Safe For Multiple Threads Access [`18926b1`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/18926b19ad3a01a36233fcde12cb872701167f38)
- Add SetAdditionalParameters Extension Method for Commands [`243b47d`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/243b47dc116c17d3e2d661a000f91f3fc284c99e)
- Add a new constructor for GetCartByCustomerIdCommand with customerId as string [`3065f1b`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/3065f1bc40ee9af0b6ed7bf76cca7c85fa5b1852)
- Add SetKeyUpdateAction Model for ProductDiscount Endpoint [`44dd4e3`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/44dd4e30bebac9b639f50bd5a7ae1dfd473d99ff)
- Update Packages Versions and Validate services Scope on build [`#87`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/87)
- Add a new constructor in `ChangePasswordCommand` with IVersioned parameter [`e52a421`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/e52a421f7e22d18667901ca5ba08932d1339e419)
- Add dockerfile to build container with SDK for Core 3.0 & 2.1 [`a17ca36`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/a17ca36308a09b1ec29d8cbd5393d3814d864ede)
- Add these new commands : [`6bbac63`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/6bbac63ebbb7a2e98ccd4664f8c132308427ad67)
   * GetShippingMethodsForCartCommand
   * GetShippingMethodsForLocationCommand
   * GetShippingMethodsForOrderEditCommand

### Bug Fixes
- Fix Attributes casting [`#115`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/115)
- Solving an issue while creating httpApiCommands [`#115`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/115)
- Add `Type` Property to State Model [`173ad9e`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/173ad9e4d924d08f14399a1a8cd3a76ed9ba6272)
- Add `Key` Property to ProductDiscountDraft and ProductDiscount Models [`b7ebde2`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/b7ebde2cfb401ecc680f54ba33587335db37d7a2)
- Adjust GetCartByCustomerIdCommand to adapt changes in the changed endpoint. [`62e596d`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/62e596ddf0ec3428bda94ed6f140791381f052eb)
- Remove IHttpClientFactory registration from SimpleInjector DependencySetup [`c279fbb`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/c279fbbfd2315c934a30c08f5364cf41e832ac6c)
- Register IHttpFactory only if not yet registered [`2c1c0f3`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/2c1c0f3ee2bb91ef8904617faadb0b784a075602)
- Remove ApiExceptionFactory from DependencyInjection [`#86`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/86)
- Add fixes in SimpleInjector project about IHttpClientFactory issues [`#81`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/81)
- Fix deserialization of InventoryEntry references [`bb0a070`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/bb0a070ae5d4d84d9c560d3e1e569aaed3fb30df)
- Use Default Serialization settings when failing to get settings based on Type [`93105a4`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/93105a4047671c5c80937c8bec406a285f0f2b19)
- Add CustomFieldsPredicateVisitorConverter to solve case sensitive issue on custom fields predicate #89 [`393710e`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/393710e7b26b9824cbc06162000d35d1457b2940)
- Fix the type of some properties in customer models [`b7c0248`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/b7c024877dc98173bb2a27db64b606c9baf3459b)
- Solve type of property `Assets` to be `List<AssetDraft>` instead of `List<Asset>` in CategoryDraft Model [`f89a0ba`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/f89a0baf2f73de2ebedb709a0240981d3ffa11f2)
- Fix typo in PriceTier [`b8b4227`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/b8b42273424e610fcc31a3c30f6955740426a4d6)

### Breaking Changes
- Change Type of `SupplyChannel` Property in SetSupplyChannelUpdateAction Model from `Reference<Channel>` to `IReference<Channel>` [`6a6ad8c`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/6a6ad8c6fa04ebd1d600a8826a19d1f504be3a48)
- Change Type of `Category` Property in AddToCategoryUpdateAction from `ResourceIdentifier` to `IReference<Category>` [`548a888`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/548a888c94f0eed5661bb3cac9776b6059f39c59)
- Rename Order Update Action `SetShipmentReturnStateUpdateAction` to be `SetReturnShipmentStateUpdateAction` [`bfbcc99`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/bfbcc99fa0bc7b853ecf22a43755b26b45b286ee)
- Change Type of `State` Property in TransitionStateUpdateAction from `Reference<State>` to `IReference<State>` [`a9920c1`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/a9920c130b720fe619b9193e1df0e0abab9a2a0b)
- Change Properties with Type `Reference<>` to be of Type `IReference<>` in these models: [`4c92e4b`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/4c92e4b1d4424ecb121b561430b30617f33d6797)
    * AddPaymentUpdateAction
    * RemovePaymentUpdateAction
    * TransitionStateUpdateAction
    * SetCustomerUpdateAction
- Make "Id" Property Nullable and adding constructors to RemoveProductVariantUpdateAction [`8de429c`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/8de429c4aac533457f1926a969dc8da38a6bac56)
- Change Properties with Type `Reference<>` to be of Type `IReference<>` in these models: [`fde8546`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/fde85468c94f06e22f265f8fdb37cd9787de6450)
    * RemoveDiscountCodeUpdateAction
    * SetCustomShippingMethodUpdateAction
    * SetShippingMethodUpdateAction
- Rename `HttpApiErrorResponse` class to `ErrorResponse` [`8d85626`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/8d85626968a2e419a332042e29f45e4491846376)
- Move these classes from `commercetools.Sdk.HttpApi.Domain` project to `commercetools.Sdk.Domain` project:[`8d85626`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/8d85626968a2e419a332042e29f45e4491846376)
    * ErrorResponse
    * Error
    * GeneralError
    * ConcurrentModificationError
- Change Properties of type `ResourceIdentifier<Store>` to `IReferenceable<Store>` [`8f5d858`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/8f5d858e0726e2211a47d03e379f7a557a2b5f82)
- Rename namespace of ApiClient from `commercetools.Sdk.Domain.ApiClient` to `commercetools.Sdk.Domain.ApiClients` [`c25bd36`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/c25bd36bc1c287ee5535bfe5714ed807f2df0b0b)
- Rename folder of Registration Project from `commercetools.Sdk.Reflection` to `commercetools.Sdk.Registration` [`71babdd`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/71babdd4be595fc3e3f9cd65f386c92ddce4231d)
- Move Project Models to `commercetools.Sdk.Domain.Projects` Namespace [`c24b2b7`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/c24b2b78734b2cea22b8a6d16312fec757c4539d)
- Change Type of `TransactionId` Property to be string instead of Guid in these models: [`b8045f6`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/b8045f6397d01feb196fa0bd18007dbe3c89743d)
    * ChangeTransactionInteractionIdUpdateAction
    * ChangeTransactionStateUpdateAction
    * ChangeTransactionTimestampUpdateAction

- Rename namespace of types models from `commercetools.Sdk.Domain` to `commercetools.Sdk.Domain.Types` [`b815ccc`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/b815ccc473d878e7ec6e23e15f409b51b3c9e9b0)

## [1.0.0-beta-3 - 2019-11-06](https://github.com/commercetools/commercetools-dotnet-core-sdk/compare/1.0.0-beta-2...1.0.0-beta-3)
### Features
- Add dockerfile to build container with SDK for Core 3.0 & 2.1

### Bug Fixes
- Fix tokenProvider tests
- Fix IntegrationTest to use SimpleInjector container




## [1.0.0-beta-2 - 2019-10-22](https://github.com/commercetools/commercetools-dotnet-core-sdk/compare/1.0.0-beta-1...1.0.0-beta-2)
### Features
- Improve exception message for attribute deserialization error [`#66`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/66)
- Adding models and unit tests for Subscriptions endpoint [`#63`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/63)
- Multiple skip take [`#62`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/62)
- Allow multiple Take and Skip for item providing actions; [`#59`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/59)
- Adding more LINQ predicates unit tests [`#52`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/52)
- Adding LINQ documentation [`#52`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/52)
- Adding models and tests for messages endpoint [`#51`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/51)
- Adding models and tests for API extensions endpoint [`#50`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/50)
- Adding models and tests for shopping list endpoint [`#47`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/47)
- Adding models and tests for shipping methods endpoint [`#46`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/46)
- Adding channels update action Models [`e5ea851`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/e5ea851a48ef62c52e3f418241c46f15ff04c35b)
- Adding store models & update cart and order models to include store reference [`c50679b`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/c50679b6b2b232e9a94b843721b9d88830c0c51b)
- Adding SuggestQueryCommand, ProductSuggestion, SuggestionResult and SuggestQueryRequestMessageBuilder [`54eb92d`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/54eb92d21e63f37da25fabd8e9c6a4bcba526556)
- Adding IKeyReferencable interface to resources can be use as Resource Identifier By Key [`177e210`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/177e210d30ce1290d1c8e68c257e552e1cf23acf)
- Adding delete & get helper extensions [`0e3ff57`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/0e3ff574b178733bfed688ab437112e3b48f2836)
- Adding update helper extensions [`e1c0979`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/e1c097918cc8854febe9dd93919aadf9fccab12f)
- Improve resource identifier usage [`ac7a065`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/ac7a065bff384080edc802c603e43b42f99987b2)
- Update installation in readme [`18e75cb`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/18e75cb1a8ee3f3698a651337d3726283f8bbf51)
- Handle implicit bool in query predicate expressions [`#74`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/74)

### Bug Fixes
- Solve empty set attribute value array issue [`#73`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/73)
- Adding comparison methods to solve compare strings issue [`#68`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/68)
- Fix camelcase issue in custom fields [`#49`](https://github.com/commercetools/commercetools-dotnet-core-sdk/pull/49)
- Solving CustomFields Issue In Draft Models [`531aee8`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/531aee8d38e68311899977d54818c7ce56cbeec4)
- Solving Some LINQ Predicates issues [`4ed9782`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/4ed978208dadedf828f165239bc1703e8514ecae)
- Fixing property names in some models like:
    - property `FieldName` in Type/UpdateActions/ChangeEnumValueOrderUpdateAction
    - property `FieldName` in Type/UpdateActions/ChangeLocalizedEnumValueOrderUpdateAction
- Adding missing properties in some models like: property `Key` in CartDiscount and CartDiscountDraft Models

### Breaking Changes

##### Zones:
- Changing type of property `Name` in UpdateActions/ChangeNameUpdateAction from `LocalizedString` to `string`

##### ProductTypes:
- Changing namespace of all Update Actions of ProductTypes from commercetools.Sdk.Domain.ProductTypes to commercetools.Sdk.Domain.ProductTypes.UpdateActions
- Changing type of property `Name` in ProductTypes/UpdateActions/ChangeNameUpdateAction from `LocalizedString` to `string`
- Changing type of property `Description` in ProductTypes/UpdateActions/ChangeDescriptionUpdateAction from `LocalizedString` to `string`

##### TaxCategories:
- Changing namespace of all models of TaxCategories from commercetools.Sdk.Domain to commercetools.Sdk.Domain.TaxCategories
- Changing namespace of all update Actions of TaxCategories from commercetools.Sdk.Domain.TaxRates to commercetools.Sdk.Domain.TaxCategories.UpdateActions

##### CartDiscounts:
- Changing namespace of all models of CartDiscounts from commercetools.Sdk.Domain to commercetools.Sdk.Domain.CartDiscounts
- Changing namespace of all update Actions of CartDiscounts from commercetools.Sdk.Domain.CartDiscounts to commercetools.Sdk.Domain.CartDiscounts.UpdateActions

##### DiscountCodes:
- Changing namespace of all models of DiscountCodes from commercetools.Sdk.Domain to commercetools.Sdk.Domain.DiscountCodes
- Changing namespace of all update Actions of DiscountCodes from commercetools.Sdk.Domain.DiscountCodes to commercetools.Sdk.Domain.DiscountCodes.UpdateActions

##### ShippingMethods
- Changing Type of property `ShippingRates` to `List<ShippingRateDraft>` instead of `List<ShippingRate>` in ZoneRateDraft
- Changing type of property `Name` in UpdateActions/ChangeNameUpdateAction from `LocalizedString` to `string`

##### CustomFields
- Replace properties of type `CustomFields` with type `CustomFieldsDraft` in these Draft Models
- ShoppingLists/ShoppingListDraft
- ShoppingLists/TextLineItemDraft
- Carts/CartDraft
- Carts/CustomLineItemDraft
- Carts/LineItemDraft
- Carts/UpdateActions/AddLineItemUpdateAction

##### Property type fixes
- Solving type of properties in some domain models,[`3beb7eda`](https://github.com/commercetools/commercetools-dotnet-core-sdk/commit/3beb7edaa8cfc938d48b12efcbc1f62bd56f1ddd)
##### Carts
- Changing namespace of Delivery model from commercetools.Sdk.Domain to commercetools.Sdk.Domain.Carts
##### ProductDiscounts
- Convert property `ProductId` in GetMatchingProductDiscountParameters model to be `string` instead of `Guid`


## 1.0.0-beta-1 - 2019-07-11
