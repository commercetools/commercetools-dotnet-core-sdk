using commercetools.Sdk.Domain;
using commercetools.Sdk.LinqToQueryPredicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class QueryPredicateTests
    {

        // key = "c14"
        // key != "c14"
        // not(key = "c14")
        // parent(id = "13c4ee51-ff35-490f-8e43-349e39c34646") and parent(typeId = "category")
        // parent(id = "13c4ee51-ff35-490f-8e43-349e39c34646" and typeId = "category")
        // version < 30 or key = "c14"
        // version < 30 and key = "c14"
        // not(version < 30 and key = "c14")
        // key in ("c14", "c15")
        //QueryPredicate<Category> queryPredicate3 = new QueryPredicate<Category>(c => c.Name["en"] == "categoryName");
        //QueryPredicate<Category> queryPredicate2 = new QueryPredicate<Category>(c => c.Key.In("c14", "c15"));
        //QueryPredicate<Category> queryPredicate4 = new QueryPredicate<Category>(c => !c.Key.In("c14", "c15"));

        [Fact]
        public void ExpressionStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Key == "c14";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("key = \"c14\"", result);
        }

        [Fact]
        public void ExpressionPropertyTwoLevelStringEqual()
        {
            Expression<Func<Category, bool>> expression = c => c.Parent.Id == "13c4ee51-ff35-490f-8e43-349e39c34646";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("parent(id = \"13c4ee51-ff35-490f-8e43-349e39c34646\")", result);
        }

        [Fact]
        public void ExpressionPropertyThreeLevelStringEqual()
        {
            Expression<Func<Product, bool>> expression = p => p.MasterData.MasterVariant.Id == "13c4ee51-ff35-490f-8e43-349e39c34646";
            QueryPredicateExpressionVisitor queryPredicateExpressionVisitor = new QueryPredicateExpressionVisitor();
            string result = queryPredicateExpressionVisitor.ProcessExpression(expression);
            Assert.Equal("masterData(masterVariant(id = \"13c4ee51-ff35-490f-8e43-349e39c34646\"))", result);
        }
    }      
}
