
using Microsoft.AspNetCore.Mvc;
using Laboris.DAL;
using Laboris.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Laboris.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jsoncookie = Request.Cookies["Basket"];
            List<BasketItemVm> basketItems = new List<BasketItemVm>();

            if (jsoncookie != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<CookieItem>>(jsoncookie);

                foreach (var item in cookieItems)
                {
                    var product = _context.Products
                        .Include(p => p.ProductImages)
                        .FirstOrDefault(p => p.Id == item.Id);


                    if (product == null)
                    {
                        cookieItems.Remove(item);
                        continue;
                    }



                    if (product != null)
                    {
                        basketItems.Add(new BasketItemVm()
                        {
                            Id = item.Id,
                            Count = item.Count,
                            Color = product.Color,
                            Size = product.Size,
                            Price=(int)product.Price,
                            Name = product.Name, 
                            PhotoUrl = product.PhotoUrl
                        });

                    }
                }

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
            }

          

            return View(basketItems);
        }


        public IActionResult AddBasket(int id)
        {
            if (id <= 0) return View("Error");

            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            List<CookieItem> basket;

            var json = Request.Cookies["Basket"];

            if (json != null)
            {
                basket = JsonConvert.DeserializeObject<List<CookieItem>>(json);
                var existProduct = basket.FirstOrDefault(p => p.Id == id);

                if (existProduct != null)
                {
                    existProduct.Count += 1;
                }
                else
                {
                    basket.Add(new CookieItem()
                    {
                        Id = id,
                        Count = 1,
                        Color = product.Color,
                        Size = product.Size,
                        
                    });
                }
            }
            else
            {
                basket = new List<CookieItem>
                {
                    new CookieItem()
                    {
                        Id = id,
                        Count = 1,
                        Color = product.Color,
                        Size = product.Size
                    }
                };
            }

            var cookieBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", cookieBasket);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult GetBasket()
        {
            var basketCookieJson = Request.Cookies["Basket"];
            return Content(basketCookieJson);
        }
    }
}
