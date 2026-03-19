using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCafe.Web.Models
{
    [Table("tb_Que")]
    public class Que
    {
        [Key]
        [StringLength(10)]
        public string MaQue { get; set; }

        [Required]
        [StringLength(50)]
        public string TenQue { get; set; }

        // Navigation property
        public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
    }
}