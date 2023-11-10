using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;

public interface ISaveGenre
    : IRequestHandler<SaveGenreInput, GenreModelOutput>
{
    
}