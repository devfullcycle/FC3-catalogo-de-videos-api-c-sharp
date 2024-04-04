using FC.Codeflix.Catalog.Application.UseCases.Video.SaveVideo;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Video;

public class SaveVideoMessageHandler<T>
    : IMessageHandler<T> where T : VideoPayloadModel
{
    private readonly IMediator _mediator;

    public SaveVideoMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleMessageAsync(MessageModel<T> messageModel, CancellationToken cancellationToken)
    {
        var id = messageModel.Payload.After.Id;
        var input = new SaveVideoInput(id);
        await _mediator.Send(input, cancellationToken);
    }
}