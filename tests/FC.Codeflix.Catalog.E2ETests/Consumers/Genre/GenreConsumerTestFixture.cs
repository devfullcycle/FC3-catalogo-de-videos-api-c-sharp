using System.Text.Json;
using Confluent.Kafka;
using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Genre;

public class GenreConsumerTestFixture : GenreTestFixtureBase
{
    private readonly KafkaConfiguration _kafkaConfiguration;

    public GenreConsumerTestFixture()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;
        // Wait a little for the consumer to be assigned a Partition, mainly in case of rebalacing
        Thread.Sleep(15_000);
    }

    public Task PublishMessageAsync(object message)
        => PublishMessageAsync(
            _kafkaConfiguration.GenreConsumer.BootstrapServers,
            _kafkaConfiguration.GenreConsumer.Topic,
            message);

    public MessageModel<GenrePayloadModel> BuildValidMessage(string operation, GenreModel genreModel)
    {
        var message = new MessageModel<GenrePayloadModel>
        {
            Payload = new MessageModelPayload<GenrePayloadModel>
            {
                Op = operation
            }
        };
        var genrePayload = new GenrePayloadModel
        {
            Id = genreModel.Id
        };
        if (operation == "d")
        {
            message.Payload.Before = genrePayload;
        }
        else
        {
            message.Payload.After = genrePayload;
        }

        return message;
    }
    
    public MessageModel<GenrePayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, DataGenerator.GetGenreModelList(1)[0]);
}

[CollectionDefinition(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTestFixtureCollection
    : ICollectionFixture<GenreConsumerTestFixture>
{ }