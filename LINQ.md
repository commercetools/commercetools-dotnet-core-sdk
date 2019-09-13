## Query Predicates Examples

```c#
var categoryQueryCommand = new QueryCommand<Category>();
var customerQueryCommand = new QueryCommand<Customer>();
var productQueryCommand = new QueryCommand<Product>();
var channelQueryCommand = new QueryCommand<Channel>();
var productProjectionQueryCommand = new QueryCommand<ProductProjection>();
```

//Compare a field's value to a given value
```c#
categoryQueryCommand.Where(c => c.Key == "c14"); // query catgeories with Key "c14"
categoryQueryCommand.Where(c => c.Key != "c14"); // query catgeories with Key not equal "c14"
categoryQueryCommand.Where(c => c.Version > 30); // query categories with version greater than 30
categoryQueryCommand.Where(c => c.Version <= 30); // query categories with version 30 or less
```

//if value from variable use valueOf like below
```c#
var categoryKey = "c14"
var category = new Category { Key = "c14" }
categoryQueryCommand.Where(c => c.Key == categoryKey.valueOf());
categoryQueryCommand.Where(c => c.Key == category.Key.valueOf());
```

// Combine any two conditional expressions in a logical conjunction / disjunction like below
```c#
categoryQueryCommand.Where(c => c.Key == "c14" && c.Version == 30); // query categories with key "c14" and with Version 30
categoryQueryCommand.Where(c => c.Key == "c14" || c.Version == 30); // query categories with key "c14" or with Version 30
```

// Negate any other conditional expression
```c#
categoryQueryCommand.Where(c => !(c.Key == "c14" && c.Version == 30));
```

// Check whether a field's value is or is not contained in
// a specified set of values.
```c#
categoryQueryCommand.Where(c => c.Key.In("c14", "c15")); // key in ("c14", "c15")
categoryQueryCommand.Where(c => c.Key.NotIn("c14", "c15")); // key not in ("c14", "c15")
```

// Check whether an array contains all or any of a set of values
```c#
customerQueryCommand.Where(c => c.ShippingAddressIds.ContainsAll("c14", "c15"));
customerQueryCommand.Where(c => c.ShippingAddressIds.ContainsAny("c14", "c15"));
```

// Check whether an array is empty
```c#
customerQueryCommand.Where(c => c.ShippingAddressIds.IsEmpty());
```

// Check whether a field exists & has a non-null value
```c#
categoryQueryCommand.Where(c => c.Key.IsDefined());
categoryQueryCommand.Where(c => c.Key.IsNotDefined());
```

// Descend into nested objects
```c#
productQueryCommand.Where(p => p.MasterData.Current.Slug["en"] == "product");//masterData(current(slug(en = "product")))
```

// Descend into nested arrays of objects
```c#
productQueryCommand.Where(p => p.Attributes.Any(a => a.ToTextAttribute().Name == "text-name" && a.ToTextAttribute().Value == "text-value")); // attributes(name = "text-name" and value = "text-value")
```

// Query GeoJSON field within a circle
```c#
channelQueryCommand.Where(c => c.GeoLocation.WithinCircle(13.37774, 52.51627, 1000));
```

// for missing attribute
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => !variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name")));
```

// for single attribute value of TextType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name" && a.ToTextAttribute().Value == "attribute-value")));
```
// for multiple attribute values of TextType with same name
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name" && a.ToTextAttribute().Value.In("attribute-value-1", "attribute-value-2"))));
```

// for single attribute value of LTextType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "attribute-name" && a.ToLocalizedTextAttribute().Value["en"] == "attribute-value")));
```

// for multiple attribute values of LTextType with same name
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "attribute-name" && (a.ToLocalizedTextAttribute().Value["en"] == "english-value" || a.ToLocalizedTextAttribute().Value["de"] == "german-value"))));
```

// for EnumType or LocalizableEnumType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToEnumAttribute().Name == "attribute-name" && a.ToEnumAttribute().Value.Key == "enum-key")));
```

// for MoneyType (currencyCode is required)
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToMoneyAttribute().Name == "attribute-name" && a.ToMoneyAttribute().Value.CentAmount == 999 && a.ToMoneyAttribute().Value.CurrencyCode == "EUR")));
```
// for MoneyType with centAmount within a specific range (currencyCode is required)
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToMoneyAttribute().Name == "attribute-name" && a.ToMoneyAttribute().Value.CentAmount > 999 && a.ToMoneyAttribute().Value.CentAmount < 1001 && a.ToMoneyAttribute().Value.CurrencyCode == "EUR")));
```

