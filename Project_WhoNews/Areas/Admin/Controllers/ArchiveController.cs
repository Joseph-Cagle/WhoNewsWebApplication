using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_WhoNews.Data;
using Project_WhoNews.Models;
using Project_WhoNews.Models.Entities;

namespace Project_WhoNews.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArchiveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArchiveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Archive
        public async Task<IActionResult> Index(string SearchString, string sortOrder, string currentFilter, int? pageNumber)
        {

            var articles = from c in _context.archiveDb1
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
            return View(await PaginatedList<ArchiveDb1>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Archive/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archiveDb1 = await _context.archiveDb1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (archiveDb1 == null)
            {
                return NotFound();
            }

            return View(archiveDb1);
        }

        // GET: Admin/Archive/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Archive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ArchiveDate,Author,BodyContent,DatePublished,HeaderContent,Image")] ArchiveDb1 archiveDb1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(archiveDb1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(archiveDb1);
        }

        // GET: Admin/Archive/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archiveDb1 = await _context.archiveDb1.FindAsync(id);
            if (archiveDb1 == null)
            {
                return NotFound();
            }
            return View(archiveDb1);
        }

        // POST: Admin/Archive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ArchiveDate,Author,BodyContent,DatePublished,HeaderContent,Image")] ArchiveDb1 archiveDb1)
        {
            if (id != archiveDb1.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(archiveDb1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArchiveDb1Exists(archiveDb1.Id))
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
            return View(archiveDb1);
        }

        // GET: Admin/Archive/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archiveDb1 = await _context.archiveDb1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (archiveDb1 == null)
            {
                return NotFound();
            }

            return View(archiveDb1);
        }

        // POST: Admin/Archive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var archiveDb1 = await _context.archiveDb1.FindAsync(id);
            _context.archiveDb1.Remove(archiveDb1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArchiveDb1Exists(int id)
        {
            return _context.archiveDb1.Any(e => e.Id == id);
        }
    }
}
