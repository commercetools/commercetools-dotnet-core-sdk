## Query Predicates Examples

```c#
var categoryQueryCommand = new QueryCommand<Category>();
var customerQueryCommand = new QueryCommand<Customer>();
var productQueryCommand = new QueryCommand<Product>();
var channelQueryCommand = new QueryCommand<Channel>();
var queryCommand = new QueryCommand<ProductProjection>();
```

// Compare a field's value to a given value
```c#

// query catgeories with Key "c14"
categoryQueryCommand.Where(c => c.Key == "c14"); // predicate: key = "c14"

// query catgeories with Key not equal "c14"
categoryQueryCommand.Where(c => c.Key != "c14"); // predicate: key != "c14"

// query categories with version greater than 30 
categoryQueryCommand.Where(c => c.Version > 30); // predicate: version > 30

// query categories with version 30 or less 
categoryQueryCommand.Where(c => c.Version <= 30); // predicate: version <= 30
```

// if value from variable use valueOf like below
```c#
var categoryKey = "c14";
var category = new Category { Key = "c14" };
categoryQueryCommand.Where(c => c.Key == categoryKey.valueOf());
categoryQueryCommand.Where(c => c.Key == category.Key.valueOf());
```

// Combine any two conditional expressions in a logical conjunction / disjunction like below
```c#

// query categories with key "c14" and with Version 30
// predicate: key = "c14" and version = 30
categoryQueryCommand.Where(c => c.Key == "c14" && c.Version == 30);

// query categories with key "c14" or with Version 30
// predicate: key = "c14" or version = 30
categoryQueryCommand.Where(c => c.Key == "c14" || c.Version == 30);
```

// Negate any other conditional expression
```c#

// predicate: not(key = "c14" and version = 30)
categoryQueryCommand.Where(c => !(c.Key == "c14" && c.Version == 30));
```

// Check whether a field's value is or is not contained in
// a specified set of values.
```c#

// predicate: key in ("c14", "c15")
categoryQueryCommand.Where(c => c.Key.In("c14", "c15"));

// predicate: key not in ("c14", "c15")
categoryQueryCommand.Where(c => c.Key.NotIn("c14", "c15")); 
```

// Check whether an array contains all or any of a set of values
```c#

// predicate: shippingAddressIds contains all ("c14", "c15")
customerQueryCommand.Where(c => c.ShippingAddressIds.ContainsAll("c14", "c15"));

// predicate: shippingAddressIds contains any ("c14", "c15")
customerQueryCommand.Where(c => c.ShippingAddressIds.ContainsAny("c14", "c15"));
```

// Check whether an array is empty
```c#

// predicate: shippingAddressIds is empty
customerQueryCommand.Where(c => c.ShippingAddressIds.IsEmpty());
```

// Check whether a field exists & has a non-null value
```c#

// predicate: key is defined
categoryQueryCommand.Where(c => c.Key.IsDefined());

// predicate: key is not defined
categoryQueryCommand.Where(c => c.Key.IsNotDefined());
```

// Descend into nested objects
```c#

// predicate: masterData(current(slug(en = "product")))
productQueryCommand.Where(p => p.MasterData.Current.Slug["en"] == "product");
```

// Descend into nested arrays of objects
```c#

// predicate: attributes(name = "text-name" and value = "text-value")
productQueryCommand.Where(p => 
                p.Attributes.Any(a => 
                                    a.ToTextAttribute().Name == "text-name" && 
                                    a.ToTextAttribute().Value == "text-value"
                                )
                    ); 
```

// Query GeoJSON field within a circle
```c#
// The two first parameters are the longitude and latitude of the circle's center.
// The third parameter is the radius of the circle in meter.
// predicate: geoLocation within circle(13.37774, 52.51627, 1000)
channelQueryCommand.Where(c => c.GeoLocation.WithinCircle(13.37774, 52.51627, 1000));
```

