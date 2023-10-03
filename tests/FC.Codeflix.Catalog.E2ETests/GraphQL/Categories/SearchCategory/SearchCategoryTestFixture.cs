using RepositoryDTOs = FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.Common;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.E2ETests.GraphQL.Categories.SearchCategory;
public class SearchCategoryTestFixture : CategoryTestFixture
{
    public IList<CategoryModel> GetCategoryModelList(IEnumerable<string> categoryNames)
        => DataGenerator.GetCategoryModelList(categoryNames);

    public IList<CategoryModel> CloneCategoriesListOrdered(
        IList<CategoryModel> categoriesList,
        string orderBy,
        RepositoryDTOs.SearchOrder direction)
            => DataGenerator.CloneCategoriesListOrdered(categoriesList, orderBy, direction);
}

[CollectionDefinition(nameof(SearchCategoryTestFixture))]
public class SearchCategoryTestFixtureCollection
    : ICollectionFixture<SearchCategoryTestFixture>
{ }

