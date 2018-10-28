using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq.Extensions.Carts
{
    public static class CartExtensions
    {
        public static int LineItemCount(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static int LineItemCount(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static int CustomLineItemCount(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static int CustomLineItemCount(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemNetTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemNetTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemNetTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemNetTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemGrossTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemGrossTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemGrossTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemGrossTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LineItemExists(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LineItemExists(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static bool ForAllLineItems(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static bool ForAllLineItems(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static string Currency(this Cart source)
        {
            throw new NotImplementedException();
        }

        public static Customer Customer(this Cart source)
        {
            throw new NotImplementedException();
        }

        public static string CustomerGroupKey(this Cart source)
        {
            throw new NotImplementedException();
        }

        public static string CustomerGroupKey(this Price source)
        {
            throw new NotImplementedException();
        }

        public static string CustomTypeKey(this Cart source)
        {
            throw new NotImplementedException();
        }

        public static string CustomTypeKey(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static bool Equal(this Money source, string parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LessThan(this Money source, string parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LessThanOrEqual(this Money source, string parameter)
        {
            throw new NotImplementedException();
        }

        public static bool GreaterThan(this Money source, string parameter)
        {
            throw new NotImplementedException();
        }

        public static bool GreaterThanOrEqual(this Money source, string parameter)
        {
            throw new NotImplementedException();
        }

        public static string ProductKey(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string CatalogId(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string Sku(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string CategoriesId(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string CategoriesKey(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string CategoriesWithAncestorsId(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string CategoriesWithAncestorsKey(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static List<Domain.Attribute> Attributes(this LineItem source)
        {
            throw new NotImplementedException();
        }

        public static string DiscountId(this Price source)
        {
            throw new NotImplementedException();
        }
    }
}
