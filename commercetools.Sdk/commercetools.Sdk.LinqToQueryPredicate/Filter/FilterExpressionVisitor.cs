using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    // TODO Refactor
    public class FilterExpressionVisitor : IFilterExpressionVisitor
    {
        public string Render(Expression expression)
        {
            FilterVisitorFactory filterVisitorFactory = new FilterVisitorFactory();
            return filterVisitorFactory.CreateFilterVisitor(expression).Render();
            //// c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e"
            //if (expression.NodeType == ExpressionType.Lambda)
            //{
            //    return Render(((LambdaExpression)expression).Body);
            //}
            //// c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e") || c.Id.Subtree("2fd1d652-2533-40f1-97d7-713ac24668b1")
            //if (expression is BinaryExpression)
            //{
            //    return GetGroup((BinaryExpression)expression).Render();
            //}
            //return GetFilter(expression).Render();
            //c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e"
            //if (expression.NodeType == ExpressionType.Equal)
            //{
            //    return GetEqual((BinaryExpression)expression);
            //}
            //p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e")
            //if (expression.NodeType == ExpressionType.Call)
            //{
            //    return GetMethod((MethodCallExpression)expression);
            //}
            //p.Categories
            //if (expression.NodeType == ExpressionType.MemberAccess)
            //{
            //    return GetMember((MemberExpression)expression);
            //}

            //c
            //lambda expression parameter does not need to be returned
            //if (expression.NodeType == ExpressionType.Parameter)
            //{
            //    return string.Empty;
            //}
            //TODO Move message to a resource file
            //throw new NotSupportedException("The expression type is not supported.");
        }

        //private FilterVisitor GetGroup(BinaryExpression expression)
        //{
        //    GroupExpressionParser groupExpressionParser = new GroupExpressionParser();
        //    List<Expression> expressions = groupExpressionParser.FlattenExpressions(expression);
        //    GroupFilterVisitorFactory groupFilterFactory = new GroupFilterVisitorFactory();
        //    FilterVisitor filterVisitor = groupFilterFactory.GetGroupFilterVisitor(expressions);
        //    return filterVisitor;
        //}

        //private string RenderParents(List<string> parents)
        //{
        //    return string.Join('.', parents);
        //}        

        //private string GetEqual(BinaryExpression expression)
        //{
        //    string left = string.Empty;
        //    string right = string.Empty;
        //    if (expression.Left.NodeType == ExpressionType.MemberAccess)
        //    {
        //        left = ((MemberExpression)expression.Left).Member.Name;
        //    }
        //    if (expression.Right.NodeType == ExpressionType.Constant)
        //    {
        //        right = expression.Right.ToString();
        //    }
        //    var parent = ((MemberExpression)expression.Left).Expression;
        //    var parentPath = Render(parent);
        //    if (string.IsNullOrEmpty(parentPath))
        //    {
        //        return $"{left.ToCamelCase()}:{right}";
        //    }
        //    else
        //    {
        //        return parentPath + "." + $"{left.ToCamelCase()}:{right}";
        //    }
        //}

        //private string GetMethod(MethodCallExpression expression)
        //{
        //    // p => p.Categories.Any(c => c.Id == "34940e9b-0752-4ffa-8e6e-4f2417995a3e")
        //    if (expression.Method.Name == "Any")
        //    {
        //        return Render(expression.Arguments[0]) + "." + Render(expression.Arguments[1]);
        //    }
        //    // p => p.Categories.Missing()
        //    if (this.extensionMethodMapping.ContainsKey(expression.Method.Name))
        //    {
        //        return Render(expression.Arguments[0]) + ":" + this.extensionMethodMapping[expression.Method.Name];
        //    }
        //    // c => c.Id.Subtree("34940e9b-0752-4ffa-8e6e-4f2417995a3e")
        //    if (expression.Method.Name == "Subtree")
        //    {
        //        SubtreeFilterVisitor subtreeFilterVisitor = new SubtreeFilterVisitor(expression);
        //        return subtreeFilterVisitor.Render();
        //    }
        //    // v => v.Price.Value.CentAmount.Range(1, 30)
        //    if (expression.Method.Name == "Range")
        //    {
        //        RangeGroupFilterVisitor rangeGroupFilterVisitor = new RangeGroupFilterVisitor(new List<MethodCallExpression>() { expression });
        //        return rangeGroupFilterVisitor.Render();
        //    }
        //    // TODO Move message to a resource file
        //    throw new NotSupportedException("The expression type is not supported.");
        //}

        //private string GetMember(MemberExpression expression)
        //{
        //    var parent = expression.Expression;
        //    var parentPath = Render(parent);
        //    var memberName = string.Empty;
        //    if (expression.Member.Name != "Value")
        //    {
        //        memberName = expression.Member.Name.ToCamelCase();
        //    }
        //    if (string.IsNullOrEmpty(parentPath))
        //    {
        //        return memberName;
        //    }
        //    else
        //    {
        //        if (memberName != string.Empty)
        //        { 
        //            return parentPath + "." + memberName;
        //        }
        //        return parentPath;
        //    }
        //}
    }
}
