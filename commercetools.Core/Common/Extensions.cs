using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace commercetools.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Allows easy access to the EnumMember attribute.
        /// </summary>
        /// <param name="val">Enum</param>
        /// <returns></returns>
        public static string ToEnumMemberString(this Enum val)
        {
            EnumMemberAttribute[] attributes = (EnumMemberAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return attributes.Length > 0 ? attributes[0].Value : string.Empty;
        }

        /// <summary>
        /// Gets the JSON representation for an object.
        /// </summary>
        /// <param name="val">Object</param>
        /// <returns>JSON string</returns>
        public static string ToJsonString(this object val)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(val, settings);
        }

        /// <summary>
        /// Gets a query string from a NameValueCollection, with the values 
        /// properly encoded.
        /// </summary>
        /// <param name="val">Values</param>
        /// <returns>A query string, including the ?</returns>
        public static string ToQueryString(this NameValueCollection val)
        {
            if (val == null || val.Count < 1)
            {
                return string.Empty;
            }

            List<String> items = new List<String>();

            foreach (string name in val)
            {
                items.Add(string.Concat(name, "=", Helper.UrlEncode(val[name])));
            }

            return string.Concat("?", string.Join("&", items.ToArray()));
        }
    }
}
