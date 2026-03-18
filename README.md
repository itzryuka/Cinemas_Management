Project mang chủ để về quản lý rạp phim

Được thực hiện 100% trên Visual Studio 2022 (.Net 9.0 hoặc hơn)


Có 1 vài vấn đề lưu ý:

Phải có SQL Server Management

Project tôi thực hiện trên VS2022 nên chỉ dùng được .NET 9.0 các package cần thiết đều có sẵn

Nên nếu bạn dùng 10.0 thì hãy thực hiện như sau

Chọn file BAO_Cinemas:

chỉnh sửa đoạn <TargetFramework>net9.0</TargetFramework> thành <TargetFramework>net10.0</TargetFramework>

Và các phần Version="9.0.14" hay 9.0.5 thành 10.0 hết để có thể cập nhập thành công

và Chuột phải vào Solution 'BAO_Cinemas' -> chọn Manage NuGet Packages for Solution và update hết packages

Sau đó thì thực hiện bước dưới

B1: Sau khi xong chọn file Models\ApplicationDbContext.cs

Trong đó có Seeding Data (Dữ liệu mẫu) để khi chạy trương trình có thể hiển thị được các tài nguyên của trương trình

B2: Chọn phần Tool -> Nuget Package Manager -> Package Manager Console

B3: Viết và Enter lần lượt 2 hàm này để tạo Database

Add-Migration InitialData

Update-Database

B4: Ấn F5 hoặc bấm nút Play màu xanh ở thanh công cụ

Trong data seeding có tạo sẵn 1 tài khoản Admin để mở khóa các chức năng quản lý

Email: admin@baocinemas.com

Pass: Admin@123
