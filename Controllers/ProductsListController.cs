using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppleShowcase.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AppleShowcase.Controllers
{
    public class ProductsListController : Controller
    {
        private readonly ProductService db;
        public ProductsListController(ProductService context)
        {
            db = context;
        }
        
        public async Task<IActionResult> Mac(FilterViewModel filter)
        {
            var phones = await db.GetProducts("Mac");
            var model = new MacViewModel { Products = phones, Filter = filter };
            return View(model);
        }
        
                
        public async Task<IActionResult> IPhone(FilterViewModel filter)
        {
            var phones = await db.GetProducts("iPhone");
            var model = new IPhoneViewModel { Products = phones, Filter = filter };
            return View(model);
        }
        
        public async Task<IActionResult> IPad(FilterViewModel filter)
        {
            var phones = await db.GetProducts("iPad");
            var model = new IPadViewModel {Products = phones, Filter = filter};
            return View(model);
        }
        
        public async Task<IActionResult> Watch(FilterViewModel filter)
        {
            var phones = await db.GetProducts("Watch");
            var model = new WatchViewModel {Products = phones, Filter = filter};
            return View(model);
        }
        
        public async Task<IActionResult> Pods(FilterViewModel filter)
        {
            var phones = await db.GetProducts("Pods");
            var model = new PodsViewModel {Products = phones, Filter = filter};
            return View(model);
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