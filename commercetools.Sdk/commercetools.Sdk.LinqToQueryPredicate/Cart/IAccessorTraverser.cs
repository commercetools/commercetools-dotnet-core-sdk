using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq
{
    public interface IAccessorTraverser
    {
        string Render(Expression expression);
    }
}
