using commercetools.Sdk.Domain;
using commercetools.Sdk.Linq.Extensions.Carts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace commercetools.Sdk.Linq.Tests
{
    public class CartPredicateTests
    {
        [Fact]
        public void LineItemCountByProductIdGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(l => l.ProductId == "45224437-12bd-4742-830c-3a36b52541d3") > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemCountTrueGreaterThanTen()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void LineItemTotal()
        {
            Expression<Func<Cart, bool>> expression = c => c.LineItemCount(true) > 10;
            var result = string.Empty;
            Assert.Equal("lineItemCount(product.id = \"45224437-12bd-4742-830c-3a36b52541d3\") > 10", result);
        }

        [Fact]
        public void CartPredicateCustom()
        {
            Expression<Func<Cart, bool>> expression = c => ((StringField)c.Custom.Fields)["description"] == "my description";
            var result = string.Empty;
            Assert.Equal("custom.description = \"my description\"", result);
        }

        [Fact]
        public void CartPredicateCustomTypeId()
        {
            Expression<Func<Cart, bool>> expression = c => c.Custom.Type.Id == "f6a19a23-14e3-40d0-aee2-3e612fcb1bc7";
            var result = string.Empty;
            Assert.Equal("custom.type.id = \"f6a19a23-14e3-40d0-aee2-3e612fcb1bc7\"", result);
        }

//        does not exist
//currency
//customer.customerNumber
//customer.customerGroup.key
//customer.firstName - String
//customer.lastName - String
//customer.middleName - String
//customer.title - String
//customer.isEmailVerified - Boolean
//customer.externalId - String
//customer.createdAt - String - DateTime
//customer.lastModifiedAt - String - DateTime
//custom.type.key - key

//different name
//TotalNet - net
//customerId customer.id
//customerEmail customer.email
//customerGroup.Id customer.customerGroup.id


    }
}
