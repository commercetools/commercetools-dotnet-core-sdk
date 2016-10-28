using System;
using System.Collections.Generic;

using commercetools.Common;

using Newtonsoft.Json;

namespace commercetools.ProductTypes
{
    /// <summary>
    /// ReferenceType
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-productTypes.html#referencetype"/>
    public class ReferenceType : AttributeType
    {
        #region Properties

        [JsonProperty(PropertyName = "referenceTypeId")]
        public Common.ReferenceType? ReferenceTypeId { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Returns a JSON representation of this instance.
        /// </summary>
        /// <returns>JObject</returns>
        public ReferenceType(dynamic data = null)
            : base((object)data)
        {
            if (data == null)
            {
                return;
            }

            Common.ReferenceType? referenceTypeId;

            this.ReferenceTypeId = Helper.TryGetEnumByEnumMemberAttribute<Common.ReferenceType?>((string)data.referenceTypeId, out referenceTypeId) ? referenceTypeId : null;
        }

        #endregion
    }
}