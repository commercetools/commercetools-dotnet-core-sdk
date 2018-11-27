using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace commercetools.Sdk.HttpApi.MvcExample.Controllers
{
    public class HomeController : Controller
    {
        private IClient client;

        public HomeController(IClient client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            Category category = client.ExecuteAsync<Category>(new GetByIdCommand<Category>(new Guid("f40fcd15-b1c2-4279-9cfa-f6083e6a2988"))).Result;
            return Content(category.Id);
        }
    }
}