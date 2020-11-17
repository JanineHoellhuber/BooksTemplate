using Books.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Books.Core.Validations
{
  public class IsbnValidation : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var isbn = (string)value;

            if (Book.CheckIsbn(isbn))
            {
                return ValidationResult.Success;

            }
          
                return new ValidationResult($"Isbn {isbn} ist ungültig", new List<string>{"Isbn"});

            

        }
  }
}
