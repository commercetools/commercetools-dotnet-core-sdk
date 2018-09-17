using commercetools.Sdk.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.HttpApi.Tests
{
    public class TestUtils
    {
        public static ClientConfiguration GetClientConfiguration(string clientSettings)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return config.GetSection(clientSettings).Get<ClientConfiguration>();
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static IDictionary<Type, Type> GetRegisteredRequestMessageBuilders()
        {
            IDictionary<Type, Type> registeredRequestMessageBuilders  = new Dictionary<Type, Type>();
            registeredRequestMessageBuilders.Add(typeof(GetByIdCommand), typeof(GetByIdRequestMessageBuilder));
            registeredRequestMessageBuilders.Add(typeof(GetByKeyCommand), typeof(GetByKeyRequestMessageBuilder));
            registeredRequestMessageBuilders.Add(typeof(CreateCommand), typeof(CreateRequestMessageBuilder));
            registeredRequestMessageBuilders.Add(typeof(UpdateByIdCommand), typeof(UpdateByIdRequestMessageBuilder));
            return registeredRequestMessageBuilders;
        }
    }
}