using commercetools.Sdk.Domain;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Products.Attributes;
using commercetools.Sdk.Linq.Query;
using Xunit;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Messages;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Tests
{
    public class QueryPredicateTests : IClassFixture<LinqFixture>
    {
        private readonly LinqFixture linqFixture;

        public QueryPredicateTests(LinqFixture linqFixture)
        {
            this.linqFixture = linqFixture;
        }

        [Fact]
        public void ExpressionStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key == "c14";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringEqualVar()
        {
            string key = "c14";
            Expression<Func<Category, bool>> expression = c => c.Key == key;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringEqualVarProperty()
        {
            Category category = new Category
            {
                Key = "c14"
            };
            Expression<Func<Category, bool>> expression = c => c.Key == category.Key.valueOf();
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringEqualVarPropertyNonLocal()
        {
            Category category = new Category
            {
                Key = "c14"
            };
            // For comparison of non local values the valueOf extension method has to be used
            Expression<Func<Category, bool>> expression = c => c.Key == category.Key.valueOf();
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringNotEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\"", result);
        }

        [Fact]
        public void ExpressionStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and version = 30", result);
        }

        [Fact]
        public void ExpressionStringOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" or version = 30", result);
        }

        [Fact]
        public void ExpressionStringAndOr()
        {
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Name["en"] == "men" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and name(en = \"men\") or version = 30", result);
        }

        [Fact]
        public void ExpressionDictionaryVar()
        {
            string language = "en";
            Expression<Func<Category, bool>> expression = c => c.Key != "c14" && c.Name[language] == "men" || c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key != \"c14\" and name(en = \"men\") or version = 30", result);
        }

        [Fact]
        public void ExpressionNotStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key = \"c14\")", result);
        }

        [Fact]
        public void ExpressionNotStringAnd()
        {
            Expression<Func<Category, bool>> expression = c => !(c.Key == "c14" && c.Version == 30);
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key = \"c14\" and version = 30)", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "13c4ee51-ff35-490f-8e43-349e39c34646";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("parent(id = \"13c4ee51-ff35-490f-8e43-349e39c34646\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelStringEqual()
        {
            Expression<Func<ProductCatalogData, bool>> expression = p => p.Current.MasterVariant.Key == "p15";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("current(masterVariant(key = \"p15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyIntEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Version == 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("version = 30", result);
        }

        [Fact]
        public void ExpressionPropertyIntLessThan()
        {
            Expression<Func<Category, bool>> expression = c => c.Version < 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("version < 30", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelIntLessThan()
        {
            Expression<Func<ProductData, bool>> expression = p => p.MasterVariant.Id < 30;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterVariant(id < 30)", result);
        }

        [Fact]
        public void ExpressionPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.In("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyIsDefined()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.IsDefined();
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key is defined", result);
        }

        [Fact]
        public void ExpressionNotPropertyInString()
        {
            Expression<Func<Category, bool>> expression = c => !c.Key.In("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("not(key in (\"c14\", \"c15\"))", result);
        }

        [Fact]
        public void ExpressionPropertyNotInString()
        {
            Expression<Func<Category, bool>> expression = c => c.Key.NotIn("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("key not in (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyContainsAllString()
        {
            Expression<Func<Customer, bool>> expression = c => c.ShippingAddressIds.ContainsAll("c14", "c15");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("shippingAddressIds contains all (\"c14\", \"c15\")", result);
        }

        [Fact]
        public void ExpressionPropertyDictionaryEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Name["en"] == "men";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("name(en = \"men\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelDictionaryEqual()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Slug["en"] == "product";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(slug(en = \"product\")))", result);
        }

        [Fact]
        public void ExpressionPropertyTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToTextAttribute().Name == "text-name" && a.ToTextAttribute().Value == "text-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value = \"text-value\")", result);
        }
        
        [Fact]
        public void ExpressionPropertyMoneyAttributeValueEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToMoneyAttribute().Name == "attribute-name" && a.ToMoneyAttribute().Value.CentAmount == 999 && a.ToMoneyAttribute().Value.CurrencyCode == "EUR"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value(centAmount = 999) and value(currencyCode = \"EUR\")))", result);
        }
        
        [Fact]
        public void ExpressionPropertyNumberAttributeValueWithinRange()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToNumberAttribute().Name == "attribute-name" && a.ToNumberAttribute().Value > 999 && a.ToNumberAttribute().Value < 1001));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value > 999 and value < 1001))", result);
        }
        
        [Fact]
        public void ExpressionPropertyDateTimeAttributeValueVariableEqual()
        {
            var cDateTime = DateTime.Parse("2019-09-11T15:27:55.123+02:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToLocalTime();
            Assert.Equal(DateTimeKind.Local, cDateTime.Kind);//local time
            
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "C-DateTime" && a.ToDateTimeAttribute().Value == cDateTime.AsDateTimeAttribute()));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-DateTime\" and value = \"2019-09-11T13:27:55.123Z\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyDateTimeAttributeValueParseEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateTimeAttribute().Name == "C-DateTime" && a.ToDateTimeAttribute().Value == DateTime.Parse("2019-10-11T15:25:12.123+02:00", CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.AdjustToUniversal).AsDateTimeAttribute()));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-DateTime\" and value = \"2019-10-11T13:25:12.123Z\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyDateAttributeValueVariableEqual()
        {
            var cDate = DateTime.Parse("2019-09-11T15:00:00.000+02:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToLocalTime();
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateAttribute().Name == "C-Date" && a.ToDateAttribute().Value == cDate.AsDateAttribute()));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-Date\" and value = \"2019-09-11\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyDateAttributeValueParseEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToDateAttribute().Name == "C-Date" && a.ToDateAttribute().Value == DateTime.Parse("2019-10-11", CultureInfo.InvariantCulture).AsDateAttribute()));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-Date\" and value = \"2019-10-11\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyTimeAttributeValueParseEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTimeAttribute().Name == "C-Time" && a.ToTimeAttribute().Value == DateTime.Parse("2019-10-11T15:33:11.123+02:00", CultureInfo.GetCultureInfo("de-DE"), DateTimeStyles.AdjustToUniversal).TimeOfDay.AsTimeAttribute()));//utc time
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-Time\" and value = \"13:33:11.123\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyTimeAttributeValueVariableEqual()
        {
            var cTime = new TimeSpan(0,13, 22, 12, 123 );
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTimeAttribute().Name == "C-Time" && a.ToTimeAttribute().Value == cTime.AsTimeAttribute()));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"C-Time\" and value = \"13:22:12.123\"))", result);
        }
        
        [Fact]
        public void ExpressionPropertyReferenceTypeAttributeValueEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToReferenceAttribute().Name == "attribute-name" && a.ToReferenceAttribute().Value.TypeId == ReferenceTypeId.Category && a.ToReferenceAttribute().Value.Id == "963cbb75-c604-4ad2-841c-890b792224ee" ));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value(typeId = \"category\") and value(id = \"963cbb75-c604-4ad2-841c-890b792224ee\")))", result);
        }
        

        [Fact]
        public void ExpressionPropertyTextAttributeValueEqualCaseSensitive()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToTextAttribute().Name == "Color" && a.ToTextAttribute().Value == "Red");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"Color\" and value = \"Red\")", result);
        }

        [Fact]
        public void ExpressionPropertyTextAttributeCastValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => ((TextAttribute)a).Name == "text-name" && ((TextAttribute)a).Value == "text-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value = \"text-value\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "text-name" && a.ToLocalizedTextAttribute().Value["en"] == "text-value-en");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\"))", result);
        }

        [Fact]
        public void ExpressionPropertyEnumAttributeValueEqual()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToEnumAttribute().Name == "enum-name" && a.ToEnumAttribute().Value.Key == "enum-value");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"enum-name\" and value(key = \"enum-value\"))", result);
        }
        
        [Fact]
        public void ExpressionProductProjectionVariantEnumAttributeValueEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToEnumAttribute().Name == "attribute-name" && a.ToEnumAttribute().Value.Key == "enum-key"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value(key = \"enum-key\")))", result);
        }

        [Fact]
        public void ExpressionPropertyPropertyGrouping()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "some id" || c.Parent.Id == "some other id";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("parent(id = \"some id\" or id = \"some other id\")", result);
        }

        [Fact]
        public void ExpressionPropertyLocalizedTextAttributeValueEqualGrouped()
        {
            Expression<Func<ProductVariant, bool>> expression = p => p.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "text-name" && (a.ToLocalizedTextAttribute().Value["en"] == "text-value-en" || a.ToLocalizedTextAttribute().Value["de"] == "text-value-de"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("attributes(name = \"text-name\" and value(en = \"text-value-en\" or de = \"text-value-de\"))", result);
        }

        [Fact]
        public void WithinCircle()
        {
            Expression<Func<Channel, bool>> expression = c => c.GeoLocation.WithinCircle(13.37774, 52.51627, 1000);//longitude, latitude, radius
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("geoLocation within circle(13.37774, 52.51627, 1000)", result);
        }

        [Fact]
        public void ExpressionProjectionPropertyLocalizedTextAttributeValueEqualGrouped()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToLocalizedTextAttribute().Name == "text-name" && (a.ToLocalizedTextAttribute().Value["en"] == "text-value-en" || a.ToLocalizedTextAttribute().Value["de"] == "text-value-de")));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"text-name\" and value(en = \"text-value-en\" or de = \"text-value-de\")))", result);
        }
        
        [Fact]
        public void ExpressionProjectionPropertyForMissingAttribute()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => !variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(not(attributes(name = \"attribute-name\")))", result);
        }
        
        [Fact]
        public void ExpressionProjectionPropertyTextAttributeValueEqual()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name" && a.ToTextAttribute().Value == "attribute-value"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value = \"attribute-value\"))", result);
        }
        [Fact]
        public void ExpressionProjectionPropertyTextAttributeValueIn()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Variants.Any(variant => variant.Attributes.Any(a => a.ToTextAttribute().Name == "attribute-name" && a.ToTextAttribute().Value.In("attribute-value-1", "attribute-value-2")));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("variants(attributes(name = \"attribute-name\" and value in (\"attribute-value-1\", \"attribute-value-2\")))", result);
        }

        [Fact]
        public void QueryInList()
        {
            Expression<Func<ProductProjection, bool>> expression = p => p.Categories.Any(reference => reference.Id.In("test"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("categories(id in (\"test\"))", result);
        }

        [Fact]
        public void QueryNestedProperty()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key == "test";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key = \"test\")))", result);
        }

        [Fact]
        public void QueryNestedCombine()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key == "test" && p.MasterData.Current.MasterVariant.Sku == "something";
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key = \"test\" and sku = \"something\")))", result);
        }

        [Fact]
        public void QueryNestedMemberInCombination()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key == "test" && p.MasterData.Current.MasterVariant.Sku.In("test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key = \"test\" and sku in (\"test\"))))", result);
        }

        [Fact]
        public void QueryNestedInCombination()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key.In("test") && p.MasterData.Current.MasterVariant.Sku.In("test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key in (\"test\") and sku in (\"test\"))))", result);
        }

        [Fact]
        public void QueryNestedInAnyCombination()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key.In("test") && p.MasterData.Current.Categories.Any(reference => reference.Id == "test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key in (\"test\")))) and masterData(current(categories(id = \"test\")))", result);
        }

        [Fact]
        public void QueryNestedDictionaryMethodCombination()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Name["en"] == "test" && p.MasterData.Current.Categories.Any(reference => reference.Id == "test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(name(en = \"test\"))) and masterData(current(categories(id = \"test\")))", result);
        }

        [Fact]
        public void QueryNestedIn()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key.In("test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key in (\"test\"))))", result);
        }

        [Fact]
        public void QueryNestedDefined()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.MasterVariant.Key.IsDefined();
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(masterVariant(key is defined)))", result);
        }

        [Fact]
        public void QueryNestedInList()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Categories.Any(reference => reference.Id.In("test"));
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(categories(id in (\"test\"))))", result);
        }

        [Fact]
        public void QueryNestedComparison()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Current.Categories.Any(reference => reference.Id == "test");
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(current(categories(id = \"test\")))", result);
        }
        
        [Fact]
        public void ExpressionIdPropertyIsGreaterThan()
        {
            var lastMessage = new GeneralMessage {Id = "92e3f00a-4e6d-4ad4-bd47-afe1630539f3"};
            Expression<Func<Message, bool>> expression = m => m.Id.IsGreaterThan(lastMessage.Id.valueOf());
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("id > \"92e3f00a-4e6d-4ad4-bd47-afe1630539f3\"", result);
        }
        
        [Fact]
        public void ExpressionIdPropertyIsGreaterThanOrEqual()
        {
            var lastMessage = new GeneralMessage {Id = "92e3f00a-4e6d-4ad4-bd47-afe1630539f3"};
            Expression<Func<Message, bool>> expression = m => m.Id.IsGreaterThanOrEqual(lastMessage.Id.valueOf());
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("id >= \"92e3f00a-4e6d-4ad4-bd47-afe1630539f3\"", result);
        }
        [Fact]
        public void ExpressionIdPropertyIsLessThan()
        {
            var lastMessage = new GeneralMessage {Id = "92e3f00a-4e6d-4ad4-bd47-afe1630539f3"};
            Expression<Func<Message, bool>> expression = m => m.Id.IsLessThan(lastMessage.Id.valueOf());
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("id < \"92e3f00a-4e6d-4ad4-bd47-afe1630539f3\"", result);
        }
        [Fact]
        public void ExpressionIdPropertyIsLessThanOrEqual()
        {
            var lastMessage = new GeneralMessage {Id = "92e3f00a-4e6d-4ad4-bd47-afe1630539f3"};
            Expression<Func<Message, bool>> expression = m => m.Id.IsLessThanOrEqual(lastMessage.Id.valueOf());
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("id <= \"92e3f00a-4e6d-4ad4-bd47-afe1630539f3\"", result);
        }
        
        [Fact]
        public void ExpressionExplicitEqualTrue()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Published == true;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(published = true)", result);
        }

        [Fact]
        public void ExpressionExplicitEqualFalse()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Published == false;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(published = false)", result);
        }

        [Fact]
        public void ExpressionImplicitEqualTrue()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.Published;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(published = true)", result);
        }

        [Fact]
        public void ExpressionImplicitEqualFalse()
        {
            Expression<Func<Product, bool>> expression = p => !p.MasterData.Published;
            IQueryPredicateExpressionVisitor queryPredicateExpressionVisitor = this.linqFixture.GetService<IQueryPredicateExpressionVisitor>();
            string result = queryPredicateExpressionVisitor.Render(expression);
            Assert.Equal("masterData(published = false)", result);
        }
    }
}
