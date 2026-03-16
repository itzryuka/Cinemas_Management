using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // Thêm thư viện này để lấy Claim NameIdentifier

namespace BAO_Cinemas.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // TRANG SƠ ĐỒ CHỌN GHẾ
        // =========================================================
        public IActionResult SeatSelection(int id)
        {
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Include(s => s.Room)
                .FirstOrDefault(s => s.Id == id);

            if (showtime == null)
            {
                return NotFound("Không tìm thấy suất chiếu này!");
            }

            return View(showtime);
        }

        // =========================================================
        // NHẬN DỮ LIỆU ĐẶT VÉ VÀ LƯU VÀO DATABASE (LUỒNG SIÊU TỐC)
        // =========================================================
        [HttpPost]
        // Đã xóa 2 tham số: customerName và customerPhone
        public IActionResult ConfirmBooking(int showtimeId, string selectedSeats)
        {
            if (string.IsNullOrEmpty(selectedSeats))
                return RedirectToAction("SeatSelection", new { id = showtimeId });

            var showtime = _context.Showtimes.FirstOrDefault(s => s.Id == showtimeId);
            if (showtime == null) return NotFound();

            // 1. Kiểm tra trùng ghế (Logic bảo mật)
            var newSeats = selectedSeats.Split(',');
            var currentBooked = string.IsNullOrEmpty(showtime.BookedSeats)
                ? new List<string>()
                : showtime.BookedSeats.Split(',').ToList();

            foreach (var seat in newSeats)
            {
                if (currentBooked.Contains(seat))
                    return Content($"Rất tiếc, ghế {seat} đã có người mua mất rồi!");
            }

            // 2. TẠO HÓA ĐƠN (BOOKING)
            var booking = new Booking
            {
                ShowtimeId = showtimeId,

                // Tự động lấy Email đăng nhập gán vào tên khách hàng
                CustomerName = User.Identity.Name ?? "Khách hàng",

                // Điền cứng "Không yêu cầu" cho SĐT vì đã bỏ ô nhập liệu
                CustomerPhone = "Không yêu cầu",

                // Lấy UserId (nếu bạn đã thêm cột UserId vào bảng Booking như hướng dẫn)
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,

                SelectedSeats = selectedSeats,
                TotalPrice = newSeats.Length * showtime.Price,
                BookingTime = DateTime.Now
            };

            _context.Bookings.Add(booking);

            // 3. CẬP NHẬT GHẾ ĐÃ BÁN TRONG SHOWTIME
            if (string.IsNullOrEmpty(showtime.BookedSeats))
                showtime.BookedSeats = selectedSeats;
            else
                showtime.BookedSeats += "," + selectedSeats;

            _context.SaveChanges();

            // 4. CHUYỂN HƯỚNG SANG TRANG VÉ THÀNH CÔNG
            return RedirectToAction("Success", new { bookingId = booking.Id });
        }

        // =========================================================
        // TRANG HIỂN THỊ VÉ ĐIỆN TỬ
        // =========================================================
        public IActionResult Success(int bookingId)
        {
            var booking = _context.Bookings
                .Include(b => b.Showtime).ThenInclude(s => s.Movie)
                .Include(b => b.Showtime).ThenInclude(s => s.Cinema)
                .Include(b => b.Showtime).ThenInclude(s => s.Room)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null) return NotFound();

            return View(booking);
        }
    }
}