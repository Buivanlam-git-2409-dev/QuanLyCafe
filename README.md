# Hệ Thống Quản Lý Cafe ChiChi (Web Version)

Đây là dự án được chuyển đổi từ ứng dụng Windows Forms sang nền tảng **ASP.NET Core MVC 8.0**, cung cấp giải pháp quản lý quán Cafe toàn diện trên trình duyệt web với giao diện hiện đại và quy trình nghiệp vụ chuyên nghiệp.

## 🚀 Các Tính Năng Chính
- **Bảng Điều Khiển (Dashboard):** Xem nhanh các chỉ số kinh doanh (Doanh thu, Sản phẩm, Nhân viên, Đơn hàng).
- **Quản Lý Danh Mục:** Sản phẩm (kèm hình ảnh), Loại món, Công dụng món ăn.
- **Quản Lý Nhân Sự & Khách Hàng:** Thông tin nhân viên, quê quán, nhà cung cấp và khách hàng thân thiết.
- **Nghiệp Vụ Bán Hàng (Sales):** Lập hóa đơn bán hàng, tự động trừ số lượng trong kho khi xuất món.
- **Nghiệp Vụ Nhập Hàng (Inventory):** Nhập hàng từ nhà cung cấp, tự động cộng tồn kho và cập nhật giá nhập.
- **In Ấn:** Hỗ trợ in hóa đơn/phiếu nhập hàng trực tiếp từ trình duyệt.

---

## 🛠 Yêu Cầu Hệ Thống
1. **.NET 8.0 SDK** (hoặc mới hơn).
2. **SQL Server** (LocalDB hoặc Express/Standard).
3. **Visual Studio 2022** (khuyên dùng) hoặc **VS Code**.

---

## ⚙️ Hướng Dẫn Cấu Hình & Chạy Dự Án

### Bước 1: Cấu hình Cơ sở dữ liệu (Database)
1. Mở tệp `appsettings.json` trong thư mục `MyCafe.Web`.
2. Chỉnh sửa dòng `DefaultConnection` để khớp với máy của bạn:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TÊN_MÁY_CỦA_BẠN;Database=QL_Quancaphe;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
   *Lưu ý: Thay `TÊN_MÁY_CỦA_BẠN` bằng tên instance SQL Server của bạn (ví dụ: `localhost`, `.\SQLEXPRESS` hoặc tên máy tính).*

### Bước 2: Tạo Database & Migrations
Dự án đã có sẵn các bản Migrations. Bạn chỉ cần chạy lệnh sau để tự động tạo cấu trúc bảng trong SQL Server:

**Sử dụng Terminal/Command Prompt:**
```powershell
dotnet ef database update
```
*(Nếu chưa cài công cụ EF, hãy chạy: `dotnet tool install --global dotnet-ef`)*

**Hoặc sử dụng Package Manager Console (trong Visual Studio):**
```powershell
Update-Database
```

### Bước 3: Chạy Ứng Dụng
1. Trong Visual Studio: Nhấn phím `F5` hoặc nút `Start`.
2. Hoặc sử dụng CLI:
   ```powershell
   dotnet run
   ```
3. Truy cập địa chỉ hiển thị trên màn hình (thường là `https://localhost:5001` hoặc `http://localhost:5000`).

---

## 📁 Cấu Trúc Thư Mục
- `/Models`: Định nghĩa các thực thể (Entity) như Sanpham, HoaDon, NhanVien.
- `/Data`: Chứa `ApplicationDbContext` - Cấu hình EF Core và các quan hệ bảng.
- `/Controllers`: Xử lý logic nghiệp vụ cho từng phân hệ.
- `/Views`: Giao diện người dùng sử dụng Razor Engine và Bootstrap 5.
- `/wwwroot`: Chứa các tài nguyên tĩnh như CSS, JS, hình ảnh.

---

## 📝 Lưu Ý Nghiệp Vụ
- **Quản lý Kho:** Hệ thống sẽ tự động cộng/trừ số lượng sản phẩm dựa trên Hóa đơn Nhập/Bán. Hãy đảm bảo bạn tạo Sản phẩm trước khi thực hiện các giao dịch.
- **Xóa Dữ Liệu:** Khi xóa một Hóa đơn Bán, hệ thống sẽ tự động hoàn trả (cộng lại) số lượng sản phẩm vào kho để đảm bảo tính chính xác.

---
*Chúc bạn quản lý quán Cafe thành công!*
