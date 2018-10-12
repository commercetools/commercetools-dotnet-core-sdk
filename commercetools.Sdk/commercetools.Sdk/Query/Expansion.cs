﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ReferenceExpansion<T> : Expansion<T, Reference>
    {
        public ReferenceExpansion(Expression<Func<T, Reference>> expression) : base(expression)
        {
        }
    }

    public class AttributeExpansion<T> : Expansion<T, Attribute> where T: Product
    {
        public AttributeExpansion(Expression<Func<T, Attribute>> expression) : base(expression)
        {
        }
    }

    public class LineItemExpansion<T> : Expansion<T, LineItem> where T : ShoppingList
    {
        public LineItemExpansion(Expression<Func<T, LineItem>> expression) : base(expression)
        {
        }
    }

    public class Expansion<T, S>
    {
        public Expression<Func<T, S>> Expression { get; set; }

        public Expansion(Expression<Func<T, S>> expression)
        {
            this.Expression = expression;
        }
    }

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

        public static Attribute ExpandValues(this List<Attribute> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandProductSlugs(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }

        public static LineItem ExpandVariants(this List<LineItem> list)
        {
            return list.FirstOrDefault();
        }
    }
}
