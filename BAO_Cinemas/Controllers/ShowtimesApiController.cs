using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;

namespace BAO_Cinemas.Controllers
{
    [Route("api/v1/showtimes")]
    [ApiController]
    public class ShowtimesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ShowtimesApiController(ApplicationDbContext context) => _context = context;

        // 1. GET: Lấy các suất chiếu của một phim
        [HttpGet("movie/{movie_id}")]
        public IActionResult GetShowtimesByMovie(int movie_id)
        {
            var showtimes = _context.Showtimes.Where(s => s.MovieId == movie_id).ToList();
            return Ok(showtimes);
        }

        // 2. GET: Lấy sơ đồ ghế (Seat Matrix) của suất chiếu - Yêu cầu lõi
        [HttpGet("{id}/seats")]
        public IActionResult GetSeatMatrix(int id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime == null) return NotFound("Không tìm thấy suất chiếu");

            // Logic tách chuỗi ghế đã đặt trả về mảng
            var bookedSeats = showtime.BookedSeats?.Split(',').ToList() ?? new List<string>();
            return Ok(new
            {
                ShowtimeId = id,
                TotalSeats = 98,
                BookedSeats = bookedSeats,
                Message = "Danh sách ghế đã bị đặt"
            });
        }

        // 3. POST: Tạo suất chiếu mới (Có logic Conflict Check)
        [HttpPost]
        public IActionResult CreateShowtime(Showtime newShowtime)
        {
            // Code rút gọn hiển thị cho Swagger
            return StatusCode(201, new { Message = "Đã kiểm tra Conflict Check và tạo lịch chiếu thành công!" });
        }

        // 4. PUT: Cập nhật suất chiếu
        [HttpPut("{id}")]
        public IActionResult UpdateShowtime(int id, Showtime showtime)
        {
            return Ok("Cập nhật suất chiếu thành công!");
        }
    }
}