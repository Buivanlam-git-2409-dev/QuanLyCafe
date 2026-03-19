using Microsoft.EntityFrameworkCore;
using MyCafe.Web.Models;

namespace MyCafe.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        // Master Data
        public DbSet<Loai> Loais { get; set; }
        public DbSet<CongDung> CongDungs { get; set; }
        public DbSet<Que> Ques { get; set; }
        public DbSet<Sanpham> Sanphams { get; set; }
        public DbSet<NCC> NCCs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        
        // Staff & Organization
        public DbSet<NhanVien> NhanViens { get; set; }
        
        // Transactions
        public DbSet<HoaDonNhap> HoaDonNhaps { get; set; }
        public DbSet<HoaDonBan> HoaDonBans { get; set; }
        public DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }
        public DbSet<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite keys for detail tables
            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .HasKey(c => new { c.MaHDN, c.MaSP });

            modelBuilder.Entity<ChiTietHoaDonBan>()
                .HasKey(c => new { c.MaHDB, c.MaSP });

            // Configure foreign key relationships
            modelBuilder.Entity<Sanpham>()
                .HasOne(s => s.Loai)
                .WithMany(l => l.Sanphams)
                .HasForeignKey(s => s.MaLoai)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Sanpham>()
                .HasOne(s => s.CongDung)
                .WithMany(c => c.Sanphams)
                .HasForeignKey(s => s.MaCongDung)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NhanVien>()
                .HasOne(n => n.Que)
                .WithMany(q => q.NhanViens)
                .HasForeignKey(n => n.MaQue)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HoaDonNhap>()
                .HasOne(h => h.NhanVien)
                .WithMany(n => n.HoaDonNhaps)
                .HasForeignKey(h => h.MaNV)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HoaDonNhap>()
                .HasOne(h => h.NCC)
                .WithMany(n => n.HoaDonNhaps)
                .HasForeignKey(h => h.MaNCC)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HoaDonBan>()
                .HasOne(h => h.NhanVien)
                .WithMany(n => n.HoaDonBans)
                .HasForeignKey(h => h.MaNV)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HoaDonBan>()
                .HasOne(h => h.KhachHang)
                .WithMany(k => k.HoaDonBans)
                .HasForeignKey(h => h.MaKH)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .HasOne(c => c.HoaDonNhap)
                .WithMany(h => h.ChiTietHoaDonNhaps)
                .HasForeignKey(c => c.MaHDN)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .HasOne(c => c.Sanpham)
                .WithMany()
                .HasForeignKey(c => c.MaSP)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChiTietHoaDonBan>()
                .HasOne(c => c.HoaDonBan)
                .WithMany(h => h.ChiTietHoaDonBans)
                .HasForeignKey(c => c.MaHDB)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}