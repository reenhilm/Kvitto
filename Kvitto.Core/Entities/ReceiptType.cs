using System.ComponentModel.DataAnnotations;

namespace Kvitto.Core.Entities
{
    public class ReceiptType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = default!;
    }
}