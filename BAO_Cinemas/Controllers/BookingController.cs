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
                .Include(s => s.Movie).Include(s => s.Cinema).Include(s => s.Room)
                .FirstOrDefault(s => s.Id == id);
            if (showtime == null) return NotFound();

            // Lấy danh sách ghế đã đặt từ BookingSeats thay vì BookedSeats string
            var bookedSeatCodes = _context.BookingSeats
                .Where(bs => bs.ShowtimeId == id && bs.Status == "confirmed")
                .Select(bs => bs.Seat.SeatCode)
                .ToList();

            ViewBag.BookedSeatCodes = bookedSeatCodes;
            return View(showtime);
        }

        // =========================================================
        // NHẬN DỮ LIỆU ĐẶT VÉ VÀ LƯU VÀO DATABASE (LUỒNG SIÊU TỐC)
        // =========================================================
        [HttpPost]
        public IActionResult ConfirmBooking(int showtimeId, string selectedSeats)
        {
            if (string.IsNullOrEmpty(selectedSeats))
                return RedirectToAction("SeatSelection", new { id = showtimeId });

            var showtime = _context.Showtimes.Include(s => s.Room).FirstOrDefault(s => s.Id == showtimeId);
            if (showtime == null) return NotFound();

            var requestedSeatCodes = selectedSeats.Split(',').Select(s => s.Trim()).ToList();

            // 1. Lấy SeatId từ SeatCode (tra bảng Seats)
            var seats = _context.Seats
                .Where(s => s.RoomId == showtime.RoomId && requestedSeatCodes.Contains(s.SeatCode) && s.Status == "active")
                .ToList();

            if (seats.Count != requestedSeatCodes.Count)
                return Content("Một hoặc nhiều ghế không hợp lệ!");

            // 2. Kiểm tra xung đột (dùng DB thay vì cắt chuỗi)
            var seatIds = seats.Select(s => s.Id).ToList();
            var conflicted = _context.BookingSeats
                .Where(bs => bs.ShowtimeId == showtimeId
                          && bs.Status == "confirmed"
                          && seatIds.Contains(bs.SeatId))
                .Select(bs => bs.Seat.SeatCode)
                .ToList();

            if (conflicted.Any())
                return Content($"Rất tiếc, ghế {string.Join(", ", conflicted)} đã có người đặt!");

            // 3. Tạo Booking
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booking = new Booking
            {
                ShowtimeId = showtimeId,
                UserId = userId,
                TotalPrice = seats.Count * showtime.Price,
                BookingTime = DateTime.Now,
                Status = "confirmed"
            };
            _context.Bookings.Add(booking);
            _context.SaveChanges(); // SaveChanges lần 1 để có BookingId

            // 4. Insert BookingSeats (mỗi ghế = 1 dòng)
            var bookingSeats = seats.Select(seat => new BookingSeat
            {
                BookingId = booking.Id,
                SeatId = seat.Id,
                ShowtimeId = showtimeId,
                Status = "confirmed"
            }).ToList();

            _context.BookingSeats.AddRange(bookingSeats);
            _context.SaveChanges();

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