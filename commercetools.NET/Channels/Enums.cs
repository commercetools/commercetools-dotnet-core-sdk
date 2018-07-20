
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace commercetools.Channels
{
    /// <summary>
    /// ChannelRoleEnum enumeration.
    /// </summary>
    /// <see href="https://docs.commercetools.com/http-api-projects-channels.html#channelroleenum"/>

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChannelRoleEnum
    {
        InventorySupply,
        ProductDistribution,
        OrderExport,
        OrderImport,
        Primary
    }
}
