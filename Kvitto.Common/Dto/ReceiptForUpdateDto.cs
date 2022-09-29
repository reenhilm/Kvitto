using System.ComponentModel.DataAnnotations;

namespace Kvitto.Common.Dto
{
    public class ReceiptForUpdateDto : ReceiptForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
