using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FluentAssertions;
using NSubstitute;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.CastMember.SearchCastMember;

namespace FC.Codeflix.Catalog.UnitTests.Application.UseCases.CastMember.SearchCastMember;

[Collection(nameof(SearchCastMemberTestFixture))]
public class SearchCastMemberTest
{
    private readonly SearchCastMemberTestFixture _fixture;

    public SearchCastMemberTest(SearchCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnsSearchResult))]
    [Trait("Application", "[UseCase] SearchCastMember")]
    public async Task ReturnsSearchResult()
    {
        var repository = _fixture.GetMockRepository();
        var castMembers = _fixture.GetCastMemberList();
        var input = _fixture.GetSearchInput();
        var expectedQueryResult = new SearchOutput<DomainEntity.CastMember>(
            input.Page,
            input.PerPage,
            input.PerPage,
            castMembers);
        repository.SearchAsync(
            Arg.Any<SearchInput>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(expectedQueryResult));
        var useCase = new UseCase.SearchCastMember(repository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQueryResult.Total);
        output.Items.Should().BeEquivalentTo(castMembers);
        await repository.Received(1).SearchAsync(
            Arg.Is<SearchInput>(search =>
                search.Page == input.Page &&
                search.PerPage == input.PerPage &&
                search.Search == input.Search &&
                search.Order == input.Order &&
                search.OrderBy == input.OrderBy),
            Arg.Any<CancellationToken>());

    }
}