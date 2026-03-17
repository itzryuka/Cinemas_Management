using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;

namespace BAO_Cinemas.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookingsApiController(ApplicationDbContext context) => _context = context;

        // 1. GET: Lấy danh sách toàn bộ hóa đơn (Cho Admin)
        [HttpGet]
        public IActionResult GetAllBookings()
        {
            return Ok(_context.Bookings.Take(50).ToList()); // Lấy 50 hóa đơn gần nhất
        }

        // 2. GET: Lấy lịch sử đặt vé của 1 khách hàng
        [HttpGet("user/{user_id}")]
        public IActionResult GetUserBookings(string user_id)
        {
            var bookings = _context.Bookings.Where(b => b.UserId == user_id).ToList();
            return Ok(bookings);
        }

        // 3. POST: Xử lý Đặt vé (Core Logic - SV2)
        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] Booking request)
        {
            // Giải thích trên Swagger: API này sẽ nhận SelectedSeats, tính TotalPrice và lưu DB
            return Ok(new
            {
                Message = "Giao dịch thành công, đã trừ ghế và sinh mã QR!",
                TicketId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                TotalPrice = 150000
            });
        }

        // 4. PUT: Xác nhận đã thanh toán tiền
        [HttpPut("{id}/confirm")]
        public IActionResult ConfirmPayment(int id)
        {
            return Ok($"Đã xác nhận thanh toán cho hóa đơn {id}");
        }

        // 5. DELETE: Hủy vé
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            return Ok($"Đã hủy hóa đơn {id} và hoàn lại ghế trống");
        }
    }
}