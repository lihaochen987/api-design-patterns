using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace backend.Shared;

public class RecursiveValidator
{
    public List<ValidationResult> Validate(object obj)
    {
        var validationResults = new List<ValidationResult>();

        ValidateObject(obj, string.Empty, validationResults);
        return validationResults;
    }

    private void ValidateObject(object? obj, string propertyPath, List<ValidationResult> validationResults)
    {
        if (obj == null) return;

        // Validate the object itself using ValidationContext
        var objValidationResults = new List<ValidationResult>();
        var objValidationContext = new ValidationContext(obj);

        Validator.TryValidateObject(obj, objValidationContext, objValidationResults, validateAllProperties: true);

        // Add property path to validation results
        foreach (var result in objValidationResults)
        {
            var memberNames = result.MemberNames.Select(memberName =>
                string.IsNullOrEmpty(propertyPath) ? memberName : $"{propertyPath}.{memberName}");

            validationResults.Add(new ValidationResult(
                result.ErrorMessage,
                memberNames
            ));
        }

        // Get all properties of the object
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            object? value = property.GetValue(obj);
            string currentPath = string.IsNullOrEmpty(propertyPath)
                ? property.Name
                : $"{propertyPath}.{property.Name}";

            // Skip if value is null or it's a primitive type
            if (value == null || IsPrimitive(property.PropertyType))
                continue;

            // Handle collections
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) &&
                property.PropertyType != typeof(string))
            {
                int index = 0;
                foreach (object? item in (IEnumerable)value)
                {
                    if (item != null && !IsPrimitive(item.GetType()))
                    {
                        ValidateObject(item, $"{currentPath}[{index}]", validationResults);
                    }

                    index++;
                }
            }
            // Handle complex objects
            else
            {
                ValidateObject(value, currentPath, validationResults);
            }
        }
    }

    private bool IsPrimitive(Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(Guid) ||
               type.IsEnum;
    }
}
