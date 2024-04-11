using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;

public class DeleteVideoInput : IRequest
{
    public DeleteVideoInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}