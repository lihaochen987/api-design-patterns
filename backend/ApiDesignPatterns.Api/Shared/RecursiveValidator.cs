using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace backend.Shared;

/// <summary>
/// Provides recursive validation for complex objects, including nested properties and collections.
/// Validates objects using DataAnnotations and tracks the full property path for validation results.
/// </summary>
/// <remarks>
/// The validator:
/// - Recursively validates nested objects and collections
/// - Maintains property paths for clear error identification
/// - Handles null values and primitive types appropriately
/// - Supports standard DataAnnotations validation attributes
/// </remarks>
/// <example>
/// Here's an example using nested objects:
/// <code>
/// public class Address
/// {
///     [Required]
///     public string Street { get; set; }
///
///     [Required]
///     public string City { get; set; }
/// }
///
/// public class Person
/// {
///     [Required]
///     public string Name { get; set; }
///
///     [Range(0, 150)]
///     public int Age { get; set; }
///
///     public Address HomeAddress { get; set; }
///
///     public List&lt;Address&gt; AlternateAddresses { get; set; }
/// }
///
/// // Usage example:
/// var validator = new RecursiveValidator();
/// var person = new Person
/// {
///     Name = "",  // Invalid
///     Age = 200,  // Invalid
///     HomeAddress = new Address
///     {
///         Street = "",  // Invalid
///         City = null   // Invalid
///     },
///     AlternateAddresses = new List&lt;Address&gt;
///     {
///         new Address { Street = "123 Main St", City = "" }  // City invalid
///     }
/// };
///
/// var results = validator.Validate(person);
/// // Results will contain:
/// // - "Name" : "The Name field is required"
/// // - "Age" : "Age must be between 0 and 150"
/// // - "HomeAddress.Street" : "The Street field is required"
/// // - "HomeAddress.City" : "The City field is required"
/// // - "AlternateAddresses[0].City" : "The City field is required"
/// </code>
/// </example>
public class RecursiveValidator
{
    /// <summary>
    /// Validates an object and all its nested properties recursively.
    /// </summary>
    /// <param name="obj">The object to validate.</param>
    /// <returns>A list of ValidationResult objects containing any validation errors found.</returns>
    /// <example>
    /// <code>
    /// var validator = new RecursiveValidator();
    ///
    /// // Simple object validation
    /// var person = new Person { Name = "" };
    /// var results = validator.Validate(person);
    /// // Results: ["The Name field is required"]
    ///
    /// // Collection validation
    /// var department = new Department
    /// {
    ///     Name = "HR",
    ///     Employees = new[]
    ///     {
    ///         new Person { Name = "" },
    ///         new Person { Name = "John", Age = 200 }
    ///     }
    /// };
    /// var results = validator.Validate(department);
    /// // Results:
    /// // - "Employees[0].Name": "The Name field is required"
    /// // - "Employees[1].Age": "Age must be between 0 and 150"
    /// </code>
    /// </example>
    public List<ValidationResult> Validate(object obj)
    {
        var validationResults = new List<ValidationResult>();

        ValidateObject(obj, string.Empty, validationResults);
        return validationResults;
    }


    /// <summary>
    /// Recursively validates an object and its properties, tracking the property path.
    /// </summary>
    /// <param name="obj">The object to validate.</param>
    /// <param name="propertyPath">The current property path for nested validation.</param>
    /// <param name="validationResults">The list to store validation results.</param>
    /// <remarks>
    /// This method handles:
    /// - Direct object validation using DataAnnotations
    /// - Collection property validation with indexed paths
    /// - Complex object property validation with nested paths
    /// - Null checking and primitive type detection
    /// </remarks>
    private static void ValidateObject(object? obj, string propertyPath, List<ValidationResult> validationResults)
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

    /// <summary>
    /// Determines if a type should be treated as primitive (non-recursive).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type should be treated as primitive, false otherwise.</returns>
    /// <remarks>
    /// Primitive types include:
    /// - .NET primitive types
    /// - String
    /// - Decimal
    /// - DateTime
    /// - Guid
    /// - Enums
    /// </remarks>
    private static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(Guid) ||
               type.IsEnum;
    }
}
