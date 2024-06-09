using System.ComponentModel.DataAnnotations;

namespace library_api.Validations
{
	public class FirstLetterUppercaseAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null || string.IsNullOrEmpty(value.ToString()))
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