# ☕ Lộ trình xây dựng dự án Web Quản Lý Cafe (ASP.NET Core 8)

Tài liệu này hướng dẫn bạn cách xây dựng lại dự án "Quản Lý Cafe" từ bản gốc WinForms sang nền tảng Web hiện đại. Chúng ta sẽ đi từ các thành phần nhỏ nhất đến logic nghiệp vụ phức tạp.

---

## 🏗️ Giai đoạn 1: Khởi tạo & Cấu trúc (Setup)
Mục tiêu: Thiết lập môi trường và tạo khung dự án.

### 1. Tạo Solution và Project
Mở Terminal tại thư mục làm việc của bạn:
```bash
# 1. Tạo thư mục gốc
mkdir MyCafeWeb && cd MyCafeWeb

# 2. Tạo Solution (Giải pháp tổng thể)
dotnet new sln -n MyCafeWeb

# 3. Tạo dự án ASP.NET Core MVC (Giao diện + Logic)
dotnet new mvc -n MyCafe.Web

# 4. Thêm dự án vào Solution
dotnet add sln MyCafe.Web MyCafe.Web.csproj
```

### 2. Cài đặt Thư viện (Entity Framework Core)
Để làm việc với SQL Server một cách hiện đại (thay vì viết SQL dài dòng như bản cũ):
```bash
cd MyCafe.Web
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

---

## 🗄️ Giai đoạn 2: Thiết kế Cơ sở dữ liệu (Database Setup)
*Dành cho Server: `.\SQLEXPRESS`*

### 1. Tạo các Models (Thư mục `Models/`)
Mỗi bảng trong SQL tương ứng với 1 Class trong C#. Tạo các file sau:

- **Loai.cs**:
```csharp
using System.ComponentModel.DataAnnotations;
namespace MyCafe.Web.Models {
    public class Loai {
        [Key]
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
    }
}
```
- **Que.cs**: (Tương tự với `MaQue`, `TenQue`)
- **Congdung.cs**: (Tương tự với `MaCongDung`, `TenCongDung`)

### 2. Tạo ApplicationDbContext (Thư mục `Data/`)
Tạo file `Data/ApplicationDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Models;

namespace MyCafe.Web.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Loai> Loais { get; set; }
        public DbSet<Que> Ques { get; set; }
        public DbSet<Congdung> Congdungs { get; set; }
    }
}
```

### 3. Cấu hình Chuỗi kết nối (appsettings.json)
Sửa file `appsettings.json` (Chú ý dấu `\\` cho SQLEXPRESS):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=QL_Quancaphe;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### 4. Đăng ký vào hệ thống (Program.cs)
Thêm các dòng sau vào đầu file `Program.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 5. Lệnh khởi tạo Database (Chạy trong Terminal)
```bash
# 1. Cài đặt công cụ EF (nếu chưa có)
dotnet tool install --global dotnet-ef

# 2. Kiểm tra lỗi code
dotnet build

# 3. Tạo kịch bản Migration
dotnet ef migrations add InitialCreate

# 4. Đẩy vào SQL Server thực tế
dotnet ef database update
```

---

## 🚀 Giai đoạn 3: Phát triển tính năng (Làm từ nhỏ đến lớn)

### Bước 1: Quản lý Danh mục (Học CRUD cơ bản)
Bắt đầu với bảng `Loai` (Loại sản phẩm).
- **Mục tiêu:** Hiển thị danh sách, Thêm mới, Sửa, Xóa.
- **Câu lệnh tạo nhanh Code mẫu (Scaffolding):**
```bash
# Cài đặt công cụ sinh code
dotnet tool install -g dotnet-aspnet-codegenerator

# Sinh tự động Controller và View cho bảng Loai
dotnet aspnet-codegenerator controller -name LoaiController -m Loai -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout
```

### Bước 2: Quản lý Sản phẩm (Học về Media)
- **Logic:** Khi thêm sản phẩm, bạn cần xử lý chọn ảnh từ máy tính và lưu vào thư mục `wwwroot/images/`.
- **Hiển thị:** Học cách dùng thẻ `<img>` để load ảnh từ server lên web.

### Bước 3: Quản lý Nhân viên & Đăng nhập (Security)
- Chuyển bảng `User` từ WinForms sang hệ thống Identity của ASP.NET.
- **Tính năng:** Đăng nhập, phân quyền (Admin được sửa giá, Nhân viên chỉ được bán hàng).

### Bước 4: Nghiệp vụ Bán hàng (The Core Logic)
Đây là "trái tim" của dự án cafe.
1. **Giao diện bán hàng:** Chọn món -> Thêm vào danh sách tạm (Giỏ hàng).
2. **Thanh toán:**
   - Tạo 1 dòng trong `HDB` (Mã HĐ, Tổng tiền, Ngày bán).
   - Tạo nhiều dòng trong `CTHDB` (Từng món khách đã mua).
   - **Quan trọng:** Tự động trừ số lượng sản phẩm trong bảng `Sanpham` (Trừ tồn kho).

---

## 📊 Giai đoạn 5: Báo cáo & Thống kê
- **Thống kê Doanh thu:** Viết lệnh LINQ để tính tổng tiền theo tháng/năm.
- **Top bán chạy:** Tìm ra 5 sản phẩm có số lượng bán nhiều nhất trong `CTHDB`.
- **Tìm kiếm:** Tìm hóa đơn theo tên khách hàng hoặc mã nhân viên.

---

## 💡 Các câu lệnh dotnet "Bỏ túi"
| Lệnh | Mô tả |
| :--- | :--- |
| `dotnet run` | Khởi động trang web |
| `dotnet watch` | Chạy web và tự động cập nhật khi bạn sửa code (Rất nên dùng) |
| `dotnet ef migrations add InitialCreate` | Tạo file kịch bản để thay đổi Database |
| `dotnet ef database update` | Đẩy các thay đổi từ code xuống SQL Server thực tế |
| `dotnet build` | Kiểm tra xem code có lỗi cú pháp không |

---

## 🚩 Lời khuyên khi học
- Đừng copy-paste toàn bộ. Hãy gõ từng dòng Code từ bản WinForms sang Web để hiểu sự khác biệt.
- Bản WinForms dùng `SQL_tb_...` để kết nối, bản Web sẽ dùng `Entity Framework Core`. Hãy tập trung học phần này!
