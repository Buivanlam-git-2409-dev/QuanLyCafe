using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_HDB")]
    public class HoaDonBan
    {
        [Key]
        [StringLength(10)]
        public string MaHDB { get; set; }

        public DateTime? NgayBan { get; set; }

        [ForeignKey("NhanVien")]
        [StringLength(10)]
        public string MaNV { get; set; }
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("KhachHang")]
        [StringLength(10)]
        public string MaKH { get; set; }
        public virtual KhachHang KhachHang { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TongTien { get; set; }

        // Navigation property
        public virtual ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();
    }
}
