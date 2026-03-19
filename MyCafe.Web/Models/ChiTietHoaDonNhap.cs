using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_CTHDN")]
    public class ChiTietHoaDonNhap
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("HoaDonNhap")]
        [StringLength(10)]
        public string MaHDN { get; set; }
        public virtual HoaDonNhap HoaDonNhap { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Sanpham")]
        [StringLength(10)]
        public string MaSP { get; set; }
        public virtual Sanpham Sanpham { get; set; }

        public int? SoLuong { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DonGia { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ThanhTien { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? KhuyenMai { get; set; }
    }
}
