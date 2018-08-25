using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    // commerce tools client, do not mistake for HttpClient
    public class Client : IClient
    {
        public string Name { get; set; }
        private IApiClient apiClient; 

        public Category GetCategoryById(int categoryId)
        {
            return this.apiClient.GetCategoryById(categoryId); 
        }

        // TODO An idea is to move the code from ApiClient to here so that we remove one class step that is not needed
        public Client(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
    }
}
