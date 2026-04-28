using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAO_Cinemas.Models
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; } // Giờ bắt đầu chiếu

        public DateTime EndTime { get; set; } // Giờ kết thúc

        [Required]
        public double Price { get; set; } // Giá vé cho 1 ghế của suất này (VD: 75000)

        // Lưu danh sách các ghế đã được đặt, cách nhau bằng dấu phẩy. VD: "A1,A2,C5"
        // Mặc định lúc mới tạo suất chiếu là chuỗi rỗng (chưa ai đặt)
        public string Status { get; set; } = "scheduled";

        // Khóa ngoại: Suất chiếu này chiếu Phim gì?
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        // Khóa ngoại: Suất chiếu này ở Phòng nào?
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        // Khóa ngoại: Suất chiếu này ở Rạp nào? (Giúp truy vấn danh sách rạp cực nhanh)
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; }
    }
}