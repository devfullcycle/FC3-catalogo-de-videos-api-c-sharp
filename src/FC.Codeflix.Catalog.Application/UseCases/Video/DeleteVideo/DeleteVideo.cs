using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;

public class DeleteVideo : IDeleteVideo
{
    private readonly IVideoRepository _videoRepository;

    public DeleteVideo(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task Handle(DeleteVideoInput request, CancellationToken cancellationToken)
        => await _videoRepository.DeleteAsync(request.Id, cancellationToken);
}