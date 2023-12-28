using System.Text.Json;
using Confluent.Kafka;
using FC.Codeflix.Catalog.E2ETests.Base.Fixture;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Category;

public class CategoryConsumerTestFixture : CategoryTestFixtureBase
{
    private readonly KafkaConfiguration _kafkaConfiguration;

    public CategoryConsumerTestFixture()
    {
        _kafkaConfiguration = WebAppFactory.Services.GetRequiredService<IOptions<KafkaConfiguration>>().Value;
        // Wait a little for the consumer to be assigned a Partition, mainly in case of rebalacing
        Thread.Sleep(15_000);
    }

    public async Task PublishMessageAsync(object message)
    {
        var config = new ProducerConfig { BootstrapServers = _kafkaConfiguration.CategoryConsumer.BootstrapServers };
        using var producer = new ProducerBuilder<string, string>(config).Build();
        var rawMessage = JsonSerializer.Serialize(message, SerializerConfiguration.JsonSerializerOptions);
        _ = await producer.ProduceAsync(
            _kafkaConfiguration.CategoryConsumer.Topic,
            new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = rawMessage
            });
    }

    public MessageModel<CategoryPayloadModel> BuildValidMessage(string operation, CategoryModel categoryModel)
    {
        var message = new MessageModel<CategoryPayloadModel>
        {
            Payload = new MessageModelPayload<CategoryPayloadModel>
            {
                Op = operation
            }
        };
        var categoryPayload = new CategoryPayloadModel
        {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Description = categoryModel.Description,
            CreatedAt = categoryModel.CreatedAt,
            IsActive = categoryModel.IsActive
        };
        if (operation == "d")
        {
            message.Payload.Before = categoryPayload;
        }
        else
        {
            message.Payload.After = categoryPayload;
        }

        return message;
    }
    
    public MessageModel<CategoryPayloadModel> BuildValidMessage(string operation)
        => BuildValidMessage(operation, DataGenerator.GetCategoryModelList(1)[0]);
}

[CollectionDefinition(nameof(CategoryConsumerTestFixture))]
public class CategoryConsumerTestFixtureCollection
    : ICollectionFixture<CategoryConsumerTestFixture>
{ }