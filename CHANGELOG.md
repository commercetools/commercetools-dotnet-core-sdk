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
