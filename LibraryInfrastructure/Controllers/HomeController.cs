using LibraryDomain.Model;
using LibraryInfrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryInfrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialNetworkContext _context;

        public HomeController(SocialNetworkContext context)
        {
            _context = context;
        }

        // Завантаження постів разом з автором і коментарями (і їх авторами)
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }
    }
}
