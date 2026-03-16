using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System; // Bổ sung thư viện cho DateTime
using System.Linq; // Bổ sung thư viện cho LINQ

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

        // 1. TRANG QUẢN LÝ PHIM (Sẽ gọi file Movie.cshtml)
        public async Task<IActionResult> Movie()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies); // Tự động tìm file Views/Management/Movie.cshtml
        }
        // ==========================================
        // 1. THÊM PHIM MỚI (CREATE)
        // ==========================================
        [HttpGet]
        public IActionResult CreateMovie()
        {
            return View(); // Mở form trống
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm IFormFile PosterFile vào tham số
        public async Task<IActionResult> CreateMovie(Movie movie, IFormFile PosterFile)
        {
            ModelState.Remove("Showtimes"); // Phim mới chưa có suất chiếu
            ModelState.Remove("PosterUrl"); // Đường dẫn ảnh sẽ do Controller tự tạo
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Admin có chọn file ảnh không
                if (PosterFile != null && PosterFile.Length > 0)
                {
                    // 1. Tạo tên file độc nhất (tránh việc upload 2 ảnh trùng tên bị ghi đè)
                    // Ví dụ: avengers.jpg -> 1234abcd_avengers.jpg
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PosterFile.FileName);

                    // 2. Chỉ định đường dẫn lưu file: wwwroot/images/posters
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "posters");

                    // (Đảm bảo thư mục tồn tại, nếu chưa có thì tự tạo)
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // 3. Copy file ảnh vào thư mục đó
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await PosterFile.CopyToAsync(fileStream);
                    }

                    // 4. Lưu đường dẫn tương đối vào Database
                    movie.PosterUrl = "/images/posters/" + fileName;
                }
                else
                {
                    // Nếu không chọn ảnh, có thể gán một ảnh mặc định
                    movie.PosterUrl = "/images/posters/default.jpg";
                }

                // Lưu dữ liệu vào SQL
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Movie));
            }
            return View(movie);
        }

        // ==========================================
        // 2. SỬA THÔNG TIN PHIM (EDIT)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("Không tìm thấy mã phim.");
            }

            // Tìm phim trong Database dựa theo ID (ví dụ ID = 15)
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound("Phim không tồn tại trong hệ thống.");
            }

            // Mở file Views/Management/Edit.cshtml và nhét dữ liệu phim cũ vào
            return View(movie);
        }

        // ==========================================
        // SỬA THÔNG TIN PHIM (LƯU VÀO DATABASE)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? PosterFile)
        {
            // Bảo mật: Đảm bảo ID trên URL và ID trong form là một
            if (id != movie.Id) return NotFound();

            // 1. THÔNG CHỐT LỖI NGẦM
            ModelState.Remove("Showtimes");
            ModelState.Remove("PosterUrl");

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy lại dữ liệu cũ để không bị mất link ảnh nếu không up ảnh mới
                    var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (existingMovie == null) return NotFound();

                    // Hệ thống sẽ kiểm tra xem Admin có chọn file ảnh mới không?
                    if (PosterFile != null && PosterFile.Length > 0)
                    {
                        // TRƯỜNG HỢP 1: BẠN MUỐN SỬA ẢNH THẬT (Có chọn file mới)
                        // -> Đoạn code này sẽ chạy: Tạo tên mới, lưu file mới vào wwwroot/images/posters, 
                        // và cập nhật đường dẫn mới cho bộ phim.

                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PosterFile.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "posters");
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await PosterFile.CopyToAsync(fileStream);
                        }
                        movie.PosterUrl = "/images/posters/" + fileName; // Ghi đè link ảnh mới
                    }
                    else
                    {
                        // TRƯỜNG HỢP 2: BẠN CHỈ SỬA CHỮ, KHÔNG CHỌN ẢNH MỚI (PosterFile bị null)
                        // -> Đoạn code này sẽ chạy: Lấy lại đúng cái đường dẫn ảnh cũ đắp vào, 
                        // không làm mất ảnh của phim.

                        movie.PosterUrl = existingMovie.PosterUrl;
                    }

                    // 3. CẬP NHẬT VÀ LƯU DATABASE
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Movie)); // Về lại danh sách
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }

            return View(movie);
        }

        // ==========================================
        // 3. XÓA PHIM (DELETE)
        // ==========================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Tìm phim, KÈM THEO danh sách suất chiếu của nó
            var movie = await _context.Movies
                                      .Include(m => m.Showtimes)
                                      .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            // KIỂM TRA RÀNG BUỘC: Nếu phim đang có suất chiếu thì KHÔNG ĐƯỢC XÓA
            if (movie.Showtimes != null && movie.Showtimes.Any())
            {
                // Bắn thông báo lỗi sang trang Danh sách
                TempData["ErrorMessage"] = $"Không thể xóa phim '{movie.Title}' vì đang có {movie.Showtimes.Count} suất chiếu. Vui lòng xóa suất chiếu trước!";
                return RedirectToAction(nameof(Movie));
            }

            try
            {
                // (Tùy chọn) Xóa luôn file ảnh trong thư mục wwwroot cho nhẹ máy
                if (!string.IsNullOrEmpty(movie.PosterUrl) && movie.PosterUrl != "/images/posters/default.jpg")
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, movie.PosterUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Xóa phim khỏi Database
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                // Bắn thông báo thành công
                TempData["SuccessMessage"] = $"Đã xóa phim '{movie.Title}' thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi hệ thống khi xóa: " + ex.Message;
            }

            return RedirectToAction(nameof(Movie));
        }

        // =================================================================
        // ======================= QUẢN LÝ RẠP (CINEMA) ====================
        // =================================================================

        // 1. DANH SÁCH RẠP
        [HttpGet]
        public async Task<IActionResult> Cinema()
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            return View(cinemas);
        }

        // 2. THÊM RẠP MỚI
        [HttpGet]
        public IActionResult CreateCinema()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCinema(Cinema cinema)
        {
            // Bỏ qua lỗi bắt buộc phải có danh sách phòng khi tạo rạp mới
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

        // 3. SỬA RẠP
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

            // Bỏ qua lỗi bắt buộc phải có danh sách phòng khi sửa rạp
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

        // 4. XÓA RẠP
        public async Task<IActionResult> DeleteCinema(int? id)
        {
            if (id == null) return NotFound();

            // Lấy rạp kèm theo danh sách phòng để kiểm tra ràng buộc
            var cinema = await _context.Cinemas
                                       .Include(c => c.Rooms)
                                       .FirstOrDefaultAsync(c => c.Id == id);

            if (cinema == null) return NotFound();

            // Kiểm tra: Nếu rạp đang có phòng chiếu bên trong thì chặn không cho xóa
            if (cinema.Rooms != null && cinema.Rooms.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa rạp '{cinema.Name}' vì đang có {cinema.Rooms.Count} phòng chiếu. Vui lòng xóa hết phòng trước!";
                return RedirectToAction(nameof(Cinema));
            }

            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa rạp thành công!";

            return RedirectToAction(nameof(Cinema));
        }
        //


        /*====================================
         * Mở danh sách phòng
         =====================================*/
        [HttpGet]
        public async Task<IActionResult> RoomList(int? cinemaId)
        {
            if (cinemaId == null) return NotFound("Vui lòng chọn một rạp chiếu.");

            var cinema = await _context.Cinemas.FindAsync(cinemaId);
            if (cinema == null) return NotFound("Không tìm thấy rạp này.");

            var rooms = await _context.Rooms.Where(r => r.CinemaId == cinemaId).ToListAsync();

            ViewBag.CinemaInfo = cinema;
            return View(rooms);
        }
        // =================================================================
        // 2. THÊM PHÒNG MỚI
        // =================================================================
        [HttpGet]
        public IActionResult CreateRoom(int? cinemaId)
        {
            if (cinemaId == null) return NotFound("Không xác định được rạp để thêm phòng.");

            // Tạo sẵn một phòng trống, gán sẵn mã Rạp và mặc định 98 ghế, trạng thái 1 (Hoạt động)
            var room = new Room { CinemaId = cinemaId.Value, TotalSeats = 98, Status = 1 };
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            // Bỏ qua kiểm tra lỗi ngầm của Khóa ngoại
            ModelState.Remove("Cinema");
            ModelState.Remove("Showtimes");

            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã thêm {room.Name} thành công!";

                // Lưu xong thì quay về đúng danh sách phòng của rạp đó
                return RedirectToAction(nameof(RoomList), new { cinemaId = room.CinemaId });
            }
            return View(room);
        }

        // =================================================================
        // 3. SỬA THÔNG TIN PHÒNG
        // =================================================================
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

            if (ModelState.IsValid)
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã cập nhật thông tin phòng chiếu!";

                return RedirectToAction(nameof(RoomList), new { cinemaId = room.CinemaId });
            }
            return View(room);
        }

        // =================================================================
        // 4. XÓA PHÒNG
        // =================================================================
        public async Task<IActionResult> DeleteRoom(int? id)
        {
            if (id == null) return NotFound();

            // Tìm phòng và lấy kèm danh sách suất chiếu để kiểm tra ràng buộc
            var room = await _context.Rooms
                                   .Include(r => r.Showtimes)
                                   .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null) return NotFound();

            int currentCinemaId = room.CinemaId; // Lưu tạm ID rạp để tí quay về

            // KIỂM TRA RÀNG BUỘC: Nếu phòng đã có suất chiếu thì KHÔNG ĐƯỢC XÓA
            if (room.Showtimes != null && room.Showtimes.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa '{room.Name}' vì phòng này đang có {room.Showtimes.Count} suất chiếu. Vui lòng xóa lịch chiếu trước!";
                return RedirectToAction(nameof(RoomList), new { cinemaId = currentCinemaId });
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã xóa {room.Name} thành công!";

            return RedirectToAction(nameof(RoomList), new { cinemaId = currentCinemaId });
        }

        // =================================================================
        // ======================= QUẢN LÝ SUẤT CHIẾU (SHOWTIME) ===========
        // =================================================================

        // 1. CẬP NHẬT HÀM SHOWTIME GET (Truyền thêm dữ liệu cho Popup)
        [HttpGet]
        public async Task<IActionResult> Showtime(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            // Truyền danh sách TOÀN BỘ Phim và Rạp sang View để đổ vào Dropdown của Popup
            ViewBag.AllMovies = await _context.Movies.OrderByDescending(m => m.Id).ToListAsync();
            ViewBag.AllCinemas = await _context.Cinemas.ToListAsync();

            var moviesQuery = _context.Movies.Include(m => m.Showtimes).Where(m => m.Showtimes.Any()).AsQueryable();
            if (!string.IsNullOrEmpty(searchString)) moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchString));

            var movies = await moviesQuery.OrderByDescending(m => m.Id).ToListAsync();
            return View(movies);
        }

        // 2. CẬP NHẬT HÀM TẠO LỊCH CHIẾU POST (Trả về lỗi qua TempData vì dùng Popup)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShowtime(Showtime showtime)
        {
            ModelState.Remove("Movie"); ModelState.Remove("Room"); ModelState.Remove("Cinema"); ModelState.Remove("BookedSeats");

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
                    (showtime.StartTime < s.EndTime.AddMinutes(cleaningMinutes) && showtime.EndTime.AddMinutes(cleaningMinutes) > s.StartTime)
                );

                if (isConflict)
                {
                    // Trả lỗi Xung đột về trang chủ bằng TempData thay vì Model Error
                    TempData["ErrorMessage"] = $"Lỗi Xung Đột: Phòng này đã có lịch chiếu khác. Vui lòng kiểm tra lại khung giờ (đã tính {cleaningMinutes}p dọn dẹp)!";
                    return RedirectToAction(nameof(Showtime));
                }

                showtime.BookedSeats = "";
                _context.Showtimes.Add(showtime);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đã xếp lịch chiếu thành công!";
            }
            return RedirectToAction(nameof(Showtime));
        }

        // 3. THÊM 2 HÀM API HỖ TRỢ CHO DỮ LIỆU ĐỘNG TRONG POPUP
        [HttpGet]
        public async Task<IActionResult> GetRoomsByCinema(int cinemaId)
        {
            var rooms = await _context.Rooms.Where(r => r.CinemaId == cinemaId && r.Status == 1).Select(r => new { id = r.Id, name = r.Name }).ToListAsync();
            return Json(rooms);
        }

        // 3. API HỖ TRỢ: LẤY LỊCH BẬN CỦA PHÒNG
        [HttpGet]
        public async Task<IActionResult> GetBusySchedules(int roomId, string? date)
        {
            var query = _context.Showtimes
                                .Include(s => s.Movie)
                                .Where(s => s.RoomId == roomId);

            // Nếu Admin ĐÃ CHỌN ngày -> Chỉ lấy lịch của ngày đó
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime selectedDate))
            {
                query = query.Where(s => s.StartTime.Date == selectedDate.Date);
            }
            else
            {
                // Nếu CHƯA CHỌN ngày -> Lấy tất cả lịch từ hôm nay trở về sau
                query = query.Where(s => s.StartTime.Date >= DateTime.Today);
            }

            var schedules = await query
                .OrderBy(s => s.StartTime)
                .Select(s => new {
                    title = s.Movie.Title,
                    date = s.StartTime.ToString("dd/MM/yyyy"), // Bổ sung Ngày chiếu
                    start = s.StartTime.ToString("HH:mm"),
                    end = s.EndTime.AddMinutes(15).ToString("HH:mm") // Đã cộng 15p dọn dẹp
                }).ToListAsync();

            return Json(schedules);
        }

        // =================================================================
        // 4. MÀN HÌNH CẤP 2: CHI TIẾT CÁC SUẤT CHIẾU CỦA 1 PHIM
        // =================================================================

        [HttpGet]
        public async Task<IActionResult> ShowtimeList(int? movieId)
        {
            if (movieId == null) return NotFound("Vui lòng chọn một bộ phim.");

            // Lấy thông tin phim để làm tiêu đề trang
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound("Không tìm thấy phim này.");

            // Lấy TẤT CẢ các suất chiếu của phim này, kèm theo thông tin Rạp và Phòng
            // Sắp xếp theo ngày giờ chiếu từ gần đến xa
            var showtimes = await _context.Showtimes
                                          .Include(s => s.Cinema)
                                          .Include(s => s.Room)
                                          .Where(s => s.MovieId == movieId)
                                          .OrderBy(s => s.StartTime)
                                          .ToListAsync();

            ViewBag.MovieInfo = movie;
            return View(showtimes);
        }

        // =================================================================
        // 5. XÓA 1 SUẤT CHIẾU LẺ
        // =================================================================
        public async Task<IActionResult> DeleteShowtime(int? id)
        {
            if (id == null) return NotFound();

            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();

            int currentMovieId = showtime.MovieId; // Lưu tạm để tí quay về đúng phim

            // Kiểm tra: Nếu suất chiếu này ĐÃ CÓ NGƯỜI ĐẶT GHẾ thì không cho xóa (để bảo vệ khách hàng)
            if (!string.IsNullOrEmpty(showtime.BookedSeats))
            {
                TempData["ErrorMessage"] = "Không thể hủy suất chiếu này vì đã có khách hàng đặt vé!";
                return RedirectToAction(nameof(ShowtimeList), new { movieId = currentMovieId });
            }

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã hủy suất chiếu thành công!";
            return RedirectToAction(nameof(ShowtimeList), new { movieId = currentMovieId });
        }

        // 2. XÓA CÔNG CHIẾU (Xóa sạch mọi suất chiếu của 1 phim, không xóa phim)
        public async Task<IActionResult> DeleteAllShowtimes(int? movieId)
        {
            if (movieId == null) return NotFound();

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            // Tìm tất cả các suất chiếu đang gắn với mã phim này
            var showtimes = await _context.Showtimes.Where(s => s.MovieId == movieId).ToListAsync();

            if (showtimes.Any())
            {
                // Xóa hàng loạt
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
        // HÀM BÁO CÁO DOANH THU (ĐÃ CẬP NHẬT CHỨC NĂNG LỌC THÁNG VÀ TABLE)
        // =================================================================
        [HttpGet]
        public async Task<IActionResult> RevenueReport(string selectedMonth)
        {
            // 1. XỬ LÝ LỌC THÁNG NĂM
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

            // 2. KÉO DỮ LIỆU TỪ DATABASE THEO THÁNG ĐƯỢC CHỌN
            var bookings = await _context.Bookings
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
                .Where(b => b.BookingTime.Month == month && b.BookingTime.Year == year)
                .ToListAsync();

            // 3. TÍNH TOÁN CHO PHẦN PHIM (Bên trái)
            var movieRevenue = bookings
                .GroupBy(b => b.Showtime.Movie.Title)
                .Select(g => new MovieRevVM {
                    MovieName = g.Key,
                    TicketsSold = g.Sum(b => b.SelectedSeats.Split(',', StringSplitOptions.RemoveEmptyEntries).Length),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderByDescending(m => m.Revenue)
                .ToList();

            // 4. TÍNH TOÁN CHO PHẦN NGÀY (Bên phải)
            var dailyRevenue = bookings
                .GroupBy(b => b.BookingTime.Date)
                .Select(g => new DailyRevVM {
                    DateString = g.Key.ToString("dd/MM/yyyy"),
                    Revenue = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(x => x.DateString)
                .ToList();

            // 5. ĐÓNG GÓI JSON CHO CHART.JS VẼ BIỂU ĐỒ
            ViewBag.MovieLabels = JsonSerializer.Serialize(movieRevenue.Select(m => m.MovieName));
            ViewBag.MovieData = JsonSerializer.Serialize(movieRevenue.Select(m => m.Revenue));
            ViewBag.DateLabels = JsonSerializer.Serialize(dailyRevenue.Select(d => d.DateString));
            ViewBag.DateData = JsonSerializer.Serialize(dailyRevenue.Select(d => d.Revenue));

            // 6. GỬI DỮ LIỆU LIST SANG ĐỂ VẼ BẢNG TABLE
            ViewBag.MovieTable = movieRevenue;
            ViewBag.DateTable = dailyRevenue;
            ViewBag.TotalRevenue = bookings.Sum(b => b.TotalPrice);

            return View();
        }
    } // <-- Ngoặc đóng class Controller

    // ==========================================
    // CÁC CLASS PHỤ TRỢ (Để ở ngoài Controller)
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