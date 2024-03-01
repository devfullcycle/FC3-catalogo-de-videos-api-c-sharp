using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SearchVideo;

public interface ISearchVideo
    : IRequestHandler<SearchVideoInput, SearchListOutput<VideoModelOutput>>
{
    
}