using FC.Codeflix.Catalog.Infra.Messaging.Common;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using MediatR;

namespace FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.CastMember;

public class SaveCastMemberMessageHandler
    : IMessageHandler<CastMemberPayloadModel>
{
    private readonly IMediator _mediator;

    public SaveCastMemberMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleMessageAsync(
        MessageModel<CastMemberPayloadModel> messageModel,
        CancellationToken cancellationToken)
    {
        var saveInput = messageModel.Payload.After.ToSaveCastMemberInput();
        await _mediator.Send(saveInput, cancellationToken);
    }
}