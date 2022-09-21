using System.ComponentModel.DataAnnotations;

namespace Kvitto.Core.Entities
{
    public class UploadedFile
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ContentType { get; set; } = default!;

        //Nav prop
        public Receipt? Receipt { get; set; } = default!;
        //FK
        public int? ReceiptId { get; set; }
    }
}