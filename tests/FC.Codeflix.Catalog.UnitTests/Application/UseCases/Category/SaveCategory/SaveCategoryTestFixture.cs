using FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SaveCategory;
public class SaveCategoryTestFixture 
    : CategoryTestFixture
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean());

    public SaveCategoryInput GetInvalidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(),
            null,
            GetValidCategoryDescription(),
            DateTime.Now,
            GetRandomBoolean());

}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection
    : ICollectionFixture<SaveCategoryTestFixture>
{ }
