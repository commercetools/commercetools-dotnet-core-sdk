using System.ComponentModel;

namespace commercetools.Sdk.Domain.Query
{
    public enum SortDirection
    {
        /// <summary>
        /// When the sort direction is ascending, the minimum value is used.
        /// </summary>
        [Description("asc")]
        Ascending,

        /// <summary>
        /// When the sort direction is ascending, the minimum value is used.
        /// </summary>
        [Description("desc")]
        Descending,

        /// <summary>
        /// Changes the default behaviour of the ascending sort by using the maximum value instead.
        /// </summary>
        [Description("asc.max")]
        AscendingWithMaxValue,

        /// <summary>
        /// Changes the default behaviour of the descending sort by using the minimum value instead.
        /// </summary>
        [Description("desc.min")]
        DescendingWithMinValue
    }
}
