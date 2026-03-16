using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models; // Đảm bảo đúng namespace Models của bạn
// using BAO_Cinemas.Data; // Nếu ApplicationDbContext nằm ở đây thì hãy uncomment

namespace BAO_Cinemas.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject Database vào Controller
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action xử lý trang Kho Phim
        public async Task<IActionResult> MovieStorage()
        {
            // Lấy toàn bộ danh sách phim từ bảng Movies trong Database
            // Bạn có thể dùng .OrderByDescending(m => m.Id) để phim mới hiện lên đầu
            var movies = await _context.Movies.OrderByDescending(m => m.Id).ToListAsync();

            // Gửi danh sách movies sang View (MovieStorage.cshtml)
            return View(movies);
        }
    }
}