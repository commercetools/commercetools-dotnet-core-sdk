using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace commercetools.Sdk.MultipleClients.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IClient> clients;

        public HomeController(IEnumerable<IClient> clients)
        {
            this.clients = clients;
        }

        public IActionResult Index()
        {
            IClient client = this.clients.FirstOrDefault();
            PagedQueryResult<Category> category = client.ExecuteAsync(new QueryCommand<Category>()).Result;
            string result = category.Results.Count.ToString();
            return this.Content(result);
        }
    }
}