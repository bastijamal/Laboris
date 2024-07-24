using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Laboris.DAL;
using Laboris.Models;
using Laboris.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Laboris.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public WishlistController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Wishlist()
        {
            List<WishlistItemVm> wishlistItems = new List<WishlistItemVm>();

            if (User.Identity.IsAuthenticated)
            {
                User? user = await _userManager.Users
                    .Include(u => u.wishlistItems)
                    .ThenInclude(bi => bi.Product)
                    .ThenInclude(p => p.ProductImages)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

                foreach (WishlistItem item in user.wishlistItems)
                {
                    wishlistItems.Add(new WishlistItemVm
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = (int)item.Product.Price,
                        Color = item.Product.Color,
                        PhotoUrl = item.Product.PhotoUrl
                    });
                }
            }
            else
            {
                var jsoncookie = Request.Cookies["Wishlist"];

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

                        wishlistItems.Add(new WishlistItemVm()
                        {
                            Id = item.Id,
                            Color = product.Color,
                            Size = product.Size,
                            Price = (int)product.Price,
                            Name = product.Name,
                            PhotoUrl = product.PhotoUrl
                        });
                    }

                    Response.Cookies.Append("Wishlist", JsonConvert.SerializeObject(cookieItems));
                }

                return View(wishlistItems);
            }

            return View(wishlistItems);
        }

        public async Task<IActionResult> AddWishlist(int id)
        {
            if (id <= 0) return View("Error");

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.Users
                    .Include(u => u.wishlistItems)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (user == null) return NotFound();

                WishlistItem item = user.wishlistItems.FirstOrDefault(b => b.ProductId == id);

                if (item == null)
                {
                    item = new WishlistItem
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Price = (int)product.Price
                    };

                    await _context.WishlistItems.AddAsync(item);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                var json = Request.Cookies["Wishlist"];
                List<CookieItem> wishlist = json != null ? JsonConvert.DeserializeObject<List<CookieItem>>(json) : new List<CookieItem>();

                if (!wishlist.Any(p => p.Id == id))
                {
                    wishlist.Add(new CookieItem
                    {
                        Id = id,
                        Color = product.Color,
                        Size = product.Size
                    });

                    var cookieWishlist = JsonConvert.SerializeObject(wishlist);
                    Response.Cookies.Append("Wishlist", cookieWishlist);
                }
            }

            return RedirectToAction("Wishlist");
        }

        public async Task<IActionResult> GetWishlist()
        {
            var wishlistCookieJson = Request.Cookies["Wishlist"];
            return Content(wishlistCookieJson);
        }

        public async Task<IActionResult> RemoveItem(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.Users
                    .Include(u => u.wishlistItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return NotFound("User not found.");

                var item = user.wishlistItems.FirstOrDefault(b => b.ProductId == id);

                if (item != null)
                {
                    user.wishlistItems.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound("Product not found.");
                }
            }
            else
            {
                var json = Request.Cookies["Wishlist"];

                if (json != null)
                {
                    List<WishlistCookieItem> wishlist = JsonConvert.DeserializeObject<List<WishlistCookieItem>>(json);

                    var existProduct = wishlist.FirstOrDefault(p => p.Id == id);

                    if (existProduct != null)
                    {
                        wishlist.Remove(existProduct);

                        var cookieWishlist = JsonConvert.SerializeObject(wishlist);
                        Response.Cookies.Append("Wishlist", cookieWishlist, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddMinutes(30)
                        });
                    }
                    else
                    {
                        return NotFound("Product not found in cookie.");
                    }
                }
                else
                {
                    return NotFound("Wishlist cookie not found.");
                }
            }

            return RedirectToAction("Wishlist");
        }

    }
}
