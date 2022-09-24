using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.HttpApi.CommandBuilders;
using Microsoft.AspNetCore.Mvc;

namespace commercetools.Sdk.PollyExample.Controllers
{
    public class Categories : Controller
    {
        private readonly IClient _AdminClient;
        private readonly IClient _AnonymousClient;

        public Categories(IEnumerable<IClient> clients)
        {
            this._AdminClient = clients.FirstOrDefault(c => c.Name.Equals("AdminClient"));
            this._AnonymousClient = clients.FirstOrDefault(c => c.Name.Equals("AnonymousClient"));
        }
        // GET
        public async Task<IActionResult> Index()
        {
            var categories = await _AnonymousClient.ExecuteAsync(new QueryCommand<Category>());

            //if you want to Debug NotFound Retry Policy enable this
            /*
            var category = await _AdminClient.Builder()
                .Categories()
                .GetById("NotFoundId")
                .ExecuteAsync();
            */
            return View(categories);
        }
    }
}