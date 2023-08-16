using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Category.SaveCategory;
public class SaveCategoryTestFixture 
    : CategoryUseCaseFixture
{
    public SaveCategoryInput GetValidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            DateTime.Now,
            GetRandomBoolean());

    public SaveCategoryInput GetInvalidInput()
        => new SaveCategoryInput(
            Guid.NewGuid(),
            null,
            GetValidDescription(),
            DateTime.Now,
            GetRandomBoolean());

}

[CollectionDefinition(nameof(SaveCategoryTestFixture))]
public class SaveCategoryTestFixtureCollection
    : ICollectionFixture<SaveCategoryTestFixture>
{ }
