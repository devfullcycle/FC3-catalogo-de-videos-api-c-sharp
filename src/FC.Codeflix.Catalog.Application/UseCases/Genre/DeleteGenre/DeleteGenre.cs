using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;

public class DeleteGenre : IDeleteGenre
{
    private readonly IGenreRepository _repository;

    public DeleteGenre(IGenreRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteGenreInput request, CancellationToken cancellationToken)
        => await _repository.DeleteAsync(request.Id, cancellationToken);
}