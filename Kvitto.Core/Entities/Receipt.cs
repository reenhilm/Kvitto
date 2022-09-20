using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Core.Entities
{
    public class Receipt
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

        //Nav prop
        public ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
        //FK
        public int ReceiptTypeId { get; set; }
        public ReceiptType ReceiptType { get; set; } = default!;
    }
}
