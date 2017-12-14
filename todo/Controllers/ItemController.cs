using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Models;
using Microsoft.Extensions.Options;

namespace todo.Controllers
{
    public class ItemController : Controller
    {
        private readonly CosmosDBRepository<Item> _repository;
        private readonly AppConfiguration _options;

        public ItemController(IOptions<AppConfiguration> optionsAccessor){
            _options = optionsAccessor.Value;
            _repository = new CosmosDBRepository<Item>(_options);
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            var items = await _repository.GetItemsAsync(d => !d.Completed);
            return View(items);
        }

        [ActionName("Create")]
        public IActionResult CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Id,Name,Description,Completed")] Item item)
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] Item item)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Item item = await _repository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Item item = await _repository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _repository.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Item item = await _repository.GetItemAsync(id);
            return View(item);
        }
    }
}
