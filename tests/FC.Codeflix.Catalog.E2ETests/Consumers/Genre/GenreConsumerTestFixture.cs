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

    public Task PublishMessageAsync<T>(MessageModel<T> message) where T : GenrePayloadModel
        => PublishMessageAsync(
            typeof(T) == typeof(GenreCategoryPayloadModel)
                ? _kafkaConfiguration.GenreCategoryConsumer
                : _kafkaConfiguration.GenreConsumer,
            message);

    public MessageModel<T> BuildValidMessage<T>(string operation, GenreModel genreModel) where T : GenrePayloadModel
    {
        var message = new MessageModel<T>
        {
            Payload = new MessageModelPayload<T>
            {
                Op = operation
            }
        };
        dynamic genrePayload;
        if (typeof(T) == typeof(GenreCategoryPayloadModel))
        {
            genrePayload = new GenreCategoryPayloadModel
            {
                Id = genreModel.Id
            };
        }
        else
        {
            genrePayload = new GenrePayloadModel
            {
                Id = genreModel.Id
            };
        }

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

    public MessageModel<T> BuildValidMessage<T>(string operation) where T : GenrePayloadModel
        => BuildValidMessage<T>(operation, DataGenerator.GetGenreModelList(1)[0]);

    public Domain.Entity.Genre GetValidGenre(Guid id) => DataGenerator.GetValidGenre(id);
}

[CollectionDefinition(nameof(GenreConsumerTestFixture))]
public class GenreConsumerTestFixtureCollection
    : ICollectionFixture<GenreConsumerTestFixture>
{
}