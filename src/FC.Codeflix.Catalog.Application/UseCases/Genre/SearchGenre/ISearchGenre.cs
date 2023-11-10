using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SearchGenre;

public interface ISearchGenre 
    : IRequestHandler<SearchGenreInput, SearchListOutput<GenreModelOutput>>
{
    
}