// for NumberType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToNumberAttribute().Name == "attribute-name" && a.ToNumberAttribute().Value = 999)));
```
// for NumberType with value within a specific range
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToNumberAttribute().Name == "attribute-name" && a.ToNumberAttribute().Value > 999 && a.ToNumberAttribute().Value < 1001)));
```

// for DateType, TimeType or DateTimeType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "attribute-name" && a.ToDateTimeAttribute().Value == DateTime.Parse("2016-06-04T12:27:55.344Z"))));
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "attribute-name" && a.ToDateTimeAttribute().Value == DateTime.Parse("2016-06-04"))));
```

// for DateType, TimeType or DateTimeType with a value within a specific range
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "attribute-name" && a.ToDateTimeAttribute().Value > DateTime.Parse("2016-06-04T12:27:55.344Z") && && a.ToDateTimeAttribute().Value < DateTime.Parse("2016-06-06T12:27:55.344Z"))));
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "attribute-name" && a.ToDateTimeAttribute().Value > DateTime.Parse("2016-06-04") && && a.ToDateTimeAttribute().Value < DateTime.Parse("2016-06-06"))));
```

// for ReferenceType
```c#
productProjectionQueryCommand.Where(p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToReferenceAttribute().Name == "attribute-name" && a.ToReferenceAttribute().Value.TypeId == ReferenceTypeId.Category && a.ToReferenceAttribute().Value.Id == "963cbb75-c604-4ad2-841c-890b792224ee" )));
```

## Discount Predicates Examples

### Product Discounts Examples:
```c#
var productDiscountDraft = new ProductDiscountDraft();

// match a specific variant in the specific product
productDiscountDraft.SetPredicate(p => p.ProductId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && product.VariantId() == 1);

// match a product that is only in the given category
productDiscountDraft.SetPredicate(p => p.CategoryId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");

//match a product that is in the given category
productDiscountDraft.SetPredicate(p => p.CategoriesId().Contains("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

//match a product that is in all of the the given categories
productDiscountDraft.SetPredicate(p => p.CategoriesId().ContainsAll("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));

//match a product that is in one of the the given categories
productDiscountDraft.SetPredicate(p => p.CategoriesId().ContainsAny("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));

//match a product that is in the two given categories and in no others
productDiscountDraft.SetPredicate(p => p.CategoriesId().IsIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));

//match a product that is not in a given category
productDiscountDraft.SetPredicate(p => p.CategoriesId().IsNotIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

//match the prices above 12€ for any countries except France that do not have a customer group set
productDiscountDraft.SetPredicate(p => p.CentAmount() > 1200 && p.Currency() == "EUR" && p.Country() != "FR" && p.CustomerGroup().Id.IsNotDefined());

//match all product variants that have size "L" and have the color white and black   
//size is an EnumType attribute for which the key is specified in the predicate,
//color is a SetType of Enums for which the keys are listed in the predicate.
productDiscountDraft.SetPredicate(p => p.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value == "L") && p.Attributes().Any(a => a.Name == "colors" && a.ToSetEnumAttribute().ContainsAll("black", "white")));

//match all product variants with the given sku, the boolean attribute available set to true and the number attribute weight less than 100
productDiscountDraft.SetPredicate(p => p.Sku() == "AB-12" && p.Attributes().Any(a => a.Name == "available" && a.ToBooleanAttribute().Value == true) && p.Attributes().Any(a => a.Name == "weight" && a.ToNumberAttribute().Value < 100));

//match all products that are in the given category or in a category that is a descendant of the given category
productDiscountDraft.SetPredicate(p => p.CategoriesWithAncestorsId().Contains("abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));
```

