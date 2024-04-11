using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;

public interface ISaveVideo : IRequestHandler<SaveVideoInput, VideoModelOutput>
{
    
}