using Kvitto.Common.Dto;
using System.ComponentModel.DataAnnotations;

namespace Kvitto.Common.ValidationAttributes
{
    public class ReceiptMustBeUploadedAfterPurchaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var receipt = (ReceiptForManipulationDto)validationContext.ObjectInstance;

            if (receipt.UploadedDate > receipt.PurchaseDate)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(ReceiptForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
