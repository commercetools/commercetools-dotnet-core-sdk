using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Common.Converters
{
    /// <summary>
    /// Custom converter for the LocalizedString class.
    /// </summary>
    public class LocalizedStringConverter : JsonConverter
    {
        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <returns>True if the type is LocalizedString, false otherwise</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(LocalizedString));
        }

        /// <summary>
        /// ReadJson
        /// </summary>
        /// <param name="reader">JsonReader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="serializer">Serializer</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            LocalizedString localizedString = new LocalizedString();

            foreach (var kvp in jObject)
            {
                string key = kvp.Key;
                string value = kvp.Value.ToString();
                localizedString.SetValue(key, value);
            }

            return localizedString;
        }

        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="value">Value</param>
        /// <param name="serializer">JsonSerializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is LocalizedString)
            {
                LocalizedString localizedString = (LocalizedString)value;
                JObject jObject = new JObject();

                foreach (string key in localizedString.Values.Keys)
                {
                    jObject.Add(new JProperty(key, localizedString.Values[key]));
                }

                jObject.WriteTo(writer);
            }
        }
    }
}
