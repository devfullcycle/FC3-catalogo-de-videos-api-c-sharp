using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenreInput : IRequest<GenreModelOutput>
{
    public SaveGenreInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}