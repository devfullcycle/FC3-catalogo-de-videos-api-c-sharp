namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Video;

[Collection(nameof(VideoTestFixture))]
public class VideoTest
{
    private readonly VideoTestFixture _fixture;

    public VideoTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenValidArguments_Instantiate))]
    [Trait("Domain", "Video - Aggregates")]
    public void Constructor_WhenValidArguments_Instantiate()
    {
        var example = _fixture.GetValidVideo();

        var video = new Catalog.Domain.Entity.Video(
            example.Id,
            example.Title,
            example.Description,
            example.YearLaunched,
            example.Duration,
            example.CreatedAt,
            example.Rating,
            example.Medias);

        video.Id.Should().Be(example.Id);
        video.Title.Should().Be(example.Title);
        video.Description.Should().Be(example.Description);
        video.YearLaunched.Should().Be(example.YearLaunched);
        video.Duration.Should().Be(example.Duration);
        video.CreatedAt.Should().Be(example.CreatedAt);
        video.CreatedAt.Should().Be(example.CreatedAt);
        video.Categories.Should().BeEquivalentTo(example.Categories);
        video.Genres.Should().BeEquivalentTo(example.Genres);
        video.CastMembers.Should().BeEquivalentTo(example.CastMembers);
    }
    
    [Theory(DisplayName = nameof(Constructor_WhenInvalidArguments_ThrowEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    [InlineData(null, "Description", 2021, 90, "2021-01-01", Rating.Rate10, "Title should not be empty or null")]
    [InlineData("Title", null, 2021, 90, "2021-01-01", Rating.Rate10, "Description should not be empty or null")]
    [InlineData("Title", "Description", 0, 90, "2021-01-01", Rating.Rate10, "YearLaunched should be greater than 0")]
    [InlineData("Title", "Description", 2021, 0, "2021-01-01", Rating.Rate10, "Duration should be greater than 0")]
    [InlineData("Title", "Description", 2021, 90, "0001-01-01", Rating.Rate10, "CreatedAt should be a valid date")]
    [InlineData("Title", "Description", 2021, 90, "2021-01-01", (Rating)99, "Rating is not a valid Rating")]
    public void Constructor_WhenInvalidArguments_ThrowEntityValidationException(
        string title, string description, int yearLaunched, int duration, string createdAt, Rating rating,
        string message)
    {
        var example = _fixture.GetValidVideo();

        var action = () => new Catalog.Domain.Entity.Video(
            example.Id,
            title,
            description,
            yearLaunched,
            duration,
            DateTime.Parse(createdAt),
            rating,
            example.Medias);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage(message);
    }
    
    [Fact(DisplayName = nameof(Constructor_WhenMediasIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    public void Constructor_WhenMediasIsNull_ThrowsEntityValidationException()
    {
        var example = _fixture.GetValidVideo();

        var action = () => new Catalog.Domain.Entity.Video(
            example.Id,
            example.Title,
            example.Description,
            example.YearLaunched,
            example.Duration,
            example.CreatedAt,
            example.Rating,
            null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Medias should not be null");
    }
    
    [Fact(DisplayName = nameof(AddCategories_WhenCategoriesIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - AddCategories_WhenCategoriesIsNotNull_AddItems")]
    public void AddCategories_WhenCategoriesIsNotNull_AddItems()
    {
        var video = _fixture.GetValidVideo();
        var categories = new[]
        {
            _fixture.CategoryDataGenerator.GetValidCategory(),
            _fixture.CategoryDataGenerator.GetValidCategory(),
        };

        video.AddCategories(categories);

        video.Categories.Should().BeEquivalentTo(categories);
    }

    
    [Fact(DisplayName = nameof(AddCategories_WhenCategoriesIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    public void AddCategories_WhenCategoriesIsNull_ThrowsEntityValidationException()
    {
        var video = _fixture.GetValidVideo();

        var action = () => video.AddCategories(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("categories should not be null");
    }
    
    [Fact(DisplayName = nameof(AddGenres_WhenGenresIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    public void AddGenres_WhenGenresIsNotNull_AddItems()
    {
        var video = _fixture.GetValidVideo();
        var genres = new[]
        {
            _fixture.GenreDataGenerator.GetValidGenre(),
            _fixture.GenreDataGenerator.GetValidGenre(),
        };

        video.AddGenres(genres);

        video.Genres.Should().BeEquivalentTo(genres);
    }
    
    [Fact(DisplayName = nameof(AddGenres_WhenGenresIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    public void AddGenres_WhenGenresIsNull_ThrowsEntityValidationException()
    {
        var video = _fixture.GetValidVideo();

        var action = () => video.AddGenres(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("genres should not be null");
    }
    
    [Fact(DisplayName = nameof(AddCastMembers_WhenCastMembersIsNotNull_AddItems))]
    [Trait("Domain", "Video - Aggregates")]
    public void AddCastMembers_WhenCastMembersIsNotNull_AddItems()
    {
        var video = _fixture.GetValidVideo();
        var castMembers = new[]
        {
            _fixture.CastMemberDataGenerator.GetValidCastMember(),
            _fixture.CastMemberDataGenerator.GetValidCastMember(),
        };

        video.AddCastMembers(castMembers);

        video.CastMembers.Should().BeEquivalentTo(castMembers);
    }
    
    [Fact(DisplayName = nameof(AddCastMembers_WhenCastMembersIsNull_ThrowsEntityValidationException))]
    [Trait("Domain", "Video - Aggregates")]
    public void AddCastMembers_WhenCastMembersIsNull_ThrowsEntityValidationException()
    {
        var video = _fixture.GetValidVideo();

        var action = () => video.AddCastMembers(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("castMembers should not be null");
    }
    
}