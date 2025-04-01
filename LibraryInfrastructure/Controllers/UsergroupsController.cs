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
    public class UsergroupsController : Controller
    {
        private readonly SocialNetworkContext _context;

        public UsergroupsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: Usergroups
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.UserGroups
                .Include(u => u.Group)
                .Include(u => u.User);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Usergroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usergroup = await _context.UserGroups
                .Include(u => u.Group)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usergroup == null)
            {
                return NotFound();
            }

            return View(usergroup);
        }

        // GET: Usergroups/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Usergroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,GroupId")] UserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usergroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", usergroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", usergroup.UserId);
            return View(usergroup);
        }

        // GET: Usergroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usergroup = await _context.UserGroups.FindAsync(id);
            if (usergroup == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", usergroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", usergroup.UserId);
            return View(usergroup);
        }

        // POST: Usergroups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,GroupId")] UserGroup usergroup)
        {
            if (id != usergroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usergroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsergroupExists(usergroup.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", usergroup.GroupId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", usergroup.UserId);
            return View(usergroup);
        }

        // GET: Usergroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usergroup = await _context.UserGroups
                .Include(u => u.Group)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usergroup == null)
            {
                return NotFound();
            }

            return View(usergroup);
        }

        // POST: Usergroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usergroup = await _context.UserGroups.FindAsync(id);
            if (usergroup != null)
            {
                _context.UserGroups.Remove(usergroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ====== НОВИЙ МЕТОД: /Usergroups/Group ======
        [HttpGet]
        public IActionResult Group()
        {
            // Якщо хочете відобразити список usergroup, розкоментуйте:
            var allUserGroups = _context.UserGroups
                .Include(u => u.Group)
                .Include(u => u.User)
                .ToList();
            return View(allUserGroups);

            // Поки що повертаємо порожнє View, щоб не було 404
            return View();
        }

        private bool UsergroupExists(int id)
        {
            return _context.UserGroups.Any(e => e.Id == id);
        }
    }
}
