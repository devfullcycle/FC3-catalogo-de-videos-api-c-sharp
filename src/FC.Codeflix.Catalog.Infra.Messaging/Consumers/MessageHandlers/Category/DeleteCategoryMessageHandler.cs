using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;

public class DeleteCategoryMessageHandler
    : IMessageHandler<CategoryPayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteCategoryMessageHandler> _logger;

    public DeleteCategoryMessageHandler(
        IMediator mediator,
        ILogger<DeleteCategoryMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(
        MessageModel<CategoryPayloadModel> messageModel,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleteInput = messageModel.Payload.Before.ToDeleteCategoryInput();
            await _mediator.Send(deleteInput, cancellationToken);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Category not found. Message: {@message}", messageModel);
        }
    }
}