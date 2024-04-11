using FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Video;

public class DeleteVideoMessageHandler: IMessageHandler<VideoPayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteVideoMessageHandler> _logger;

    public DeleteVideoMessageHandler(IMediator mediator, ILogger<DeleteVideoMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(
        MessageModel<VideoPayloadModel> messageModel,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = messageModel.Payload.Before.Id;
            var input = new DeleteVideoInput(id);
            await _mediator.Send(input, cancellationToken);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Video not found. Message: {@message}", messageModel);
        }
    }
}