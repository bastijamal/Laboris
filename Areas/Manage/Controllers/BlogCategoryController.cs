
using Laboris.DAL;
using Laboris.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


namespace Laboris.Areas.Manage.Controllers;
[Area("Manage")]

public class BlogCategoryController : Controller
{
    private readonly AppDbContext _context;

    public BlogCategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.BlogCategories.ToListAsync();

        return View(categories);
    }



    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(BlogCategory blogCategory)
    {

        if (!ModelState.IsValid) return View(blogCategory);
       

        await _context.BlogCategories.AddAsync(blogCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }



    public IActionResult Update(int id)
    {
        var blogCategory = _context.BlogCategories.FirstOrDefault(s => s.Id == id);
        if (blogCategory == null)
        {
            return View();
        }

        return View(blogCategory);
    }


    [HttpPost]
    public IActionResult Update(BlogCategory newblogcategory)
    {
        var oldblogcategory = _context.BlogCategories.FirstOrDefault(x => x.Id == newblogcategory.Id);
        if (oldblogcategory == null)
        {
            return View();
        }

        oldblogcategory.Name = newblogcategory.Name;
       

        _context.SaveChanges();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        var blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (blogCategory == null)
        {
            return NotFound();
        }

        _context.BlogCategories.Remove(blogCategory);
        await _context.SaveChangesAsync(); 

        return RedirectToAction("Index");
    }

}








