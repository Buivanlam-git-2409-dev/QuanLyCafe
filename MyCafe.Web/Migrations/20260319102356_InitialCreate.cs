using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCafe.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Congdung",
                columns: table => new
                {
                    MaCongDung = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenCongDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Congdung", x => x.MaCongDung);
                });

            migrationBuilder.CreateTable(
                name: "tb_Khachhang",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenKH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Khachhang", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "tb_Loai",
                columns: table => new
                {
                    MaLoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Loai", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "tb_NCC",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_NCC", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "tb_Que",
                columns: table => new
                {
                    MaQue = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenQue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Que", x => x.MaQue);
                });

            migrationBuilder.CreateTable(
                name: "tb_Sanpham",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaLoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    MaCongDung = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HinhAnh = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Sanpham", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_tb_Sanpham_tb_Congdung_MaCongDung",
                        column: x => x.MaCongDung,
                        principalTable: "tb_Congdung",
                        principalColumn: "MaCongDung");
                    table.ForeignKey(
                        name: "FK_tb_Sanpham_tb_Loai_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "tb_Loai",
                        principalColumn: "MaLoai");
                });

            migrationBuilder.CreateTable(
                name: "tb_Nhanvien",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaQue = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Nhanvien", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK_tb_Nhanvien_tb_Que_MaQue",
                        column: x => x.MaQue,
                        principalTable: "tb_Que",
                        principalColumn: "MaQue");
                });

            migrationBuilder.CreateTable(
                name: "tb_HDB",
                columns: table => new
                {
                    MaHDB = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayBan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_HDB", x => x.MaHDB);
                    table.ForeignKey(
                        name: "FK_tb_HDB_tb_Khachhang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "tb_Khachhang",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK_tb_HDB_tb_Nhanvien_MaNV",
                        column: x => x.MaNV,
                        principalTable: "tb_Nhanvien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "tb_HDN",
                columns: table => new
                {
                    MaHDN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_HDN", x => x.MaHDN);
                    table.ForeignKey(
                        name: "FK_tb_HDN_tb_NCC_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "tb_NCC",
                        principalColumn: "MaNCC");
                    table.ForeignKey(
                        name: "FK_tb_HDN_tb_Nhanvien_MaNV",
                        column: x => x.MaNV,
                        principalTable: "tb_Nhanvien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "tb_CTHDB",
                columns: table => new
                {
                    MaHDB = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KhuyenMai = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_CTHDB", x => new { x.MaHDB, x.MaSP });
                    table.ForeignKey(
                        name: "FK_tb_CTHDB_tb_HDB_MaHDB",
                        column: x => x.MaHDB,
                        principalTable: "tb_HDB",
                        principalColumn: "MaHDB",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_CTHDN",
                columns: table => new
                {
                    MaHDN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KhuyenMai = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_CTHDN", x => new { x.MaHDN, x.MaSP });
                    table.ForeignKey(
                        name: "FK_tb_CTHDN_tb_HDN_MaHDN",
                        column: x => x.MaHDN,
                        principalTable: "tb_HDN",
                        principalColumn: "MaHDN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_CTHDN_tb_Sanpham_MaSP",
                        column: x => x.MaSP,
                        principalTable: "tb_Sanpham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_CTHDN_MaSP",
                table: "tb_CTHDN",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_tb_HDB_MaKH",
                table: "tb_HDB",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_tb_HDB_MaNV",
                table: "tb_HDB",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_tb_HDN_MaNCC",
                table: "tb_HDN",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_tb_HDN_MaNV",
                table: "tb_HDN",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Nhanvien_MaQue",
                table: "tb_Nhanvien",
                column: "MaQue");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Sanpham_MaCongDung",
                table: "tb_Sanpham",
                column: "MaCongDung");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Sanpham_MaLoai",
                table: "tb_Sanpham",
                column: "MaLoai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_CTHDB");

            migrationBuilder.DropTable(
                name: "tb_CTHDN");

            migrationBuilder.DropTable(
                name: "tb_HDB");

            migrationBuilder.DropTable(
                name: "tb_HDN");

            migrationBuilder.DropTable(
                name: "tb_Sanpham");

            migrationBuilder.DropTable(
                name: "tb_Khachhang");

            migrationBuilder.DropTable(
                name: "tb_NCC");

            migrationBuilder.DropTable(
                name: "tb_Nhanvien");

            migrationBuilder.DropTable(
                name: "tb_Congdung");

            migrationBuilder.DropTable(
                name: "tb_Loai");

            migrationBuilder.DropTable(
                name: "tb_Que");
        }
    }
}
