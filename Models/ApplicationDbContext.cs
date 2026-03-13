using BAO_Cinemas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Thêm dòng này
using Microsoft.EntityFrameworkCore;

namespace BAO_Cinemas.Models
{
    // PHẢI SỬA: DbContext -> IdentityDbContext<IdentityUser>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // RẤT QUAN TRỌNG: Phải giữ lại dòng này để Identity khởi tạo các bảng User/Role
            base.OnModelCreating(modelBuilder);

            // --- TẠO QUYỀN ADMIN VÀ USER MẪU VỚI ID CỐ ĐỊNH ---
            // Sử dụng các chuỗi text cố định thay vì Guid.NewGuid()
            string adminRoleId = "admin-role-id-123";
            string userRoleId = "user-role-id-456";
            string adminUserId = "admin-user-id-789";

            // 1. Thêm các Role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            // 2. Tạo tài khoản Admin mặc định
            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@baocinemas.com",
                NormalizedUserName = "ADMIN@BAOCINEMAS.COM",
                Email = "admin@baocinemas.com",
                NormalizedEmail = "ADMIN@BAOCINEMAS.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123")
            });

            // 3. Gán quyền Admin cho tài khoản
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            // --- THÊM ĐOẠN CODE NÀY ĐỂ FIX LỖI CASCADE PATH ---
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Showtimes)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Cinema)
                .WithMany()
                .HasForeignKey(s => s.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            // =================================================================
            // --- DỮ LIỆU MẪU CHI TIẾT
            // =================================================================

            // 1. SEED CINEMA (RẠP CHIẾU)
            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { Id = 1, Name = "BAO Cinemas Cầu Giấy", Address = "241 Xuân Thủy, Cầu Giấy, Hà Nội", Hotline = "1900 1111" },
                new Cinema { Id = 2, Name = "BAO Cinemas Quận 1", Address = "Bitexco, Quận 1, TP.HCM", Hotline = "1900 2222" },
                new Cinema { Id = 4, Name = "BAO Cinemas Hà Đông", Address = "Ngõ 3, Phố Xốm, Hà Đông", Hotline = "1900 3667" }
            );

            // 2. SEED ROOM (PHÒNG CHIẾU)
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Phòng 1 - Standard", TotalSeats = 98, CinemaId = 1, Status = 1 },
                new Room { Id = 2, Name = "Phòng 2 - IMAX", TotalSeats = 98, CinemaId = 1, Status = 1 },
                new Room { Id = 3, Name = "Phòng 1 - Standard", TotalSeats = 98, CinemaId = 2, Status = 1 }
            );

            // 3. SEED MOVIE (DANH SÁCH PHIM)
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Avengers: Endgame", Genre = "Hành Động, Viễn Tưởng", Duration = 180, ReleaseDate = new DateTime(2019, 4, 26), PosterUrl = "/images/posters/avengers_endgame.jpg", CategoryId = 1 },
                new Movie { Id = 2, Title = "Chào Vũ Nhá", Genre = "Kinh Dị", Duration = 148, ReleaseDate = new DateTime(2025, 9, 9), PosterUrl = "/images/posters/chao_vu_nha.jpg", CategoryId = 1 },
                new Movie { Id = 3, Title = "Kokuho", Genre = "Tâm Lý", Duration = 174, ReleaseDate = new DateTime(2026, 3, 6), PosterUrl = "/images/posters/fb23d9ba-3b22-4ce5-abfe-9dc0560973f0_Kokuho.jpg", CategoryId = 0 },
                new Movie { Id = 4, Title = "Zootopia", Genre = "Hoạt hình, Hài, Phiêu lưu", Duration = 108, ReleaseDate = new DateTime(2016, 3, 4), PosterUrl = "/images/posters/db3e3440-c65d-4651-9b02-0f4340a666b7_Zootopia.jpg", CategoryId = 1 },
                new Movie { Id = 5, Title = "Up", Genre = "Hoạt hình, Phiêu lưu, Gia đình", Duration = 96, ReleaseDate = new DateTime(2009, 5, 29), PosterUrl = "/images/posters/dbe18a3d-adbc-418d-b6bc-105c02f29d15_up.jpg", CategoryId = 0 },
                new Movie { Id = 6, Title = "Despicable Me", Genre = "Hoạt hình, Phiêu lưu, Gia đình ", Duration = 95, ReleaseDate = new DateTime(2010, 7, 9), PosterUrl = "/images/posters/a5dbc4af-ecea-4024-b259-9a62536cab00_despicable_me.jpg", CategoryId = 0 },
                new Movie { Id = 7, Title = "La Tiểu Hắc Chiến Ký 2", Genre = "Hành Động, Hoạt Hình", Duration = 120, ReleaseDate = new DateTime(2026, 3, 20), PosterUrl = "/images/posters/1f9996b4-1960-4b42-9152-dc93bc9e9688_la_tieu_hac_chien_ky_2.jpg", CategoryId = 2 },
                new Movie { Id = 9, Title = "Thoát Khỏi Tận Thế", Genre = "Khoa Học Viễn Tưởng, Phiêu Lưu", Duration = 157, ReleaseDate = new DateTime(2026, 3, 20), PosterUrl = "/images/posters/e35007eb-37a0-486c-8e49-ac82a9dabc8d_thoat_khoi_tan_the.jpg", CategoryId = 2 },
                new Movie { Id = 10, Title = "Đại Tiệc Trăng Máu", Genre = "Hài, Kinh Dị, Tâm Lý", Duration = 180, ReleaseDate = new DateTime(2026, 4, 24), PosterUrl = "/images/posters/7a9bab2b-4946-4f05-9653-2484e106a006_dai_tiec_trang_mau_8.jpg", CategoryId = 2 },
                new Movie { Id = 11, Title = "Kung Fu Panda", Genre = "Hoạt hình, Hài, Hành động", Duration = 92, ReleaseDate = new DateTime(2008, 6, 6), PosterUrl = "/images/posters/20bb43de-a951-4595-8be7-01e317964db3_kung_fu_panda.jpg", CategoryId = 0 },
                new Movie { Id = 13, Title = "Finding Nemo", Genre = "Hoạt hình, Phiêu lưu, Gia đình", Duration = 100, ReleaseDate = new DateTime(2003, 5, 30), PosterUrl = "/images/posters/4047f645-7d94-4543-89aa-4cb888877eed_nemo.jpg", CategoryId = 0 },
                new Movie { Id = 15, Title = "John Wick: Chapter 2", Genre = "Hành động, Tội phạm, Giật gân", Duration = 122, ReleaseDate = new DateTime(2017, 2, 10), PosterUrl = "/images/posters/6919d6d6-a607-4fe1-b6f2-14bb834c38ad_john_wick_2.jpg", CategoryId = 1 },
                new Movie { Id = 16, Title = "John Wick: Chapter 1", Genre = "Hành động, Giật gân", Duration = 101, ReleaseDate = new DateTime(2014, 10, 24), PosterUrl = "/images/posters/d685d40f-ec80-4184-81e4-3babf6d01461_john_wick.jpg", CategoryId = 2 },
                new Movie { Id = 17, Title = "John Wick: Chapter 3 – Parabellum", Genre = "Hành động, Tội phạm, Giật gân", Duration = 131, ReleaseDate = new DateTime(2019, 5, 17), PosterUrl = "/images/posters/c821ada9-4298-4132-a8b1-35c772c6f54b_john_wick_3.jpg", CategoryId = 0 },
                new Movie { Id = 18, Title = "Titanic", Genre = "Lãng mạn, Tâm lý", Duration = 194, ReleaseDate = new DateTime(1997, 12, 19), PosterUrl = "/images/posters/5534399c-a8ea-423e-a3ff-fe48c137c945_Titanic.jpg", CategoryId = 0 }
            );

            // 4. SEED SHOWTIME (SUẤT CHIẾU)
            modelBuilder.Entity<Showtime>().HasData(
                new Showtime { Id = 1, MovieId = 1, CinemaId = 1, RoomId = 1, StartTime = new DateTime(2026, 3, 13, 10, 30, 0), EndTime = new DateTime(2026, 3, 13, 13, 30, 0), Price = 75000, BookedSeats = "" },
                new Showtime { Id = 2, MovieId = 1, CinemaId = 1, RoomId = 2, StartTime = new DateTime(2026, 3, 13, 14, 0, 0), EndTime = new DateTime(2026, 3, 13, 17, 0, 0), Price = 90000, BookedSeats = "" },
                new Showtime { Id = 3, MovieId = 1, CinemaId = 2, RoomId = 3, StartTime = new DateTime(2026, 3, 13, 19, 45, 0), EndTime = new DateTime(2026, 3, 13, 22, 45, 0), Price = 100000, BookedSeats = "" },
                new Showtime { Id = 4, MovieId = 1, CinemaId = 2, RoomId = 3, StartTime = new DateTime(2026, 3, 14, 09, 0, 0), EndTime = new DateTime(2026, 3, 14, 12, 0, 0), Price = 75000, BookedSeats = "A1" },
                new Showtime { Id = 10, MovieId = 2, CinemaId = 1, RoomId = 1, StartTime = new DateTime(2026, 3, 18, 06, 0, 0), EndTime = new DateTime(2026, 3, 18, 08, 43, 0), Price = 50000, BookedSeats = "A1" },
                new Showtime { Id = 11, MovieId = 17, CinemaId = 1, RoomId = 1, StartTime = new DateTime(2026, 3, 18, 09, 0, 0), EndTime = new DateTime(2026, 3, 18, 11, 26, 0), Price = 80000, BookedSeats = "" },
                new Showtime { Id = 12, MovieId = 17, CinemaId = 1, RoomId = 2, StartTime = new DateTime(2026, 3, 18, 06, 0, 0), EndTime = new DateTime(2026, 3, 18, 08, 26, 0), Price = 100000, BookedSeats = "A1" },
                new Showtime { Id = 13, MovieId = 18, CinemaId = 2, RoomId = 3, StartTime = new DateTime(2026, 3, 18, 06, 0, 0), EndTime = new DateTime(2026, 3, 18, 09, 29, 0), Price = 75000, BookedSeats = "E8" }
            );

            // 5. SEED BOOKING (LỊCH SỬ ĐẶT VÉ/DOANH THU)
            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, CustomerName = "khach1@gmail.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "A1,A2", TotalPrice = 150000, BookingTime = new DateTime(2026, 3, 10, 14, 30, 0), ShowtimeId = 1 },
                new Booking { Id = 2, CustomerName = "khach2@gmail.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "B5,B6,B7", TotalPrice = 225000, BookingTime = new DateTime(2026, 3, 11, 18, 0, 0), ShowtimeId = 1 },
                new Booking { Id = 3, CustomerName = "khach3@gmail.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "C1,C2", TotalPrice = 180000, BookingTime = new DateTime(2026, 3, 12, 19, 30, 0), ShowtimeId = 2 },
                new Booking { Id = 4, CustomerName = "khach4@gmail.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "D4", TotalPrice = 90000, BookingTime = new DateTime(2026, 3, 13, 20, 0, 0), ShowtimeId = 2 },
                new Booking { Id = 5, CustomerName = "khach5@gmail.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "E1,E2,E3,E4", TotalPrice = 400000, BookingTime = new DateTime(2026, 3, 14, 10, 0, 0), ShowtimeId = 1 },
                new Booking { Id = 6, CustomerName = "admin@baocinemas.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "A1", TotalPrice = 50000, BookingTime = DateTime.Parse("2026-03-14T00:46:53"), ShowtimeId = 10, UserId = adminUserId },
                new Booking { Id = 7, CustomerName = "admin@baocinemas.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "A1", TotalPrice = 75000, BookingTime = DateTime.Parse("2026-03-14T00:52:11"), ShowtimeId = 4, UserId = adminUserId },
                new Booking { Id = 8, CustomerName = "admin@baocinemas.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "A1", TotalPrice = 100000, BookingTime = DateTime.Parse("2026-03-14T00:54:58"), ShowtimeId = 12, UserId = adminUserId },
                new Booking { Id = 9, CustomerName = "doanbaongo@baocinemas.com", CustomerPhone = "Không yêu cầu", SelectedSeats = "E8", TotalPrice = 75000, BookingTime = DateTime.Parse("2026-03-14T01:22:03"), ShowtimeId = 13, UserId = "2c560b88-4178-4531-9177-11b7430c4714" }
            );
        }
    }
}