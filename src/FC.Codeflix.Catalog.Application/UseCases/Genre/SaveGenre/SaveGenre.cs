using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Domain.Repositories;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenre : ISaveGenre
{
    private readonly IGenreRepository _repository;
    private readonly IAdminCatalogGateway _gateway;

    public SaveGenre(
        IGenreRepository repository,
        IAdminCatalogGateway gateway)
    {
        _repository = repository;
        _gateway = gateway;
    }
    
    public async Task<GenreModelOutput> Handle(SaveGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _gateway.GetGenreAsync(request.Id, cancellationToken);
        await _repository.SaveAsync(genre, cancellationToken);
        return GenreModelOutput.FromGenre(genre);
    }
}