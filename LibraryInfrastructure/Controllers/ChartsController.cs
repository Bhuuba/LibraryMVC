using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByYearResponseItem(string Year, int Count);

        private readonly DblibraryContext _db;

        public ChartsController(DblibraryContext db)
        {
            _db = db;
        }

        [HttpGet("countPostsByYear")]
        public async Task<JsonResult> GetCountPostsByYearAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _db
                .Posts
                .GroupBy(post => post.CreateAt.Year)  // Залежить від вашого поля
                .Select(g => new CountByYearResponseItem(
                    g.Key.ToString(),
                    g.Count()
                ))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
        // Приклад у тому ж ChartsController
        [HttpGet("countCommentsByYear")]
        public async Task<IActionResult> GetCountCommentsByYearAsync(CancellationToken cancellationToken)
        {
            var result = await _db.Coments
                .GroupBy(c => c.CreateAt.Year)
                .Select(g => new {
                    year = g.Key.ToString(),
                    count = g.Count()
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(result);
        }


    }
}
