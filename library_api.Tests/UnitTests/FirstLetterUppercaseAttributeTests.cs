using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.Tests.UnitTests
{
	[TestClass]
	public class FirstLetterUppercaseAttributeTests
	{
		[TestMethod]
		public void FailOnFirstLetterLowercaseAttribute()
		{
			FirstLetterUppercaseAttribute firstLetterUppercase = new FirstLetterUppercaseAttribute();
			string valueToTest = "edwin";
			ValidationContext validationContext = new ValidationContext(new { Name = valueToTest });

			ValidationResult testResult = firstLetterUppercase.GetValidationResult(valueToTest, validationContext);

			Assert.AreEqual("First letter must be on uppercase.", testResult?.ErrorMessage);
		}

		[TestMethod]
		public void SuccessOnFirstLetterUppercaseAttribute()
		{
			FirstLetterUppercaseAttribute firstLetterUppercase = new FirstLetterUppercaseAttribute();
			string valueToTest = "Edwin";
			ValidationContext validationContext = new ValidationContext(new { Name = valueToTest });

			ValidationResult testResult = firstLetterUppercase.GetValidationResult(valueToTest, validationContext);

			Assert.IsNull(testResult);
		}

		[TestMethod]
		public void SuccessOnNullFirstLetterUppercaseAttribute()
		{
			FirstLetterUppercaseAttribute firstLetterUppercase = new FirstLetterUppercaseAttribute();
			string valueToTest = null;
			ValidationContext validationContext = new ValidationContext(new { Name = valueToTest });

			ValidationResult testResult = firstLetterUppercase.GetValidationResult(valueToTest, validationContext);

			Assert.IsNull(testResult);
		}
	}
}