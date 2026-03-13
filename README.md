Project mang chủ để về quản lý rạp phim

Được thực hiện 100% trên Visual Studio 2022 (.Net 9.0 hoặc hơn)


Có 1 vài vấn đề lưu ý:

1, Cần phải có SQL Server Management Studio (2022 hoặc hơn)

2, Có cài những Nuget - Solution (Packages for Solution)

Ở đây mình sử dụng .NET 9.0 nên tải phiên bản 9.0.14

<img width="1163" height="98" alt="image" src="https://github.com/user-attachments/assets/9a99b5df-0b02-42a5-a154-002eac5d6b0b" />
<img width="1174" height="121" alt="image" src="https://github.com/user-attachments/assets/e7f27a85-4f63-4f45-bff7-c49fae1ca09d" />
<img width="1188" height="120" alt="image" src="https://github.com/user-attachments/assets/1ec84a7a-b5bc-424a-b5b9-0aa64ca335db" />
<img width="1184" height="109" alt="image" src="https://github.com/user-attachments/assets/2f560dbc-df1f-4a80-9581-e74faceddeba" />
<img width="1177" height="107" alt="image" src="https://github.com/user-attachments/assets/2aa34b17-8357-49d6-be76-daa76eaa4f9d" />
<img width="1178" height="94" alt="image" src="https://github.com/user-attachments/assets/1b288c5c-6f39-4923-9cf8-028e9b7b47bc" />
<img width="1184" height="126" alt="image" src="https://github.com/user-attachments/assets/97d1c609-f1cc-444a-9de9-80b5e77ba76a" />

Để tải được những thứ trên cần chuột phải vào Solution như hình dưới:
<img width="476" height="237" alt="image" src="https://github.com/user-attachments/assets/d7bb8d55-b55e-49f4-8163-92947ae14a35" />

Chọn Manage Nuget Packages for Solution
<img width="583" height="395" alt="image" src="https://github.com/user-attachments/assets/8dbee4a4-566b-4626-ae12-85dce6a79566" />


B1: Sau khi xong chọn file Models\ApplicationDbContext.cs

Trong đó có Seeding Data (Dữ liệu mẫu) để khi chạy trương trình có thể hiển thị được các tài nguyên của trương trình

B2: Chọn phần Tool -> Nuget Package Manager -> Package Manager Console

B3: Viết và Enter lần lượt 2 hàm này để tạo Database

Add-Migration ActiveSeedData

Update-Database

B4: Ấn F5 hoặc bấm nút Play màu xanh ở thanh công cụ

Vì nhiều thứ vẫn chưa rõ mong mn có thể để lại góp ý ạ
