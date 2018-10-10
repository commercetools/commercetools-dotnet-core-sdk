using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class Expansion<T>
    {
        public Expression<Func<T, Reference>> Expression { get; set; }

        public Expansion(Expression<Func<T, Reference>> expression)
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
    }
}
