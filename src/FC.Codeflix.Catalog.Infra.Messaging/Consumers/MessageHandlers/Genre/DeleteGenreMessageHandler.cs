using FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Genre;

public class DeleteGenreMessageHandler
    : IMessageHandler<GenrePayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteGenreMessageHandler> _logger;

    public DeleteGenreMessageHandler(IMediator mediator, ILogger<DeleteGenreMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(MessageModel<GenrePayloadModel> messageModel, CancellationToken cancellationToken)
    {
        try
        {
            var id = messageModel.Payload.Before.Id;
            var input = new DeleteGenreInput(id);
            await _mediator.Send(input, cancellationToken);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Genre not found. Message: {@message}", messageModel);
        }
    }
}