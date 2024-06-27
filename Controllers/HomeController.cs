using System.Diagnostics;
using Laboris.DAL;
using Laboris.Models;
using Laboris.Services;
using Laboris.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Laboris.Controllers;

[AutoValidateAntiforgeryToken]

public class HomeController : Controller
{


    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly LayoutService _layoutService;

    public HomeController(AppDbContext dbContext, UserManager<User> userManager, LayoutService layoutService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _layoutService = layoutService;
    }

    public async Task<IActionResult> Index()
    {
        ///COOKIE

        //Response.Cookies.Append("Name", "Basti", new CookieOptions()
        //{
        //    MaxAge = TimeSpan.FromSeconds(10)
        //});


        


        var products = await _dbContext.Products
                                 .Include(p => p.ProductImages)
                                 .ToListAsync();

        if (products == null)
        {
            return View(new List<Products>());
        }

        return View(products);



    }


    public IActionResult Cart()
    {

        string cookie = Request.Cookies["Name"];
        if (cookie == null) return NotFound();

        HttpContext.Session.SetString("Name", "Basti");

        return View();
    }



    public async Task<IActionResult> Blog(string? search, int? categoryId)
    {
        IQueryable<Blog> query = _dbContext.Blogs.Include(b => b.Category).AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Header.ToLower().Contains(search.ToLower()));
        }

        if (categoryId != null)
        {
            query = query.Where(b => b.CategoryId == categoryId);
        }

        var blogVm = new BlogVm
        {
            Categories = await _dbContext.BlogCategories.ToListAsync(),
            Blogs = await query.ToListAsync(),
            CategoryId = categoryId,
            Search = search
        };

        return View(blogVm);
    }


    ////////////////////////////////////////////////////////////


    public async Task<IActionResult> AllProducts(string? search, int? order, int? categoryId)
    {
        IQueryable<Products> query = _dbContext.Products.Include(p => p.ProductImages).AsQueryable();

        switch (order)
        {
            case 1:
                query = query.OrderBy(p => p.Name);
                break;

            case 2:
                query = query.OrderBy(p => p.Price);
                break;


            case 3:
                query = query.OrderByDescending(p => p.Id);
                break;

        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
        }

        if (categoryId != null)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }


        ShopVm shopVm = new ShopVm()
        {
            Categories = await _dbContext.ProductsCategories.Include(c => c.Products).ToListAsync(),
            Products = await query.ToListAsync(),
            CategoryId = categoryId,
            Search = search,
            Order = order,

        };

        return View(shopVm);
    }




    ///

    [Authorize]
    public async Task<IActionResult> CheckOut()
    {
        User user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound();

        OrderVm orderVm = new()
        {
            EmailAdress = user.Email,
            LastName = user.LastName,
            FirstName = user.FirstName
        };

        //ViewBag.BasketItems = await _dbContext.basketItems.Where(b => b.UserId == user.Id).ToListAsync();

        var basketItems = await _dbContext.BasketItems
                                    .Include(b => b.Product)
                                    .Where(b => b.UserId == user.Id)
                                    .ToListAsync();

        ViewBag.BasketItems = basketItems;


        ///Muveggeti countu duzeltmek uchun
        foreach (var item in basketItems)
        {
            if (item.Count == 0)
            {
                item.Count = 1;
            }
        }


        return View(orderVm);
    }




    [HttpPost]
    public async Task<IActionResult> CheckOut(OrderVm orderVm)
    {

        User user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound();

        List<BasketItem> items = await _dbContext.BasketItems.Where(b => b.UserId == user.Id).ToListAsync();

        if (!ModelState.IsValid)
        {
            ViewBag.BasketItems = items;
            return View();
        }


        double total = 0;

        for (int i = 0; i < items.Count; i++)
        {
            total += items[i].Count * items[i].Product.Price;
        }


        Order order = new Order()
        {
            UserId = user.Id,
            Status = null,
            basketItems = items,
            PurchaseAt = DateTime.Now,
            TotalPrice = total,
            Adress = orderVm.AdressDistrc + orderVm.AdressStreet,
            Postcode = orderVm.Postcode

        };

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();



        return View();
    }

    ///







    ///
    public IActionResult Error(string error)
    {
        error = "You have error!";

        return View(model: error);

    }



    public IActionResult About()
    {

        return View(_dbContext.Customers.ToList());

    }

    public IActionResult Contact()
    {
        return View();
    }



    public IActionResult Wishlist()
    {
        return View();
    }



}

