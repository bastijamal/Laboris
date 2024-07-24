using System.Diagnostics;
using Laboris.Abstractions.MailService;
using Laboris.DAL;
using Laboris.Models;
using Laboris.Services;
using Laboris.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Laboris.Controllers;

[AutoValidateAntiforgeryToken]

public class HomeController : Controller
{

    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly LayoutService _layoutService;
    private readonly IMailService _mailService;

    public HomeController(AppDbContext dbContext, UserManager<User> userManager, LayoutService layoutService, IMailService mailService, IEmailService emailService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _layoutService = layoutService;
        _mailService = mailService;
        _emailService = emailService;
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




    public async Task<IActionResult> Blog(string? search, int? categoryId, int page)
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

        //PAGINATION
        double count = await query.CountAsync();
        ViewBag.TotalPage = Math.Ceiling(count / 2);
        ViewBag.CurrentPage = page + 1;
        List<Blog> blogs = await query.Skip(page * 2).Take(2).ToListAsync();


        var blogVm = new BlogVm
        {
            Categories = await _dbContext.BlogCategories.ToListAsync(),
            //Blogs = await query.ToListAsync(),
            Blogs = blogs,
            CategoryId = categoryId,
            Search = search
        };


        return View(blogVm);
    }





    public async Task<IActionResult> AllProducts(string? search, int? order, int? categoryId, int page, string? color, int? tagId)
    {
        //double count = await _dbContext.Products.CountAsync();
        //ViewBag.TotalPage = Math.Ceiling(count / 12); 
        //ViewBag.CurrentPage = page + 1;


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

        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(p => p.Color.ToLower() == color.ToLower());
        }

        if (tagId != null)
        {
            query = query.Where(p => p.ProductTags.Any(pt => pt.TagId == tagId));
        }

        var tags = await _dbContext.Tags.ToListAsync();


        //PAGINATION
        double count = await query.CountAsync();
        ViewBag.TotalPage = Math.Ceiling(count / 12);
        ViewBag.CurrentPage = page + 1;
        List<Products> products = await query.Skip(page * 12).Take(12).ToListAsync();


        ShopVm shopVm = new ShopVm()
        {
            Categories = await _dbContext.ProductsCategories.Include(c => c.Products).ToListAsync(),
            //Products = await query.ToListAsync(),
            Products=products,
            CategoryId = categoryId,
            Search = search,
            Order = order,
            SelectedColor = color,
            Tags = tags
        };

        return View(shopVm);
    }


    public IActionResult Cart()
    {

        string cookie = Request.Cookies["Name"];
        if (cookie == null) return NotFound();

        HttpContext.Session.SetString("Name", "Basti");

        return View();
    }


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
    [Route("/CheckOut")]
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





    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }


    //[HttpPost]
    //public async Task<IActionResult> Contact(ContactVm vm)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        MailRequestVm request = new();
    //        request.ToEmail = "tu6y6g86w@code.edu.az";
    //        request.Subject = "Contact Detail";
    //        request.Comment

    //        await _mailService.SendEmailAsync();
    //        ViewBag.Message = "Email sent successfully.";
    //        return RedirectToAction("Contact");
    //    }
    //    return View(request);
    //}



    [HttpPost]
    public IActionResult Contact(ContactVm vm)
    {
       

        string htmlCode = @$"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>User Contact Info</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 50px auto;
            background-color: rgb(226, 198, 198);
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #333;
        }}
        .info {{
            margin-bottom: 20px;
        }}
        .info label {{
            font-weight: bold;
        }}
        .info p {{
            margin: 5px 0;
        }}
        .message {{
            margin-bottom: 20px;
        }}
        .message label {{
            font-weight: bold;
        }}
        .message p {{
            margin: 5px 0;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2>User Contact Information</h2>
        <div class=""info"">
            <label for=""name"">Name:</label>
            <p>{vm.Name}</p>
            <label for=""subject"">Subject:</label>
            <p>{vm.Subject}</p>
    <label for=""phone"">Phone:</label>
            <p>{vm.PhoneNumber}</p>
            <label for=""email"">Email:</label>
            <p>{vm.Email}</p>
        </div>
        <div class=""message"">
            <label for=""message"">Message:</label>
            <p>{vm.Message}</p>
        </div>
    </div>
</body>
</html>";


        _emailService.SendEmail(new EmailDto(body: htmlCode, subject: "Contact Info", to: "tu6y6g86w@code.edu.az"));

        return RedirectToAction("Contact");
    }





    public IActionResult Error(string error)
    {
        error = "You have error!";

        return View(model: error);

    }


    public IActionResult About()
    {

        return View(_dbContext.Customers.ToList());

    }


    public IActionResult Wishlist()
    {
        return View();
    }


    public IActionResult Detail(int id)
    {
        var product = _dbContext.Products
            .Include(p => p.ProductTags)
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);

       
        return View(product);
    }


  

}

