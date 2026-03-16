using System.Diagnostics;
using BAO_Cinemas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace BAO_Cinemas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Khai báo DbContext

        // Gộp cả Logger và DbContext vào chung một Constructor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Chỉ giữ lại 1 hàm Index để lấy dữ liệu phim
        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ danh sách phim từ Database
            var movies = await _context.Movies
                                       .Include(m => m.Showtimes)
                                            .ThenInclude(s => s.Room)   // Lấy Phòng (để check nhãn IMAX)
                                       .Include(m => m.Showtimes)       // Lệnh này bắt buộc phải lặp lại trước mỗi ThenInclude
                                            .ThenInclude(s => s.Cinema) // Lấy Rạp (để in ra tên rạp lúc gom nhóm nhóm)
                                       .ToListAsync();

            // Truyền danh sách movies này sang cho file Index.cshtml
            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}