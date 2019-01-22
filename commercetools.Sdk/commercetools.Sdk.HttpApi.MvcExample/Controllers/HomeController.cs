using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace commercetools.Sdk.ClientCredentialsExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClient client;

        public HomeController(IClient client)
        {
            this.client = client;
        }

        public IActionResult Index(string id)
        {
            Category category = this.client.ExecuteAsync(new GetByIdCommand<Category>(new Guid(id))).Result;
            return this.Content(category.Id);
        }
    }
}