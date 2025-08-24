using System.ComponentModel.DataAnnotations;

namespace BackendApi.Validation
{
    public class ValidationHelper
    {
        public static List<string> ValidateObject<T>(T obj) where T : class
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return validationResults.Select(vr => vr.ErrorMessage ?? "Validation error").ToList();
        }
    }

    public class SKUAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string sku)
            {
                return !string.IsNullOrWhiteSpace(sku) && sku.Length <= 100;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a non-empty string with maximum 100 characters.";
        }
    }
}
