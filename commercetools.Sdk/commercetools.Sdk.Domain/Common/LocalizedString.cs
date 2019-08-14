using System;

namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;

    public class LocalizedString : Dictionary<string, string>
    {
        public LocalizedString() : base()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj != null & obj is LocalizedString)
            {
                var localizedString = obj as LocalizedString;
                return this.DictionaryEqual(localizedString);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
