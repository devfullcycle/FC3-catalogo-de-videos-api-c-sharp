using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Repositories;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;

public class SearchVideo : ISearchVideo
{
    private readonly IVideoRepository _videoRepository;

    public SearchVideo(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<SearchListOutput<VideoModelOutput>> Handle(SearchVideoInput request, CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var videos = await _videoRepository.SearchAsync(searchInput, cancellationToken);
        return new SearchListOutput<VideoModelOutput>(
            videos.CurrentPage,
            videos.PerPage,
            videos.Total,
            videos.Items.Select(VideoModelOutput.FromVideo).ToList());
    }
}