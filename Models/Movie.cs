using System.ComponentModel.DataAnnotations;

namespace BAO_Cinemas.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Genre { get; set; }

        public int Duration { get; set; } // Tính bằng phút

        public DateTime ReleaseDate { get; set; }

        public string PosterUrl { get; set; }

        // Dùng để phân loại tab: 1 (Đang chiếu), 2 (Sắp chiếu), 3 (IMAX)
        public int CategoryId { get; set; }

        // Khai báo mối quan hệ: 1 Bộ phim sẽ có nhiều Suất chiếu
        public ICollection<Showtime> Showtimes { get; set; }
    }
}