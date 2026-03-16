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

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        [Required]
        public string SelectedSeats { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        public DateTime BookingTime { get; set; } = DateTime.Now;

        public int ShowtimeId { get; set; }
        [ForeignKey("ShowtimeId")]
        public Showtime Showtime { get; set; }

        // Khóa ngoại nối với bảng AspNetUsers
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}