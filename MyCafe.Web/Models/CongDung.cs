using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Congdung")]
    public class CongDung
    {
        [Key]
        [StringLength(10)]
        public string MaCongDung { get; set; }

        [Required]
        [StringLength(50)]
        public string TenCongDung { get; set; }

        // Navigation property
        public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
    }
}