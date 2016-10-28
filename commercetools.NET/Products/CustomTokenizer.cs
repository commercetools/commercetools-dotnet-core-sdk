using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.Products
{
    /// <summary>
    /// CustomTokenizer
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-products.html#suggesttokenizer"/>
    public class CustomTokenizer : SuggestTokenizer
    {
        #region Properties

        [JsonProperty(PropertyName = "inputs")]
        public List<string> Inputs { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomTokenizer(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            this.Inputs = Helper.GetListFromJsonArray<string>(data.inputs);
        }

        #endregion
    }
}