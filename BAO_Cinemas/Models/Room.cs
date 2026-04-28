using BAO_Cinemas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Room
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }
    public int TotalRows { get; set; } = 7;
    public int SeatsPerRow { get; set; } = 14;
    public int TotalSeats => TotalRows * SeatsPerRow; // Tính tự động, không lưu DB
    public int Status { get; set; } = 1;

    public int CinemaId { get; set; }
    [ForeignKey("CinemaId")] public Cinema Cinema { get; set; }
    public ICollection<Showtime> Showtimes { get; set; }
    public ICollection<Seat> Seats { get; set; }
}