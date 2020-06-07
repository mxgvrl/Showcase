using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppleShowcase.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
 
namespace AppleShowcase.Controllers
{
    [Authorize(Policy ="OnlyForAdmin")]
    public class AdminPageController : Controller
    {
        private readonly ProductService db;
        public AdminPageController(ProductService context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(FilterViewModel filter)
        {
            var phones = await db.GetProducts(filter.Name);
            var model = new IndexViewModel { Products = phones, Filter = filter };
            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
 
        public IActionResult Create()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product p)
        {
            if (!ModelState.IsValid) return View(p);
            await db.Create(p);
            return RedirectToAction("Index");
            // ReSharper disable once Mvc.ViewNotResolved
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            Product p = await db.GetProduct(id);
            if (p == null)
                return NotFound();
            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                await db.Update(p);
                return RedirectToAction("Index");
            }
            return View(p);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await db.Remove(id);
            return RedirectToAction("Index");
        }
 
        public async Task<ActionResult> AttachImage(string id)
        {
            Product p = await db.GetProduct(id);
            if (p == null)
                return NotFound();
            return View(p);
        }
        [HttpPost]
        public async Task<ActionResult> AttachImage(string id, IFormFile uploadedFile)
        {
            if(uploadedFile!=null)
            {
                await db.StoreImage(id, uploadedFile.OpenReadStream(), uploadedFile.FileName);
            }
            return RedirectToAction("Index");
        }
 
        public async Task<ActionResult> GetImage(string id)
        {
            var image = await db.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image, "image/png");
        }
    }
}