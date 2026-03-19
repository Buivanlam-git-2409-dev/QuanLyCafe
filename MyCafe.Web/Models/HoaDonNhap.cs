using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_HDN")]
    public class HoaDonNhap
    {
        [Key]
        [StringLength(10)]
        public string MaHDN { get; set; }

        public DateTime? NgayNhap { get; set; }

        [ForeignKey("NhanVien")]
        [StringLength(10)]
        public string MaNV { get; set; }
        public virtual NhanVien NhanVien { get; set; }

        [ForeignKey("NCC")]
        [StringLength(10)]
        public string MaNCC { get; set; }
        public virtual NCC NCC { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TongTien { get; set; }

        // Navigation property
        public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();
    }
}
