using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Gateways;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;

public class SaveVideo : ISaveVideo
{
    private readonly IVideoRepository _videoRepository;
    private readonly IAdminCatalogGateway _catalogGateway;

    public SaveVideo(IVideoRepository videoRepository, IAdminCatalogGateway catalogGateway)
    {
        _videoRepository = videoRepository;
        _catalogGateway = catalogGateway;
    }

    public async Task<VideoModelOutput> Handle(SaveVideoInput request, CancellationToken cancellationToken)
    {
        var video = await _catalogGateway.GetVideoAsync(request.Id, cancellationToken);
        await _videoRepository.SaveAsync(video, cancellationToken);
        return VideoModelOutput.FromVideo(video);
    }
}