## Cart Predicates
```c#
var cartDiscountDraft = new CartDiscountDraft()
```
### LineItem Predicate Examples:
```c#
var lineItemsCartDiscountTarget = new LineItemsCartDiscountTarget();
cartDiscountDraft.Target = lineItemsCartDiscountTarget;

// matches all line items
lineItemsCartDiscountTarget.Predicate = "true";
//or
lineItemsCartDiscountTarget.SetPredicate(l => true);

// matches line item with SKU "SKU-123" only if the price is a net price
lineItemsCartDiscountTarget.SetPredicate(l => l.Sku() == "SKU-123" && l.TaxRate.IncludedInPrice == false);

// matches a line item by product type, a specific product and at least 3 'rating' attributes
lineItemsCartDiscountTarget.SetPredicate(l => l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && l.Attributes().Any(a => a.Name == "rating" && ((NumberAttribute)a).Value > 3) && (l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh" || l.ProductId == "ba3e4ee7-30fa-400b-8155-46ebf423d793"));

// matches a line item that has the custom field "gender" to be "alien"
lineItemsCartDiscountTarget.SetPredicate(l => l.Custom.Fields["gender"].ToString() == "alien");

//matches a line item that is not in a given category
lineItemsCartDiscountTarget.SetPredicate(l => l.CategoriesId().IsNotIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

```

### CustomLineItem Predicate Examples:
```c#
var customLineItemsCartDiscountTarget = new CustomLineItemsCartDiscountTarget();
cartDiscountDraft.Target = customLineItemsCartDiscountTarget;

// matches all custom line items
customLineItemsCartDiscountTarget.Predicate = "true";
//or
customLineItemsCartDiscountTarget.SetPredicate(c => true);

// matches custom line items with price of individual items bigger than 10.50 EUR only if the price is a net price
customLineItemsCartDiscountTarget.SetPredicate(c => c.Money > Money.FromDecimal("EUR", 10.51M, MidpointRounding.ToEven).moneyString() && c.TaxRate.IncludedInPrice == false);

// matches a custom line item by slug
customLineItemsCartDiscountTarget.SetPredicate(c => c.Slug == "adidas-superstar-2");

// matches a custom line item that has the custom field "gender" to be "alien"
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["gender"].ToString() == "alien");

//identifies a custom field by ID (where the field is a Reference type)
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["reference-field"].ToString() == "bac1a3a5-3807-4f5b-9d07-0611984ecae8");

//checks if a custom field is contained in a collection, where the field is a reference type
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["reference-field"].ContainsAny("bac1a3a5-3807-4f5b-9d07-0611984ecae8", "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

//checks if a custom field has a value equal to 18.00 EUR, if the field is a Money type
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["price"].ToString() == "18.00 EUR");

//checks if centAmount equal to 18
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["price"].ToMoney().CentAmount == 18);

//checks if currencyCode equal EUR
customLineItemsCartDiscountTarget.SetPredicate(c => c.Custom.Fields["price"].ToMoney().CurrencyCode == "EUR");

```

### Cart Predicate Examples:
```c#
var money = Money.FromDecimal("USD", 10m);
var money2 = Money.FromDecimal("EUR", 10.50m);
var totalMoney = Money.FromDecimal("EUR", 800);
var startDate = DateTime.Parse("2019-09-11", CultureInfo.InvariantCulture);
            
// matches a cart with total line item cost bigger or equal to 10 USD (which excludes other costs, like shipping)
cartDiscountDraft.SetCartPredicate(c => c.LineItemTotal(true) > money.moneyString());

// matches a cart only when it has exactly 2 like items that have product with size "xxl" or "xl"
cartDiscountDraft.SetCartPredicate(c => c.LineItemCount(l => l.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value.In("xxl", "xl"))) == 2);

// matches a cart by customer information
cartDiscountDraft.SetCartPredicate(c.Customer().Email == "john@example.com" && c.Customer().CustomerGroup.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");

// matches a cart with a minimum total price and at least one lineItem that satisfies a price, a productType, a size attribute or a specific product
cartDiscountDraft.SetCartPredicate(c => c.TotalPrice > totalMoney.moneyString() && c.LineItemCount( l => l.Price.Value > money2.moneyString() && l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" && l.Attributes().Any(a => a.Name == "size" && a.ToTextAttribute().Value.In("xl", "xxl")) || l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh") > 0);

// matches a cart with custom.bookingStart = 11.09.2019 and custom.bookingEnd = 11.10.2019
cartDiscountDraft.SetCartPredicate(c => c.Custom.Fields["bookingStart"] == startDate.AsDate() && c.Custom.Fields["bookingEnd"] == DateTime.Parse("2019-10-11", CultureInfo.InvariantCulture).AsDate());

// matches a cart for a family (at least 2 adults and at least one youth)
cartDiscountDraft.SetCartPredicate(c => c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "adult") >= 2 && c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "youth") >= 1);
```