using System.ComponentModel.DataAnnotations;

namespace library_api.Validations
{
	public class FirstLetterCapitalized : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (string.IsNullOrEmpty(value.ToString()) || value == null)
			{
				return ValidationResult.Success;
			}

			string firstLetter = value.ToString()[0].ToString();
			if (firstLetter != firstLetter.ToUpper())
			{
				return new ValidationResult("First letter must be on uppercase.");
			}

			return ValidationResult.Success;
		}
	}
}