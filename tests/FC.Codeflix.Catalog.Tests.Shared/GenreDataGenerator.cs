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
}