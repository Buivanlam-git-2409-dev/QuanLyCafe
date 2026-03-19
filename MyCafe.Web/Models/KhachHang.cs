using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Khachhang")]
    public class KhachHang
    {
        [Key]
        [StringLength(10)]
        public string MaKH { get; set; }

        [Required]
        [StringLength(50)]
        public string TenKH { get; set; }

        // Navigation property
        public virtual ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();
    }
}
