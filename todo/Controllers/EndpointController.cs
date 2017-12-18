using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using todo.Models;

namespace todo.Controllers
{
    public class EndpointController : Controller
    {
        private readonly CosmosDBRepository<Item> _repository;
        private readonly AppConfiguration _options;

        public EndpointController(IOptions<AppConfiguration> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _repository = new CosmosDBRepository<Item>(_options);
        }

        // GET: Endpoint
        public async Task<ActionResult> Index()
        {
            // WriteRegion, ReadRegionを取得するために一度ドキュメントにアクセスさせる
            await _repository.GetItemsAsync(d => !d.Completed);

            // WriteRegion, ReadRegionを取得
            var endpoints = _repository.GetEndpoints();
            return View(endpoints);
        }
    }
}