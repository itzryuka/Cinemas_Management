using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAO_Cinemas.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Tên phòng (VD: Phòng 1, Phòng 2, Phòng IMAX)

        // Mặc định luôn là 98 ghế (7 hàng x 14 cột) như bạn yêu cầu
        public int TotalSeats { get; set; } = 98;

        // Quy ước: 1 = Hoạt động, 2 = Bảo trì, 3 = Dừng hoạt động
        public int Status { get; set; } = 1;

        // Khóa ngoại: Phòng này thuộc Rạp nào?
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }

        // Mối quan hệ: 1 Phòng sẽ có nhiều Suất chiếu khác nhau theo từng khung giờ
        public ICollection<Showtime> Showtimes { get; set; }
    }
}