using commercetools.Sdk.Linq.Filter;

namespace commercetools.Sdk.Linq.Sort
{
    public class SortPredicateVisitorFactory : PredicateVisitorFactoryBase
    {
        public SortPredicateVisitorFactory()
            : base(GetPredicateVisitorConverters<IFilterPredicateVisitorConverter>())
        {
        }
    }
}
