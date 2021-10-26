using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Client
{
    public class CheckByQueryCommand<T> : CheckCommand<T>
        where T : Resource<T>, ICheckable<T>
    {
        public CheckByQueryCommand()
        {
            Where = new List<string>();
        }

        public List<string> Where { get; private set; }
    }
}