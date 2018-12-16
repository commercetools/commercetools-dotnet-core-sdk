using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Linq.Filter
{
    public static class FilterMapping
    {
        public static Dictionary<string, string> AllowedMethods { get; } = new Dictionary<string, string>()
        {
            { "Missing", "missing" },
            { "Exists", "exists" }
        };

        public static Dictionary<string, string> AllowedGroupMethods { get; } = new Dictionary<string, string>()
        {
            { "IsOnStockInChannels", "isOnStockInChannels" }
        };

        public static List<string> MembersToSkip { get; } = new List<string>() { "Value" };
    }
}
