using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;

public class SaveVideoInput : IRequest<VideoModelOutput>
{
    public SaveVideoInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}