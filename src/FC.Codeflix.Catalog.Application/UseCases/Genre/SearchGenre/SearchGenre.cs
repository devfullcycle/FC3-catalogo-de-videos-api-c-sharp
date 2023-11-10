using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;

public class SearchGenre : ISearchGenre
{
    private readonly IGenreRepository _repository;

    public SearchGenre(IGenreRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<SearchListOutput<GenreModelOutput>> Handle(
        SearchGenreInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var genres = await _repository.SearchAsync(searchInput, cancellationToken);
        return new SearchListOutput<GenreModelOutput>(
            genres.CurrentPage,
            genres.PerPage,
            genres.Total,
            genres.Items.Select(GenreModelOutput.FromGenre).ToList());
    }
}