using BAO_Cinemas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookingSeat
{
    [Key] public int Id { get; set; }
    public string Status { get; set; } = "confirmed"; // confirmed/cancelled

    public int BookingId { get; set; }
    [ForeignKey("BookingId")] public Booking Booking { get; set; }

    public int SeatId { get; set; }
    [ForeignKey("SeatId")] public Seat Seat { get; set; }

    public int ShowtimeId { get; set; }
    [ForeignKey("ShowtimeId")] public Showtime Showtime { get; set; }
}