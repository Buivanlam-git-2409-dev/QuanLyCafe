using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Nhanvien")]
    public class NhanVien
    {
        [Key]
        [StringLength(10)]
        public string MaNV { get; set; }

        [Required]
        [StringLength(50)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(10)]
        public string GioiTinh { get; set; }

        public DateTime? NgaySinh { get; set; }

        [ForeignKey("Que")]
        [StringLength(10)]
        public string MaQue { get; set; }
        public virtual Que Que { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        // Navigation properties
        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
        public virtual ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();
    }
}
