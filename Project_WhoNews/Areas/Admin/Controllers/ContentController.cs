using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project_WhoNews.Data;
using Project_WhoNews.Models;
using Project_WhoNews.Models.Entities;

namespace Project_WhoNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Archive(int? Id, string SearchString, string sortOrder, string currentFilter, int? pageNumber)
        {

            string connectionString =
                "Server=DESKTOP-9TQMSVV\\SQLEXPRESS;Database=WhoNews!;Integrated Security=True;MultipleActiveResultSets=True";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ArchiveContent";
            cmd.Parameters.Add("@Id", SqlDbType.Int);
            cmd.Parameters["@Id"].Value = Id;
            conn.Open();
            cmd.ExecuteScalar();
            conn.Close();

            var articles = from c in _context.Content
                select c;

            ViewData["articleSort"] = String.IsNullOrEmpty(sortOrder) ? "Platforms_sort" : ""; // later implementation 
            // ViewData["ConsoleSort2"] = sortOrder == "Platforms" ? "plat_sort" : "platsort";
            ViewData["articleFilter"] = SearchString;
            ViewData["CurrentSort"] = sortOrder;


            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                articles = articles.Where(c => c.Title.Contains(SearchString) || c.Author.Contains(SearchString));

            }



            switch (sortOrder)
            {
                case "Platforms_sort":
                    articles = articles.OrderBy(c => c.Author);
                    break;

            }

            int pageSize = 5;

            return View("~/Areas/Admin/Views/Content/Index.cshtml", await PaginatedList<Content>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Content
        public async Task<IActionResult> Index(string SearchString, string sortOrder, string currentFilter, int? pageNumber)
        {

            var articles = from c in _context.Content
                select c;

            ViewData["articleSort"] = String.IsNullOrEmpty(sortOrder) ? "Article_sort" : ""; // later implementation 
            // ViewData["ConsoleSort2"] = sortOrder == "Platforms" ? "plat_sort" : "platsort";
            ViewData["articleFilter"] = SearchString;
            ViewData["CurrentSort"] = sortOrder;



            if (SearchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                articles = articles.Where(c => c.Title.Contains(SearchString) || c.Author.Contains(SearchString));

            }



            switch (sortOrder)
            {
                case "Platforms_sort":
                    articles = articles.OrderBy(c => c.Author);
                    break;

            }
            int pageSize = 5;


            return View(await PaginatedList<Content>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Admin/Content/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Content/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,BodyContent,DatePublished,HeaderContent,Image,ImagePath,IsArchived,CatItemId")] Content content)
        {

             content.IsArchived = false;



             if (ModelState.IsValid)
            {

                if (content.ImagePath != null)
                {
                    string folder = "Images/ArticleImages";
                    folder += Guid.NewGuid().ToString() + content.ImagePath.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    content.Image = "/"+folder;

                    await content.ImagePath.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                }



                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        // GET: Admin/Content/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Admin/Content/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,BodyContent,DatePublished,HeaderContent,Image,IsArchived,CatItemId")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        // GET: Admin/Content/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Admin/Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Content.FindAsync(id);
            _context.Content.Remove(content);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _context.Content.Any(e => e.Id == id);
        }
    }
}
