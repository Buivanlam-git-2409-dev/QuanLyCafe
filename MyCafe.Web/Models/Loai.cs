using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Loai")]
    public class Loai
    {
        [Key]
        [StringLength(10)]
        public string MaLoai { get; set; }

        [Required]
        [StringLength(50)]
        public string TenLoai { get; set; }

        // Navigation property
        public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
    }
}