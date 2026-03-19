using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Sanpham")]
    public class Sanpham
    {
        [Key]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(50)]
        public string TenSP { get; set; }

        [ForeignKey("Loai")]
        [StringLength(10)]
        public string MaLoai { get; set; }
        public virtual Loai Loai { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GiaNhap { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GiaBan { get; set; }

        public int? SoLuong { get; set; }

        [ForeignKey("CongDung")]
        [StringLength(10)]
        public string MaCongDung { get; set; }
        public virtual CongDung CongDung { get; set; }

        [Required]
        public byte[] HinhAnh { get; set; } = Array.Empty<byte>();

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
