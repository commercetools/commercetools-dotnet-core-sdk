using commercetools.Sdk.Domain.Carts;
using commercetools.Sdk.Domain.CustomerGroups;
using commercetools.Sdk.Domain.Customers;
using commercetools.Sdk.Domain.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Attribute = commercetools.Sdk.Domain.Products.Attributes.Attribute;

namespace commercetools.Sdk.Domain.Predicates
{
    public static class ProductDiscountExtensions
    {
        public static string ProductId(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string ProductKey(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string Sku(this Product source)
        {
            throw new NotImplementedException();
        }

        public static List<Attribute> Attributes(this Product source)
        {
            throw new NotImplementedException();
        }

        public static Money Price(this Product source)
        {
            throw new NotImplementedException();
        }

        public static double Amount(this Product source)
        {
            throw new NotImplementedException();
        }

        public static double CentAmount(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string Currency(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string Country(this Product source)
        {
            throw new NotImplementedException();
        }

        public static CustomerGroup CustomerGroup(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string CategoryId(this Product source)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<string> CategoriesId(this Product source)
        {
            throw new NotImplementedException();
        }

        public static bool IsIn(this IEnumerable<string> source, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static bool IsNotIn(this IEnumerable<string> source, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static bool IsNotIn(this string source, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<string> CategoriesWithAncestorsId(this Product source)
        {
            throw new NotImplementedException();
        }

        public static int VariantId(this Product source)
        {
            throw new NotImplementedException();
        }

        public static string VariantKey(this Product source)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsAll<T>(this SetAttribute<T> source, params string[] values)
        {
            throw new NotImplementedException();
        }
    }
}