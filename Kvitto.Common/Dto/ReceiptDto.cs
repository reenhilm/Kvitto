using System.ComponentModel.DataAnnotations;

namespace Kvitto.Common.Dto
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string StoreName { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string Description { get; set; } = default!;

        public DateTime UploadedDate { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}