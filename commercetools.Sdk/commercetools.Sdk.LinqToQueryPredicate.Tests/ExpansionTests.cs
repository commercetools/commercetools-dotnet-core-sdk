using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace commercetools.Sdk.LinqToQueryPredicate.Tests
{
    public class ExpansionTests
    {
        //https://api.sphere.io/portablevendor/products/2b327437-702e-4ab2-96fc-a98afa860b37?expand=productType
        //https://api.sphere.io/portablevendor/categories/7b46d573-f9b6-444e-921e-2d2254c136e0?expand=parent
        //https://api.sphere.io/portablevendor/categories/515fbfd1-2768-4696-b46f-a532109a226a?expand=ancestors[*]
        //https://api.sphere.io/portablevendor/categories/515fbfd1-2768-4696-b46f-a532109a226a?expand=ancestors[0]
        //https://api.sphere.io/portablevendor/products/2b327437-702e-4ab2-96fc-a98afa860b37?expand=masterData.current.categories[*]
        //https://api.sphere.io/portablevendor/products/2b327437-702e-4ab2-96fc-a98afa860b37?expand=masterData.current.masterVariant.prices[*].customerGroup
        //https://api.sphere.io/portablevendor/categories/515fbfd1-2768-4696-b46f-a532109a226a?expand=parent.parent

        [Fact]
        public void ExpandCategoryParent()
        {
            Expression<Func<Category, Reference>> expression = c => c.Parent;
            Expression<Func<Category, Reference>> expression21 = c => c.Ancestors.ExpandAll();
            Expression<Func<Category, Reference>> expression2 = c => c.Ancestors[0];
            Expression<Func<Product, Reference>> expression3 = p => p.MasterData.Current.Categories.ExpandAll();
            Expression<Func<Product, Reference>> expression4 = p => p.MasterData.Current.MasterVariant.Prices.ExpandAll().CustomerGroup;
            Expression<Func<Category, Reference>> expression5 = c => c.Parent.Obj.Parent;

            Expression<Func<Product, Domain.Attribute>> expression6 = p => p.MasterData.Current.MasterVariant.Attributes.ExpandAll();
        }
    }      
}
