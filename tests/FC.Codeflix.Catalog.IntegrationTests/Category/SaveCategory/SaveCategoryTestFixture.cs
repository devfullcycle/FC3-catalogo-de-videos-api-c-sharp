using FC.Codeflix.Catalog.Application.UseCases.Category.SaveCategory;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SaveCategory;
public class SaveCategoryTestFixture : CategoryTestFixture
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