// for missing attribute
```c#

// predicate: variants(not(attributes(name="attribute-name")))
queryCommand.Where(p => 
        p.Variants.Any(variant => 
                !variant.Attributes.Any(a => 
                        a.ToTextAttribute().Name == "attribute-name")
                )
        );
```

// for single attribute value of TextType
```c#

// predicate: variants(attributes(name="attribute-name" and value="attribute-value"))
queryCommand.Where(p => 
            p.Variants.Any(variant => 
                 variant.Attributes.Any(a => 
                        a.ToTextAttribute().Name == "attribute-name" 
                     && a.ToTextAttribute().Value == "attribute-value")
                    )
            );
```
// for multiple attribute values of TextType with same name
```c#

// predicate: variants(attributes(name="attr-name" and value in ("attr-1", "attr-2")))
queryCommand.Where(p => 
      p.Variants.Any(variant => 
          variant.Attributes.Any(a => 
              a.ToTextAttribute().Name == "attr-name" 
           && a.ToTextAttribute().Value.In("attr-1", "attr-2"
                    )
                )
            )
        );
```

// for single attribute value of LTextType
```c#

// predicate: variants(attributes(name="attr-name" and value(en="attr-value")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToLocalizedTextAttribute().Name == "attr-name" 
            &&  a.ToLocalizedTextAttribute().Value["en"] == "attr-value")
            )
        );
```

// for multiple attribute values of LTextType with same name
```c#

// predicate: variants(attributes(name="attr-name" and value(en="eng" or de="ger")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
            a.ToLocalizedTextAttribute().Name == "attr-name" 
                && (a.ToLocalizedTextAttribute().Value["en"] == "eng" 
                 || a.ToLocalizedTextAttribute().Value["de"] == "ger")
            )
        )
    );
```

// for EnumType or LocalizableEnumType
```c#

// predicate: variants(attributes(name="attribute-name" and value(key="enum-key")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToEnumAttribute().Name == "attribute-name" 
             && a.ToEnumAttribute().Value.Key == "enum-key")
            )
    );
```

// for MoneyType (currencyCode is required)
```c#

// predicate: variants(attributes(name="att" and value(centAmount=99 and currencyCode="EUR")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToMoneyAttribute().Name == "att" 
             && a.ToMoneyAttribute().Value.CentAmount == 99 
             && a.ToMoneyAttribute().Value.CurrencyCode == "EUR")
        )
    );
```
// for MoneyType with centAmount within a specific range (currencyCode is required)
```c#

// predicate: variants(attributes(name="att" and value(centAmount > 99 and centAmount < 101 and currencyCode="EUR")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
            a.ToMoneyAttribute().Name == "att" 
         && a.ToMoneyAttribute().Value.CentAmount > 99 
         && a.ToMoneyAttribute().Value.CentAmount < 101 
         && a.ToMoneyAttribute().Value.CurrencyCode == "EUR")
        )
    );
```

// for NumberType
```c#

// predicate: variants(attributes(name="attribute-name" and value=999))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToNumberAttribute().Name == "attribute-name" 
             && a.ToNumberAttribute().Value = 999)
        )
    );
```
// for NumberType with value within a specific range
```c#

// predicate: variants(attributes(name="attr" and value > 999 and value < 1001 ))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToNumberAttribute().Name == "attr" 
             && a.ToNumberAttribute().Value > 999 
             && a.ToNumberAttribute().Value < 1001)
        )
    );
```

// for DateType, TimeType or DateTimeType
```c#
var dateTime1 = DateTime.Parse("2019-06-04T12:27:55.344Z");

// predicate: variants(attributes(name="c-DateTime" and value="2019-06-04T12:27:55.344Z"))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
               a.ToDateTimeAttribute().Name == "c-DateTime" 
            && a.ToDateTimeAttribute().Value == dateTime1.AsDateTimeAttribute())
        )
    );

// predicate: variants(attributes(name="c-Date" and value="2019-06-04"))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToDateAttribute().Name == "c-Date" 
             && a.ToDateAttribute().Value == dateTime1.AsDateAttribute())
        )
    );

// predicate: variants(attributes(name="c-Time" and value="12:27:55.344"))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToTimeAttribute().Name == "c-Time" 
             && a.ToTimeAttribute().Value == dateTime1.TimeOfDay.AsTimeAttribute())
        )
    );
```

