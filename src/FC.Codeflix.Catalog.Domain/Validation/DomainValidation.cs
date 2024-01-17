using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException(
                $"{fieldName} should not be null");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
            throw new EntityValidationException(
                $"{fieldName} should not be empty or null");
    }

    public static void NotNullOrEmpty(Guid? target, string fieldName)
    {
        if (target == null || target.Value == Guid.Empty)
            throw new EntityValidationException(
                $"{fieldName} should not be empty or null");
    }

    public static void IsDefined<T>(T target, string fieldName)
        where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), target))
        {
            throw new EntityValidationException(
                $"{fieldName} is not a valid {typeof(T).Name}");
        }
    }
}
