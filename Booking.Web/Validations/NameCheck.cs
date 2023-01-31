using Booking.Data.Data;
using Booking.Core.Entities;
using System.ComponentModel.DataAnnotations;
using Booking.Web.Areas.Identity.Pages.Account;

namespace Booking.Web.Validations
{
    public class NameCheck : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            
            if (value is string input)
            {
                var inputModel = validationContext.ObjectInstance as RegisterModel.InputModel;

                if (inputModel != null)
                {
                    if (inputModel.FirstName != input)  // Efternamnet får ej vara samma som förnamnet
                    {
                        return ValidationResult.Success;
                    }
                }
            }

         return new ValidationResult(ErrorMessage);
        }
    }
}