// for DateType, TimeType or DateTimeType with a value within a specific range
```c#

var dateTime1 = DateTime.Parse("2019-06-04T12:27:55.344Z");
var dateTime2 = DateTime.Parse("2020-11-09T11:27:55.344Z");

// predicate: variants(attributes(name="c-DateTime" and value > "2019-06-04T12:27:55.344Z" and value < "2020-11-09T11:27:55.344Z"))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
               a.ToDateTimeAttribute().Name == "c-DateTime" 
            && a.ToDateTimeAttribute().Value > dateTime1.AsDateTimeAttribute()
            && a.ToDateTimeAttribute().Value < dateTime2.AsDateTimeAttribute())
        )
    );


// predicate: variants(attributes(name="c-Date" and value > "2019-06-04" and value < "2020-11-09"))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
               a.ToDateAttribute().Name == "c-Date" 
            && a.ToDateAttribute().Value > dateTime1.AsDateAttribute()
            && a.ToDateAttribute().Value < dateTime2.AsDateAttribute())
        )
    );
```

// for ReferenceType
```c#
// predicate: variants(attributes(name = "cat-Ref" and value(typeId = "category") and 
//           value(id = "963cbb75-c604-4ad2-841c-890b792224ee")))
queryCommand.Where(p => 
    p.Variants.Any(variant => 
        variant.Attributes.Any(a => 
                a.ToReferenceAttribute().Name == "cat-Ref" 
             && a.ToReferenceAttribute().Value.TypeId == ReferenceTypeId.Category 
             && a.ToReferenceAttribute().Value.Id == "963cbb75-c604-4ad2-841c-890b792224ee" )
        )
    );
```

## Discount Predicates Examples

### Product Discounts Examples:
```c#

var productDiscountDraft = new ProductDiscountDraft();

// match a specific variant in the specific product
// predicate: product.id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" and variant.id = 1
productDiscountDraft.SetPredicate(p => 
    p.ProductId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" 
    && product.VariantId() == 1);

// match a product that is only in the given category
// predicate: categories.id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"
productDiscountDraft.SetPredicate(p => 
    p.CategoryId() == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");

// match a product that is in the given category
// predicate: categories.id contains "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"
productDiscountDraft.SetPredicate(p => 
    p.CategoriesId().Contains("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

// match a product that is in all of the the given categories
// predicate: categories.id contains all ("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
//                                      "abcd9a23-14e3-40d0-aee2-3e612fcbefgh")
productDiscountDraft.SetPredicate(p => 
    p.CategoriesId().ContainsAll("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
                            "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));

// match a product that is in one of the the given categories
// predicate: categories.id contains any ("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
//                                      "abcd9a23-14e3-40d0-aee2-3e612fcbefgh")
productDiscountDraft.SetPredicate(p => 
    p.CategoriesId().ContainsAny("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
                                "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));


// match a product that is in the two given categories and in no others
// predicate: categories.id = ("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
//                          "abcd9a23-14e3-40d0-aee2-3e612fcbefgh")
productDiscountDraft.SetPredicate(p => 
    p.CategoriesId().IsIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7", 
                             "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));

// match a product that is not in a given category
// predicate: categories.id != ("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7")
productDiscountDraft.SetPredicate(p => 
    p.CategoriesId().IsNotIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));

// match the prices above 12€ for any countries except France that do not have a customer group set
// predicate: centAmount > 1200 and currency = "EUR" 
//                      and country != "FR" and customerGroup.id is not defined
productDiscountDraft.SetPredicate(p => 
    p.CentAmount() > 1200 
    && p.Currency() == "EUR" 
    && p.Country() != "FR" 
    && p.CustomerGroup().Id.IsNotDefined());

// match all product variants that have size "L" and have the color white and black   
// size is an EnumType attribute for which the key is specified in the predicate,
// color is a SetType of Enums for which the keys are listed in the predicate.
// predicate: attributes.size = "L" and attributes.colors contains all ("black", "white")
productDiscountDraft.SetPredicate(p => 
    p.Attributes().Any(a => 
        a.Name == "size" && a.ToTextAttribute().Value == "L") 
        && p.Attributes().Any(a => a.Name == "colors" 
        && a.ToSetEnumAttribute().ContainsAll("black", "white"))
    );

// match all product variants with the given sku, the boolean attribute available set to true and the number attribute weight less than 100
// predicate: sku = "AB-12" and attributes.available = true and attributes.weight < 100
productDiscountDraft.SetPredicate(p => 
    p.Sku() == "AB-12" 
    && p.Attributes().Any(a => 
        a.Name == "available" 
        && a.ToBooleanAttribute().Value == true) 
        && p.Attributes().Any(a => 
                        a.Name == "weight" && a.ToNumberAttribute().Value < 100)
    );

// match all products that are in the given category or in a category that is a descendant of the given category
// predicate: categoriesWithAncestors.id contains "abcd9a23-14e3-40d0-aee2-3e612fcbefgh"
productDiscountDraft.SetPredicate(p => 
          p.CategoriesWithAncestorsId().Contains("abcd9a23-14e3-40d0-aee2-3e612fcbefgh"));
```

