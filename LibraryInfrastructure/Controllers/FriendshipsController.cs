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
    public class FriendshipsController : Controller
    {
        private readonly DblibraryContext _context;

        public FriendshipsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Friendships
        public async Task<IActionResult> Index()
        {
            var friendships = _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2);

            return View(await friendships.ToListAsync());
        }

        // GET: Friendships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var friendship = await _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (friendship == null) return NotFound();

            return View(friendship);
        }

        // GET: Friendships/Create
        public IActionResult Create()
        {
            // Список користувачів
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Friendships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("User1Id,User2Id,Status,CreateAt")] Friendship friendship)
        {
            if (ModelState.IsValid)
            {
                // Якщо Status не задано, ставимо "user"
                if (string.IsNullOrEmpty(friendship.Status))
                    friendship.Status = "user";

                // Якщо CreateAt не вказано, ставимо поточну дату
                if (friendship.CreateAt == default)
                    friendship.CreateAt = DateTime.Now;

                _context.Add(friendship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User2Id);
            return View(friendship);
        }

        // GET: Friendships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship == null) return NotFound();

            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User2Id);
            return View(friendship);
        }

        // POST: Friendships/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User1Id,User2Id,Status,CreateAt")] Friendship friendship)
        {
            if (id != friendship.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(friendship.Status))
                        friendship.Status = "user";

                    if (friendship.CreateAt == default)
                        friendship.CreateAt = DateTime.Now;

                    _context.Update(friendship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendshipExists(friendship.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["User1Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User1Id);
            ViewData["User2Id"] = new SelectList(_context.Users, "Id", "Email", friendship.User2Id);
            return View(friendship);
        }

        // GET: Friendships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var friendship = await _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (friendship == null) return NotFound();

            return View(friendship);
        }

        // POST: Friendships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FriendshipExists(int id)
        {
            return _context.Friendships.Any(e => e.Id == id);
        }
    }
}
