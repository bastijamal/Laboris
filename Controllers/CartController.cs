
using Microsoft.AspNetCore.Mvc;
using Laboris.DAL;
using Laboris.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Laboris.Models;
using System.Security.Claims;

namespace Laboris.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Sepet öğelerini görüntüleme işlemi
        public async Task<IActionResult> Index()
        {
            List<BasketItemVm> basketItems = new List<BasketItemVm>();

            if (User.Identity.IsAuthenticated)
            {
                User? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                foreach (BasketItem item in user.BasketItems)
                {
                    basketItems.Add(new BasketItemVm
                    {
                        Name = item.Product.Name,
                        Price = (int)item.Product.Price,
                        Color = item.Product.Color,
                        Size = item.Product.Size,
                        Count = item.Count,
                        Subtotal = item.Count * (int)item.Product.Price,
                        PhotoUrl = item.Product.PhotoUrl
                    });
                }
            }
            else
            {
                var jsoncookie = Request.Cookies["Basket"];

                if (jsoncookie != null)
                {
                    var cookieItems = JsonConvert.DeserializeObject<List<CookieItem>>(jsoncookie);

                    foreach (var item in cookieItems)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == item.Id);

                        if (product != null)
                        {
                            basketItems.Add(new BasketItemVm()
                            {
                                Id = item.Id,
                                Count = item.Count,
                                Color = product.Color,
                                Size = product.Size,
                                Price = (int)product.Price,
                                Name = product.Name,
                                PhotoUrl = product.PhotoUrl
                            });
                        }
                        else
                        {
                            cookieItems.Remove(item);
                        }
                    }

                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
                }
            }

            return View(basketItems);
        }

        // Sepete ürün ekleme işlemi
        public async Task<IActionResult> AddBasket(int id)
        {
            if (id <= 0) return View("Error");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (user is null) return NotFound();

                var item = user.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item is null)
                {
                    item = new BasketItem
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Price = (int)product.Price,
                        Count = 1,
                    };
                    user.BasketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                List<CookieItem> basket;

                var json = Request.Cookies["Basket"];
                if (json != null)
                {
                    basket = JsonConvert.DeserializeObject<List<CookieItem>>(json);
                    var existProduct = basket.FirstOrDefault(p => p.Id == id);

                    if (existProduct != null)
                    {
                        existProduct.Count++;
                    }
                    else
                    {
                        basket.Add(new CookieItem
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
                new CookieItem
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
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RemoveItem(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var item = user.BasketItems.FirstOrDefault(b => b.ProductId == id);

                if (item != null)
                {
                    user.BasketItems.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound("Product not found in your basket.");
                }
            }
            else
            {
                var json = Request.Cookies["Basket"];

                if (json != null)
                {
                    List<CookieItem> basket = JsonConvert.DeserializeObject<List<CookieItem>>(json);

                    var existProduct = basket.FirstOrDefault(p => p.Id == id);

                    if (existProduct != null)
                    {
                        basket.Remove(existProduct);

                        var cookieBasket = JsonConvert.SerializeObject(basket);
                        Response.Cookies.Append("Basket", cookieBasket, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddMinutes(30)
                        });
                    }
                    else
                    {
                        return NotFound("Product not found in your basket.");
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> ClearCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.BasketItems.Clear();
                await _context.SaveChangesAsync();
            }
            else
            {
                Response.Cookies.Delete("Basket");
            }

            return RedirectToAction(nameof(Index));
        }
    }



}