## Cart Predicates
```c#
var cartDiscountDraft = new CartDiscountDraft();
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
// predicate: sku = "SKU-123" and taxRate.includedInPrice = false
lineItemsCartDiscountTarget.SetPredicate(l => 
        l.Sku() == "SKU-123" && l.TaxRate.IncludedInPrice == false);

// matches a line item by product type, a specific product and at least 3 'rating' attributes
// predicate: productType.id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" 
//             and attributes.rating > 3 and (product.id = "abcd9a23-14e3-40d0-aee2-3e612fcbefgh" 
//                                     or product.id = "ba3e4ee7-30fa-400b-8155-46ebf423d793")
lineItemsCartDiscountTarget.SetPredicate(l => 
        l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" 
     && l.Attributes().Any(a => 
                                a.Name == "rating" 
                             && ((NumberAttribute)a).Value > 3
                          ) 
     && (l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh" || 
         l.ProductId == "ba3e4ee7-30fa-400b-8155-46ebf423d793"
        )
    );

// matches a line item that has the custom field "gender" to be "alien"
// predicate: custom.gender = "alien"
lineItemsCartDiscountTarget.SetPredicate(l => 
        l.Custom.Fields["gender"].ToString() == "alien");

// matches a line item that is not in a given category
// predicate: categories.id != ("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7")
lineItemsCartDiscountTarget.SetPredicate(l => 
        l.CategoriesId().IsNotIn("f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"));
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
// predicate: money > "10.51 EUR" and taxRate.includedInPrice = false
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Money > Money.FromDecimal("EUR", 10.51M, MidpointRounding.ToEven).moneyString() 
        && c.TaxRate.IncludedInPrice == false);

// matches a custom line item by slug
// predicate: slug = "adidas-superstar-2"
customLineItemsCartDiscountTarget.SetPredicate(c => c.Slug == "adidas-superstar-2");

// matches a custom line item that has the custom field "gender" to be "alien"
// predicate: custom.gender = "alien"
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["gender"].ToString() == "alien");

//identifies a custom field by ID (where the field is a Reference type)
// predicate: custom.`reference-field` = "bac1a3a5-3807-4f5b-9d07-0611984ecae8"
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["reference-field"].ToString() == "bac1a3a5-3807-4f5b-9d07-0611984ecae8");

// checks if a custom field is contained in a collection, where the field is a reference type
// predicate: custom.`reference-field` contains any 
//              ("bac1a3a5-3807-4f5b-9d07-0611984ecae8", "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7")
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["reference-field"].ContainsAny(
            "bac1a3a5-3807-4f5b-9d07-0611984ecae8", "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7")
        );

// checks if a custom field has a value equal to 18.00 EUR, if the field is a Money type
// predicate: custom.price = "18.00 EUR"
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["price"].ToString() == "18.00 EUR");

// checks if centAmount equal to 18
// predicate: custom.price.centAmount = 18
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["price"].ToMoney().CentAmount == 18);

// checks if currencyCode equal EUR
// predicate: custom.price.currencyCode = "EUR"
customLineItemsCartDiscountTarget.SetPredicate(c => 
        c.Custom.Fields["price"].ToMoney().CurrencyCode == "EUR");
```

