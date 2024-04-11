using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Domain.ValueObjects;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;

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

    public List<VideoModel> GetVideoModelList(int count)
        => GetVideoList(count)
            .Select(VideoModel.FromEntity)
            .ToList();
    
    public List<VideoModel> GetVideoModelList(IEnumerable<string> titles)
        => titles
            .Select(title =>
            {
                var video = GetValidVideo();
                var model = VideoModel.FromEntity(video);
                model.Title = title;
                return model;
            })
            .ToList();

    
    public IList<VideoModel> CloneVideosListOrdered(
        List<VideoModel> examples, string orderBy, SearchOrder inputOrder)
    {
        var listClone = new List<VideoModel>(examples);
        var orderedEnumerable = (orderBy.ToLower(), inputOrder) switch
        {
            ("title", SearchOrder.Asc) => listClone.OrderBy(x => x.Title)
                .ThenBy(x => x.Id),
            ("title", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Title)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Title).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}