using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace BAO_Cinemas.Controllers
{
    public class ManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManagementController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // =================================================================
        // ======================= QUẢN LÝ PHIM ============================
        // =================================================================

        public async Task<IActionResult> Movie()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        [HttpGet]
        public IActionResult CreateMovie() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie(Movie movie, IFormFile PosterFile)
        {
            ModelState.Remove("Showtimes");
            ModelState.Remove("PosterUrl");
            if (ModelState.IsValid)
            {
                if (PosterFile != null && PosterFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PosterFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "posters");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                        await PosterFile.CopyToAsync(fileStream);
                    movie.PosterUrl = "/images/posters/" + fileName;
                }
                else
                {
                    movie.PosterUrl = "/images/posters/default.jpg";
                }
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Movie));
            }
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? PosterFile)
        {
            if (id != movie.Id) return NotFound();
            ModelState.Remove("Showtimes");
            ModelState.Remove("PosterUrl");
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (existingMovie == null) return NotFound();
                    if (PosterFile != null && PosterFile.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PosterFile.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "posters");
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                            await PosterFile.CopyToAsync(fileStream);
                        movie.PosterUrl = "/images/posters/" + fileName;
                    }
                    else
                    {
                        movie.PosterUrl = existingMovie.PosterUrl;
                    }
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Movie));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _context.Movies.Include(m => m.Showtimes).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();
            if (movie.Showtimes != null && movie.Showtimes.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa phim '{movie.Title}' vì đang có {movie.Showtimes.Count} suất chiếu.";
                return RedirectToAction(nameof(Movie));
            }
            try
            {
                if (!string.IsNullOrEmpty(movie.PosterUrl) && movie.PosterUrl != "/images/posters/default.jpg")
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, movie.PosterUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa phim '{movie.Title}' thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi hệ thống khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Movie));
        }

        // =================================================================
        // ======================= QUẢN LÝ RẠP ============================
        // =================================================================

        [HttpGet]
        public async Task<IActionResult> Cinema()
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            return View(cinemas);
        }

        [HttpGet]
        public IActionResult CreateCinema() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCinema(Cinema cinema)
        {
            ModelState.Remove("Rooms");
            if (ModelState.IsValid)
            {
                _context.Cinemas.Add(cinema);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã thêm rạp mới thành công!";
                return RedirectToAction(nameof(Cinema));
            }
            return View(cinema);
        }

        [HttpGet]
        public async Task<IActionResult> EditCinema(int? id)
        {
            if (id == null) return NotFound();
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCinema(int id, Cinema cinema)
        {
            if (id != cinema.Id) return NotFound();
            ModelState.Remove("Rooms");
            if (ModelState.IsValid)
            {
                _context.Update(cinema);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã cập nhật thông tin rạp!";
                return RedirectToAction(nameof(Cinema));
            }
            return View(cinema);
        }

        public async Task<IActionResult> DeleteCinema(int? id)
        {
            if (id == null) return NotFound();
            var cinema = await _context.Cinemas.Include(c => c.Rooms).FirstOrDefaultAsync(c => c.Id == id);
            if (cinema == null) return NotFound();
            if (cinema.Rooms != null && cinema.Rooms.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa rạp '{cinema.Name}' vì đang có {cinema.Rooms.Count} phòng chiếu.";
                return RedirectToAction(nameof(Cinema));
            }
            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa rạp thành công!";
            return RedirectToAction(nameof(Cinema));
        }

        // =================================================================
        // ======================= QUẢN LÝ PHÒNG ===========================
        // =================================================================

        [HttpGet]
        public async Task<IActionResult> RoomList(int? cinemaId)
        {
            if (cinemaId == null) return NotFound("Vui lòng chọn một rạp chiếu.");
            var cinema = await _context.Cinemas.FindAsync(cinemaId);
            if (cinema == null) return NotFound();
            var rooms = await _context.Rooms.Where(r => r.CinemaId == cinemaId).ToListAsync();
            ViewBag.CinemaInfo = cinema;
            return View(rooms);
        }

        [HttpGet]
        public IActionResult CreateRoom(int? cinemaId)
        {
            if (cinemaId == null) return NotFound("Không xác định được rạp để thêm phòng.");
            // FIX: Dùng TotalRows + SeatsPerRow thay vì TotalSeats
            var room = new Room { CinemaId = cinemaId.Value, TotalRows = 7, SeatsPerRow = 14, Status = 1 };
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            ModelState.Remove("Cinema");
            ModelState.Remove("Showtimes");
            ModelState.Remove("Seats");

            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                // FIX: Sau khi tạo phòng, tự động generate Seats cho phòng đó
                await GenerateSeatsForRoom(room.Id, room.TotalRows, room.SeatsPerRow);

                TempData["SuccessMessage"] = $"Đã thêm {room.Name} và tạo {room.TotalRows * room.SeatsPerRow} ghế thành công!";
                return RedirectToAction(nameof(RoomList), new { cinemaId = room.CinemaId });
            }
            return View(room);
        }

        // Helper: Tự động tạo ghế khi thêm phòng mới (thay thế BookedSeats)
        private async Task GenerateSeatsForRoom(int roomId, int totalRows, int seatsPerRow, int vipRows = 2)
        {
            var seats = new List<Seat>();
            int midRow = totalRows / 2;

            for (int row = 1; row <= totalRows; row++)
            {
                char rowLabel = (char)(64 + row); // A, B, C...
                string seatType = (row >= midRow - vipRows / 2 && row <= midRow + vipRows / 2)
                    ? "vip" : "standard";

                for (int col = 1; col <= seatsPerRow; col++)
                {
                    seats.Add(new Seat
                    {
                        RoomId = roomId,
                        SeatCode = $"{rowLabel}{col}",
                        RowLabel = rowLabel,
                        SeatNumber = col,
                        SeatType = seatType,
                        Status = "active"
                    });
                }
            }
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int? id)
        {
            if (id == null) return NotFound();
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(int id, Room room)
        {
            if (id != room.Id) return NotFound();
            ModelState.Remove("Cinema");
            ModelState.Remove("Showtimes");
            ModelState.Remove("Seats");
            if (ModelState.IsValid)
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã cập nhật thông tin phòng chiếu!";
                return RedirectToAction(nameof(RoomList), new { cinemaId = room.CinemaId });
            }
            return View(room);
        }

        public async Task<IActionResult> DeleteRoom(int? id)
        {
            if (id == null) return NotFound();
            var room = await _context.Rooms
                .Include(r => r.Showtimes)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null) return NotFound();

            int currentCinemaId = room.CinemaId;

            if (room.Showtimes != null && room.Showtimes.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa '{room.Name}' vì phòng này đang có {room.Showtimes.Count} suất chiếu.";
                return RedirectToAction(nameof(RoomList), new { cinemaId = currentCinemaId });
            }

            // FIX: Xóa tất cả Seats của phòng trước khi xóa phòng
            var seats = _context.Seats.Where(s => s.RoomId == id);
            _context.Seats.RemoveRange(seats);

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa {room.Name} thành công!";
            return RedirectToAction(nameof(RoomList), new { cinemaId = currentCinemaId });
        }

        // =================================================================
        // ======================= QUẢN LÝ SUẤT CHIẾU ======================
        // =================================================================

        [HttpGet]
        public async Task<IActionResult> Showtime(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewBag.AllMovies = await _context.Movies.OrderByDescending(m => m.Id).ToListAsync();
            ViewBag.AllCinemas = await _context.Cinemas.ToListAsync();

            var moviesQuery = _context.Movies
                .Include(m => m.Showtimes)
                .Where(m => m.Showtimes.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchString));

            var movies = await moviesQuery.OrderByDescending(m => m.Id).ToListAsync();
            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShowtime(Showtime showtime)
        {
            // FIX: Bỏ BookedSeats khỏi ModelState.Remove (không còn tồn tại)
            ModelState.Remove("Movie");
            ModelState.Remove("Room");
            ModelState.Remove("Cinema");
            ModelState.Remove("BookingSeats"); // Thay BookedSeats → BookingSeats
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                if (showtime.EndTime <= showtime.StartTime)
                {
                    TempData["ErrorMessage"] = "Giờ kết thúc phải sau giờ bắt đầu!";
                    return RedirectToAction(nameof(Showtime));
                }

                int cleaningMinutes = 15;
                bool isConflict = await _context.Showtimes.AnyAsync(s =>
                    s.RoomId == showtime.RoomId &&
                    showtime.StartTime < s.EndTime.AddMinutes(cleaningMinutes) &&
                    showtime.EndTime.AddMinutes(cleaningMinutes) > s.StartTime
                );

                if (isConflict)
                {
                    TempData["ErrorMessage"] = $"Lỗi Xung Đột: Phòng này đã có lịch chiếu khác (đã tính {cleaningMinutes}p dọn dẹp)!";
                    return RedirectToAction(nameof(Showtime));
                }

                // FIX: Không còn set BookedSeats = "" — chỉ set Status mới
                showtime.Status = "scheduled";
                _context.Showtimes.Add(showtime);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xếp lịch chiếu thành công!";
            }
            return RedirectToAction(nameof(Showtime));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomsByCinema(int cinemaId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.CinemaId == cinemaId && r.Status == 1)
                .Select(r => new { id = r.Id, name = r.Name })
                .ToListAsync();
            return Json(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> GetBusySchedules(int roomId, string? date)
        {
            var query = _context.Showtimes
                .Include(s => s.Movie)
                .Where(s => s.RoomId == roomId);

            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime selectedDate))
                query = query.Where(s => s.StartTime.Date == selectedDate.Date);
            else
                query = query.Where(s => s.StartTime.Date >= DateTime.Today);

            var schedules = await query
                .OrderBy(s => s.StartTime)
                .Select(s => new {
                    title = s.Movie.Title,
                    date = s.StartTime.ToString("dd/MM/yyyy"),
                    start = s.StartTime.ToString("HH:mm"),
                    end = s.EndTime.AddMinutes(15).ToString("HH:mm")
                }).ToListAsync();

            return Json(schedules);
        }

        [HttpGet]
        public async Task<IActionResult> ShowtimeList(int? movieId)
        {
            if (movieId == null) return NotFound();
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            var showtimes = await _context.Showtimes
                .Include(s => s.Cinema)
                .Include(s => s.Room)
                // FIX: Include BookingSeats để đếm số ghế đã đặt (thay BookedSeats string)
                .Include(s => s.BookingSeats)
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            ViewBag.MovieInfo = movie;
            return View(showtimes);
        }

        public async Task<IActionResult> DeleteShowtime(int? id)
        {
            if (id == null) return NotFound();
            var showtime = await _context.Showtimes
                // FIX: Include BookingSeats để kiểm tra thay vì BookedSeats string
                .Include(s => s.BookingSeats)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (showtime == null) return NotFound();

            int currentMovieId = showtime.MovieId;

            // FIX: Kiểm tra có vé đã đặt qua BookingSeats thay vì !string.IsNullOrEmpty(BookedSeats)
            bool hasBookings = showtime.BookingSeats != null &&
                               showtime.BookingSeats.Any(bs => bs.Status == "confirmed");
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Không thể hủy suất chiếu này vì đã có khách hàng đặt vé!";
                return RedirectToAction(nameof(ShowtimeList), new { movieId = currentMovieId });
            }

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã hủy suất chiếu thành công!";
            return RedirectToAction(nameof(ShowtimeList), new { movieId = currentMovieId });
        }

        public async Task<IActionResult> DeleteAllShowtimes(int? movieId)
        {
            if (movieId == null) return NotFound();
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            // FIX: Include BookingSeats để kiểm tra trước khi xóa hàng loạt
            var showtimes = await _context.Showtimes
                .Include(s => s.BookingSeats)
                .Where(s => s.MovieId == movieId)
                .ToListAsync();

            if (showtimes.Any())
            {
                bool anyHasBooking = showtimes.Any(s =>
                    s.BookingSeats != null && s.BookingSeats.Any(bs => bs.Status == "confirmed"));

                if (anyHasBooking)
                {
                    TempData["ErrorMessage"] = $"Không thể hủy toàn bộ lịch chiếu vì có suất chiếu đã bán vé. Vui lòng hủy từng suất!";
                    return RedirectToAction(nameof(Showtime));
                }

                _context.Showtimes.RemoveRange(showtimes);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã hủy toàn bộ lịch công chiếu của phim '{movie.Title}'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Phim này hiện chưa có lịch chiếu nào để xóa.";
            }
            return RedirectToAction(nameof(Showtime));
        }

        // =================================================================
        // ======================= BÁO CÁO DOANH THU =======================
        // =================================================================

        [HttpGet]
        public async Task<IActionResult> RevenueReport(string selectedMonth)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            if (!string.IsNullOrEmpty(selectedMonth))
            {
                var parts = selectedMonth.Split('-');
                if (parts.Length == 2)
                {
                    int.TryParse(parts[0], out year);
                    int.TryParse(parts[1], out month);
                }
            }
            else
            {
                selectedMonth = $"{year}-{month:D2}";
            }

            ViewBag.SelectedMonth = selectedMonth;

            // FIX: Include BookingSeats để đếm vé (thay SelectedSeats.Split(','))
            var bookings = await _context.Bookings
                .Include(b => b.Showtime).ThenInclude(s => s.Movie)
                .Include(b => b.BookingSeats)
                .Where(b => b.BookingTime.Month == month &&
                            b.BookingTime.Year == year &&
                            b.Status == "confirmed")
                .ToListAsync();

            // FIX: Đếm vé = COUNT(BookingSeats confirmed) thay vì SelectedSeats.Split(',').Length
            var movieRevenue = bookings
                .GroupBy(b => b.Showtime.Movie.Title)
                .Select(g => new MovieRevVM
                {
                    MovieName = g.Key,
                    TicketsSold = g.Sum(b => b.BookingSeats.Count(bs => bs.Status == "confirmed")),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderByDescending(m => m.Revenue)
                .ToList();

            var dailyRevenue = bookings
                .GroupBy(b => b.BookingTime.Date)
                .Select(g => new DailyRevVM
                {
                    DateString = g.Key.ToString("dd/MM/yyyy"),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(x => x.DateString)
                .ToList();

            ViewBag.MovieLabels = JsonSerializer.Serialize(movieRevenue.Select(m => m.MovieName));
            ViewBag.MovieData = JsonSerializer.Serialize(movieRevenue.Select(m => m.Revenue));
            ViewBag.DateLabels = JsonSerializer.Serialize(dailyRevenue.Select(d => d.DateString));
            ViewBag.DateData = JsonSerializer.Serialize(dailyRevenue.Select(d => d.Revenue));
            ViewBag.MovieTable = movieRevenue;
            ViewBag.DateTable = dailyRevenue;
            ViewBag.TotalRevenue = bookings.Sum(b => b.TotalPrice);

            return View();
        }
    }

    // ==========================================
    // CÁC CLASS PHỤ TRỢ
    // ==========================================
    public class MovieRevVM
    {
        public string MovieName { get; set; }
        public int TicketsSold { get; set; }
        public double Revenue { get; set; }
    }

    public class DailyRevVM
    {
        public string DateString { get; set; }
        public double Revenue { get; set; }
    }
}