using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;
using LibraryInfrastructure;

namespace LibraryInfrastructure.Controllers
{
    public class ComentsController : Controller
    {
        private readonly SocialNetworkContext _context;

        public ComentsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: Coments
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.Comments.Include(c => c.Post).Include(c => c.User);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Coments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coment == null)
            {
                return NotFound();
            }

            return View(coment);
        }

        // GET: Coments/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Text");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Coments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,PostId,Text,CreateAt")] Comment coment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Text", coment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", coment.UserId);
            return View(coment);
        }

        // GET: Coments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coment = await _context.Comments.FindAsync(id);
            if (coment == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Text", coment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", coment.UserId);
            return View(coment);
        }

        // POST: Coments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PostId,Text,CreateAt")] Comment coment)
        {
            if (id != coment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentExists(coment.Id))
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
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Text", coment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", coment.UserId);
            return View(coment);
        }
        public IActionResult DownloadPDF()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/f.pdf");
            var fileName = "Ви великий молодець.pdf";

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                return NotFound("Файл не знайдено");
            }
        }

        // GET: Coments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coment == null)
            {
                return NotFound();
            }

            return View(coment);
        }

        // POST: Coments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coment = await _context.Comments.FindAsync(id);
            if (coment != null)
            {
                _context.Comments.Remove(coment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
