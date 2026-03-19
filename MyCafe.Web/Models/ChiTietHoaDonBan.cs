using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_CTHDB")]
    public class ChiTietHoaDonBan
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("HoaDonBan")]
        [StringLength(10)]
        public string MaHDB { get; set; }
        public virtual HoaDonBan HoaDonBan { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaSP { get; set; }

        [StringLength(50)]
        public string TenSP { get; set; }

        public int? SoLuong { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ThanhTien { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? KhuyenMai { get; set; }
    }
}
