using FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

namespace FC.Codeflix.Catalog.Tests.Shared;

public class GenreDataGenerator : DataGeneratorBase
{
    private readonly CategoryDataGenerator _categoryDataGenerator = new();
    
    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public Genre GetValidGenre()
    {
        var categories = new[]
        {
            _categoryDataGenerator.GetValidCategory(),
            _categoryDataGenerator.GetValidCategory()
        };
        var genre = new Genre(
            Guid.NewGuid(),
            GetValidName(),
            GetRandomBoolean(),
            DateTime.Now,
            categories);

        return genre;
    }

    public List<GenreModel> GetGenreModelList(int count = 10)
        => Enumerable.Range(0, count)
            .Select(_ =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                return GenreModel.FromEntity(GetValidGenre());
            }).ToList();
    
    public SaveGenreInput GetValidSaveGenreInput()
    {
        var genre = GetValidGenre();
        return new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name)));
    }

    public SaveGenreInput GetInvalidSaveGenreInput()
    {
        var genre = GetValidGenre();
        return new(
            genre.Id,
            null!,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(item => new SaveGenreInputCategory(item.Id, item.Name)));
    }
}