namespace commercetools.Sdk.Domain
{
    using commercetools.Sdk.Domain.ShoppingLists;
    using commercetools.Sdk.Linq;
    using commercetools.Sdk.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    // TODO Refactor and split
    public static class ExpansionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="referenceList"></param>
        /// <returns></returns>
        /// <remarks>
        /// Only used in expansion expressions in order to expand by nested properties of arrays (lists).
        /// </remarks>
        public static T ExpandAll<T>(this List<T> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandProductSlugs(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }

        public static Attribute ExpandValues(this List<Attribute> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandVariants(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }
    }

    public class AttributeExpansion<T> : Expansion<T, Attribute> where T : Product
    {
        public AttributeExpansion(Expression<Func<T, Attribute>> expression) : base(expression)
        {
        }
    }

    public class Expansion<T, S> : Expansion<T>
    {
        public Expansion(Expression<Func<T, S>> expression) : base(expression)
        {
        }
    }

    public class Expansion<T>
    {
        public Expansion(Expression expression)
        {
            this.Expression = expression;
        }

        public Expression Expression { get; set; }

        public override string ToString()
        {
            return ServiceLocator.Current.GetService<IExpansionExpressionVisitor>().GetPath(this.Expression);
        }
    }

    public class LineItemExpansion<T> : Expansion<T, LineItem> where T : ShoppingList
    {
        public LineItemExpansion(Expression<Func<T, LineItem>> expression) : base(expression)
        {
        }
    }

    public class ReferenceExpansion<T> : Expansion<T, Reference>
    {
        public ReferenceExpansion(Expression<Func<T, Reference>> expression) : base(expression)
        {
        }
    }
}