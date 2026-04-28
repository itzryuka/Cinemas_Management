using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BAO_Cinemas.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        public DateTime BookingTime { get; set; } = DateTime.Now;

        public string Status { get; set; } = "confirmed";

        public int ShowtimeId { get; set; }
        [ForeignKey("ShowtimeId")]
        public Showtime Showtime { get; set; }

        // Khóa ngoại nối với bảng AspNetUsers
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; }
    }
}