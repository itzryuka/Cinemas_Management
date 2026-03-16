using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BAO_Cinemas.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Tên rạp (VD: BAO Cinemas Cầu Giấy)

        public string Address { get; set; } // Địa chỉ rạp

        public string Hotline { get; set; } // Số điện thoại

        // Mối quan hệ: 1 Rạp sẽ có nhiều Phòng chiếu
        public ICollection<Room> Rooms { get; set; }
    }
}