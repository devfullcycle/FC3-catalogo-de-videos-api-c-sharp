using FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.Common;
using NSubstitute;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenresByIds;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.Genre.GetGenresByIds;

[Collection(nameof(GenreUseCaseTestFixture))]
public class GetGenreByIdsTest
{
    private readonly GenreUseCaseTestFixture _fixture;

    public GetGenreByIdsTest(GenreUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGenresByIdsTest))]
    [Trait("Application", "[UseCase] GetGenresByIds")]
    public async Task GetGenresByIdsTest()
    {
        var repository = _fixture.GetMockRepository();
        var genres = _fixture.GetGenreList();
        var expectedOutput = genres
            .Select(genre => new
            {
                genre.Id,
                genre.Name,
                genre.CreatedAt,
                genre.IsActive,
                Categories = genre.Categories.Select(category => new { category.Id, category.Name })
            });
        var ids = expectedOutput.Select(x => x.Id);
        repository.GetGenresByIdsAsyc(
            Arg.Any<IEnumerable<Guid>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(genres));
        var useCase = new UseCase.GetGenresByIds(repository);
        var input = new GeGenresByIdsInput(ids);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().BeEquivalentTo(expectedOutput);
        await repository.Received(1).GetGenresByIdsAsyc(
            ids, Arg.Any<CancellationToken>());
    }
}