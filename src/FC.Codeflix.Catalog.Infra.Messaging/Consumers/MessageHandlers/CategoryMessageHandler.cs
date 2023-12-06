using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;

public class CategoryMessageHandler : IMessageHandler<CategoryPayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CategoryMessageHandler> _logger;

    public CategoryMessageHandler(
        IMediator mediator,
        ILogger<CategoryMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(
        MessageModel<CategoryPayloadModel> messageModel,
        CancellationToken cancellationToken)
    {
        switch (messageModel!.Payload.Operation)
        {
            case MessageModelOperation.Create:
            case MessageModelOperation.Read:
            case MessageModelOperation.Update:
                var saveInput = messageModel.Payload.After.ToSaveCategoryInput();
                await _mediator.Send(saveInput, cancellationToken);
                break;
            case MessageModelOperation.Delete:
                try
                {
                    var deleteInput = messageModel.Payload.Before.ToDeleteCategoryInput();
                    await _mediator.Send(deleteInput, cancellationToken);
                }
                catch (NotFoundException ex)
                {
                    _logger.LogError(ex, "Category not found. Message: {@message}", messageModel);
                }
                break;
            default:
                _logger.LogError("Invalid operation: {operation}", messageModel.Payload.Op);
                break;
        }
    }
}