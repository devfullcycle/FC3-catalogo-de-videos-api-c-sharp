using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public class GenreDataGenerator : DataGeneratorBase
{
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    
    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public Genre GetValidGenre(Guid? id = null)
    {
        var categories = new[]
        {
            _categoryDataGenerator.GetValidCategory(),
            _categoryDataGenerator.GetValidCategory()
        };
        var genre = new Genre(
            id ?? Guid.NewGuid(),
            GetValidName(),
            GetRandomBoolean(),
            DateTime.Now,
            categories);

        return genre;
    }
    
    public List<Genre> GetGenreList(int length = 10)
        => Enumerable
            .Range(0, length)
            .Select(_ => GetValidGenre())
            .ToList();

    public List<GenreModel> GetGenreModelList(int count = 10)
        => Enumerable.Range(0, count)
            .Select(_ =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                return GenreModel.FromEntity(GetValidGenre());
            }).ToList();
    
    
    public List<GenreModel> GetGenreModelList(IEnumerable<string> names)
        => names
            .Select(name =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                var genre = GenreModel.FromEntity(GetValidGenre());
                genre.Name = name;
                return genre;
            }).ToList();

    public IList<GenreModel> CloneGenresListOrdered(List<GenreModel> genreList, string orderBy, SearchOrder inputOrder)
    {
        var listClone = new List<GenreModel>(genreList);
        var orderedEnumerable = (orderBy.ToLower(), inputOrder) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}