using Kvitto.Common.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Kvitto.Common.Dto
{
    [ReceiptMustBeUploadedAfterPurchaseAttribute(
          ErrorMessage = "Receipts can't be uploaded before a purchase!")]
    public abstract class ReceiptForManipulationDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The StoreName shouldn't have more than 50 characters.")]
        public string StoreName { get; set; } = default!;

        [MaxLength(50, ErrorMessage = "The Description shouldn't have more than 50 characters.")]
        public virtual string Description { get; set; } = default!;

        public DateTime UploadedDate { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
