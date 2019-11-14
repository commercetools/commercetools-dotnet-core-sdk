using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using commercetools.Sdk.MvcExample.Models;

namespace commercetools.Sdk.MvcExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClient client;
        
        public HomeController(IClient client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            //Get All Products
            var productsQueryResult = await client.ExecuteAsync(new QueryCommand<Product>());
            var products = productsQueryResult.Results;
            return View(products);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}