using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.SaveGenre;

public class SaveGenreInput : IRequest<GenreModelOutput>
{
    public SaveGenreInput(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt,
        IEnumerable<SaveGenreInputCategory> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IEnumerable<SaveGenreInputCategory> Categories { get; private set; }
}

public class SaveGenreInputCategory
{
    public SaveGenreInputCategory(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
}