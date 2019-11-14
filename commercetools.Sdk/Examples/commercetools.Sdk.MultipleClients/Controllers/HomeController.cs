using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.Tokens;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.MultipleClients.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IClient> clients;
        private readonly ITokenFlowMapper tokenFlowMapper;

        public HomeController(IEnumerable<IClient> clients, ITokenFlowMapper tokenFlowMapper)
        {
            this.clients = clients;
            this.tokenFlowMapper = tokenFlowMapper;
        }

        public IActionResult Index()
        {
            IClient client1 = this.clients.Where(c => c.Name == "client1").FirstOrDefault();
            IClient client2 = this.clients.Where(c => c.Name == "client2").FirstOrDefault();
            PagedQueryResult<Category> categories1 = client1.ExecuteAsync(new QueryCommand<Category>()).Result;
            PagedQueryResult<Category> categories2 = client2.ExecuteAsync(new QueryCommand<Category>()).Result;
            string result = $"Client {client1.Name} Category count {categories1.Results.Count.ToString()} Flow {tokenFlowMapper.GetTokenFlowRegisterForClient(client1.Name).TokenFlow} \r\n";
            result += $"Client {client2.Name} Category count {categories2.Results.Count.ToString()} Flow {tokenFlowMapper.GetTokenFlowRegisterForClient(client2.Name).TokenFlow}";
            return this.Content(result);
        }
    }
}