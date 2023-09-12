using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Category.SearchCategory;
public class SearchCategoryTestFixture : CategoryTestFixture
{
    public IList<CategoryModel> GetCategoryModelList(IEnumerable<string> categoryNames)
        => categoryNames.Select(name =>
        {
            var category = CategoryModel.FromEntity(GetValidCategory());
            category.Name = name;
            return category;
        }).ToList();
}

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection
    : ICollectionFixture<SearchCategoryTestFixture>
{ }

