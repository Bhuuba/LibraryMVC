using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;
using LibraryInfrastructure;

namespace LibraryInfrastructure.Controllers
{
    public class PostsController : Controller
    {
        private readonly SocialNetworkContext _context;

        public PostsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            // Включаємо дані користувача, щоб відображати їх у списку постів
            var posts = await _context.Posts
                .Include(p => p.User)
                .ToListAsync();

            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            // Перенаправляємо до списку коментарів для цього поста:
            return RedirectToAction("Index", "Coments", new { id = post.Id });
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            // Якщо треба обирати користувача з випадаючого списку:
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Text,CreateAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Якщо CreateAt не вказано, встановлюємо поточну дату/час
                if (post.CreatedAt == default)
                {
                    post.CreatedAt = DateTime.Now;
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Якщо модель не валідна – повертаємо форму з випадаючим списком
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Text,CreateAt")] Post post)
        {
            // Вручну прив’язуємо Id, щоб EF знав, що це за запис
            post.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    // Якщо CreateAt не задано – теж ставимо поточну дату (за потреби)
                    if (post.CreatedAt == default)
                    {
                        post.CreatedAt = DateTime.Now;
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Повертаємо форму з випадаючим списком, якщо щось пішло не так
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
