using BAO_Cinemas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Seat
{
    [Key] public int Id { get; set; }
    [Required] public string SeatCode { get; set; }   // "A1", "B3"
    public char RowLabel { get; set; }
    public int SeatNumber { get; set; }
    [Required] public string SeatType { get; set; } = "standard"; // standard/vip/couple
    public string Status { get; set; } = "active";    // active/inactive

    public int RoomId { get; set; }
    [ForeignKey("RoomId")] public Room Room { get; set; }
    public ICollection<BookingSeat> BookingSeats { get; set; }
}
