using FC.Codeflix.Catalog.Api.Common;
using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;

namespace FC.Codeflix.Catalog.Api.Videos;

public class SearchVideoPayload: SearchPayload<VideoPayload>
{
    public static SearchVideoPayload FromSearchListOutput(
        SearchListOutput<VideoModelOutput> output)
        => new()
        {
            CurrentPage = output.CurrentPage,
            PerPage = output.PerPage,
            Total = output.Total,
            Items = output.Items.Select(VideoPayload.FromVideoModelOutput).ToList()
        };
}