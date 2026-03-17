using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAO_Cinemas.Models;

namespace BAO_Cinemas.Controllers
{
    [Route("api/v1/movies")] // Đổi đường dẫn có chữ v1 cho xịn
    [ApiController]
    public class MoviesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MoviesApiController(ApplicationDbContext context) => _context = context;

        // 1. GET: Lấy danh sách phim
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            return Ok(await _context.Movies.ToListAsync());
        }

        // 2. GET: Lấy chi tiết 1 phim theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound("Không tìm thấy phim!");
            return Ok(movie);
        }

        // 3. POST: Thêm phim mới
        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return StatusCode(201, movie); // 201 là mã Tạo thành công
        }

        // 4. PUT: Cập nhật phim
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id) return BadRequest("ID không khớp!");
            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Cập nhật thành công!");
        }

        // 5. DELETE: Xóa phim
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return Ok("Xóa thành công!");
        }
    }
}