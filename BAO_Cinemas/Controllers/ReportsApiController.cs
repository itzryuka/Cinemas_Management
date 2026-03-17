using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;

namespace BAO_Cinemas.Controllers
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReportsApiController(ApplicationDbContext context) => _context = context;

        // 1. GET: Báo cáo doanh thu theo tháng
        [HttpGet("revenue/month/{month}")]
        public IActionResult GetRevenueByMonth(int month)
        {
            // Tạm thời trả về data mẫu để Swagger hiện ra đẹp
            return Ok(new { Month = month, TotalRevenue = 50000000, Message = "Thống kê doanh thu tháng" });
        }

        // 2. GET: Top phim bán chạy
        [HttpGet("movies/top-selling")]
        public IActionResult GetTopSellingMovies()
        {
            return Ok(new { Message = "Danh sách Top 5 phim bán chạy nhất" });
        }

        // 3. GET: Lưu lượng rạp
        [HttpGet("cinemas/traffic")]
        public IActionResult GetCinemaTraffic()
        {
            return Ok(new { Message = "Báo cáo lưu lượng khách các rạp" });
        }
    }
}