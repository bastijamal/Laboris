using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboris.DAL;
using Laboris.Models;
using Microsoft.AspNetCore.Mvc;


namespace Laboris.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class CustomersController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CustomersController(AppDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            var customers = _dbContext.Customers.ToList();
            return View(customers);
        }


        public IActionResult Create()
        {
            //elave
            ViewBag.Categories = _dbContext.Blogs.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Customers customers)
        {

            if (!ModelState.IsValid) return View();
            if (!customers.ImgFile.ContentType.Contains("image/")) return View(customers);

            string path = _hostEnvironment.WebRootPath + @"/upload/customers/";

            string fileame = Guid.NewGuid().ToString() + customers.ImgFile.FileName;

            using (FileStream stream = new FileStream(Path.Combine(path, fileame), FileMode.Create))
            {
                await customers.ImgFile.CopyToAsync(stream);
            }

            customers.PhotoUrl = fileame;
            await _dbContext.Customers.AddAsync(customers);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }



        public IActionResult Update(int id)
        {
            var customers = _dbContext.Customers.FirstOrDefault(s => s.Id == id);
            if (customers == null)
            {
                return View();
            }
            return View(customers);
        }


        [HttpPost]
        public IActionResult Update(Customers newcustomers)
        {
            var oldcustomers = _dbContext.Customers.FirstOrDefault(x => x.Id == newcustomers.Id);
            if (oldcustomers == null)
            {
                return View();
            }

            if (newcustomers.ImgFile != null)
            {
                string path = _hostEnvironment.WebRootPath + @"/upload/customers/";
                FileInfo fileInfo = new FileInfo(path + oldcustomers.PhotoUrl);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                string filename = Guid.NewGuid() + newcustomers.ImgFile.FileName;
                using (FileStream stream = new FileStream(path + filename, FileMode.Create))
                {
                    newcustomers.ImgFile.CopyTo(stream);
                }

                oldcustomers.PhotoUrl = filename;
            }

            oldcustomers.Name = newcustomers.Name;
            oldcustomers.Position = newcustomers.Position;
            oldcustomers.Comment = newcustomers.Comment;

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var customers = _dbContext.Customers.FirstOrDefault(x => x.Id == id);
            _dbContext.Customers.Remove(customers);
            _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