### Cart Predicate Examples:
```c#

var money = Money.FromDecimal("USD", 10m);
var money2 = Money.FromDecimal("EUR", 10.50m);
var totalMoney = Money.FromDecimal("EUR", 800);
var startDate = DateTime.Parse("2019-09-11", CultureInfo.InvariantCulture);
            
// matches a cart with total line item cost bigger or equal to 10 USD (which excludes other costs, like shipping)
// predicate: lineItemTotal(true) > "10 USD"
cartDiscountDraft.SetCartPredicate(c => c.LineItemTotal(true) > money.moneyString());

// matches a cart only when it has exactly 2 like items that have product with size "xxl" or "xl"
// predicate: lineItemCount(attributes.size in ("xxl", "xl")) = 2
cartDiscountDraft.SetCartPredicate(c => 
        c.LineItemCount(l => l.Attributes().Any(a => 
                a.Name == "size" && a.ToTextAttribute().Value.In("xxl", "xl"))) == 2);

// matches a cart by customer information
// predicate: customer.email = "john@example.com" and customer.customerGroup.id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7"
cartDiscountDraft.SetCartPredicate(c => 
        c.Customer().Email == "john@example.com" 
        && c.Customer().CustomerGroup.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7");

// matches a cart with a minimum total price and at least one lineItem that satisfies a price, a productType, a size attribute or a specific product
// predicate: totalPrice > "800 EUR" and lineItemCount(price > "10.5 EUR" 
//          and productType.id = "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" 
//          and attributes.size in ("xl", "xxl") 
//          or product.id = "abcd9a23-14e3-40d0-aee2-3e612fcbefgh") > 0
cartDiscountDraft.SetCartPredicate(c => 
        c.TotalPrice > totalMoney.moneyString() 
        && c.LineItemCount( l => l.Price.Value > money2.moneyString() 
                && l.ProductType.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7" 
                && l.Attributes().Any(a => 
                        a.Name == "size" && a.ToTextAttribute().Value.In("xl", "xxl")) 
                || l.ProductId == "abcd9a23-14e3-40d0-aee2-3e612fcbefgh") > 0);

// matches a cart with custom.bookingStart = 11.09.2019 and custom.bookingEnd = 11.10.2019
// predicate: custom.bookingStart = "2019-09-11" and custom.bookingEnd = "2019-10-11"
cartDiscountDraft.SetCartPredicate(c => 
        c.Custom.Fields["bookingStart"] == startDate.AsDate() 
        && c.Custom.Fields["bookingEnd"] == DateTime.Parse(
                            "2019-10-11", CultureInfo.InvariantCulture).AsDate());

// matches a cart for a family (at least 2 adults and at least one youth)
// predicate: lineItemCount(custom.age = "adult") >=2 and lineItemCount(custom.age = "youth") >=1
cartDiscountDraft.SetCartPredicate(c => 
        c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "adult") >= 2 
     && c.LineItemCount(l=> l.Custom.Fields["age"].ToString() == "youth") >= 1);
```