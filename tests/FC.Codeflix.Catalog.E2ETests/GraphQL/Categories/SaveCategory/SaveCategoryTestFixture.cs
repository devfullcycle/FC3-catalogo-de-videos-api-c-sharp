using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SaveCategory;
public class SaveCategoryTestFixture : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name =  DataGenerator.GetValidCategoryName(),
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };

    public SaveCategoryInput GetInvalidInput()
        => new SaveCategoryInput
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Description = DataGenerator.GetValidCategoryDescription(),
            CreatedAt = DateTime.UtcNow.Date,
            IsActive = DataGenerator.GetRandomBoolean()
        };
}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection
    : ICollectionFixture<SaveCategoryTestFixture>
{ }

