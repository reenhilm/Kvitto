using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Common.Dto
{
    public class UploadedFileDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ContentType { get; set; } = default!;
    }
}
