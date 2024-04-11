using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.ValueObjects;

namespace FC.Codeflix.Catalog.Tests.Shared;

public class VideoDataGenerator : DataGeneratorBase
{
    public Video GetValidVideo(Guid? id = null)
        => new(id ?? Guid.NewGuid(),
            GetValidTitle(),
            GetValidDescription(),
            GetValidYearLaunched(),
            GetValidDuration(),
            DateTime.Now.Date,
            GetRandomRating(), 
            GetValidMedias());

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();
    
    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(
            new DateOnly(1960, 1, 1),
            new DateOnly(2022, 1, 1)
        ).Year;

    public int GetValidDuration()
        => (new Random()).Next(100, 300);
    
    public string GetValidUrl()
        => Faker.Internet.UrlWithPath();
    
    public Rating GetRandomRating()
    {
        var enumValue = Enum.GetValues<Rating>();
        var random = new Random();
        return enumValue[random.Next(enumValue.Length)];
    }
    
    public Medias GetValidMedias()
        => new(
            GetValidUrl(),
            GetValidUrl(),
            GetValidUrl(),
            GetValidUrl(),
            GetValidUrl()
        );

    public IList<Video> GetVideoList(int count)
        => Enumerable.Range(0, count)
            .Select(_ => GetValidVideo())
            .ToList();
}