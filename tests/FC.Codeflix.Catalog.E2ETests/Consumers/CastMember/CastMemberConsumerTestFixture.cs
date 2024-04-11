using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.CastMember;

public class CastMemberConsumerTestFixture : CastMemberTestFixtureBase
{
    private readonly KafkaConfiguration _kafkaConfiguration;

    public CastMemberConsumerTestFixture()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;
        // Wait a little for the consumer to be assigned a Partition, mainly in case of rebalacing
        Thread.Sleep(15_000);
    }

    public Task PublishMessageAsync(object message)
        => PublishMessageAsync(
            _kafkaConfiguration.CastMemberConsumer,
            message);

    public MessageModel<CastMemberPayloadModel> BuildValidMessage(string operation, CastMemberModel castMemberModel)
    {
        var message = new MessageModel<CastMemberPayloadModel>
        {
            Payload = new MessageModelPayload<CastMemberPayloadModel>
            {
                Op = operation
            }
        };
        var castMemberPayload = new CastMemberPayloadModel
        {
            Id = castMemberModel.Id,
            Name = castMemberModel.Name,
            Type = castMemberModel.Type,
            CreatedAt = castMemberModel.CreatedAt,
        };
        if (operation == "d")
        {
            message.Payload.Before = castMemberPayload;
        }
        else
        {
            message.Payload.After = castMemberPayload;
        }

        return message;
    }

    public MessageModel<CastMemberPayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, DataGenerator.GetCastMemberModelList(1)[0]);
}

[CollectionDefinition(nameof(CastMemberConsumerTestFixture))]
public class CastMemberConsumerTestFixtureCollection
    : ICollectionFixture<CastMemberConsumerTestFixture>
{
}