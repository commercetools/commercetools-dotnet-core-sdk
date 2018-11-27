using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public class GroupFilterVisitorFactory
    {
        public GroupFilterVisitor GetGroupFilterVisitor(List<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException();
            }
            // TODO check if all accessors are the same
            bool isRangeGroup = expressions.All(x => x is MethodCallExpression && ((MethodCallExpression)x).Method.Name == "Range");
            if (isRangeGroup)
            {
                RangeGroupFilterVisitor rangeGroupFilterVisitor = new RangeGroupFilterVisitor(expressions.Cast<MethodCallExpression>().ToList());
                return rangeGroupFilterVisitor;
            }
            else
            {
                MixedGroupFilterVisitor mixedGroupFilterVisitor = new MixedGroupFilterVisitor(expressions);
                return mixedGroupFilterVisitor;
            }
        }
    }
}