using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = Faker.Commerce.ProductName();
        Action action = 
            () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        Action action =
            () => DomainValidation.NotNull(value, fieldName);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }

    [Theory(DisplayName = nameof(GuidNotNullOrEmptyThrowWhenNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    public void GuidNotNullOrEmptyThrowWhenNullOrEmpty(string? target)
    {
        Guid? value = target == null ? null : Guid.Empty;
        string fieldName = "Id";

        Action action =
            () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }


    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = 
            () => DomainValidation.NotNullOrEmpty(target, fieldName);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(NotNegativeOrZeroOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNegativeOrZeroOk()
    {
        int target = Faker.Random.Int(1);
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNegativeOrZero(target, fieldName);

        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(NotNegativeOrZeroThrowWhenZeroOrNegative))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNegativeOrZeroThrowWhenZeroOrNegative()
    {
        int target = Faker.Random.Int(-10, 0);
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNegativeOrZero(target, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be greater than 0");
    }
    
    [Fact(DisplayName = nameof(NotMinDateTimeOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotMinDateTimeOk()
    {
        DateTime target = Faker.Date.Recent();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotMinDateTime(target, fieldName);

        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(NotMinDateTimeThrowWhenMinDateTime))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotMinDateTimeThrowWhenMinDateTime()
    {
        DateTime target = DateTime.MinValue;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotMinDateTime(target, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be a valid date");
    }
}
