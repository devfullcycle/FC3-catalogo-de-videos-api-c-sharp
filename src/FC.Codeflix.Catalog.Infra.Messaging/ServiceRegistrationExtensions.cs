using FC.Codeflix.Catalog.Infra.Messaging.Configuration;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.CastMember;
using FC.Codeflix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Genre;
using FC.Codeflix.Catalog.Infra.Messaging.Extensions;
using FC.Codeflix.Catalog.Infra.Messaging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Messaging;

public static class ServiceRegistrationExtensions
{
    private const string KafkaConfigurationSection = "KafkaConfiguration";

    public static IServiceCollection AddConsumers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<KafkaConfiguration>()
            .BindConfiguration(KafkaConfigurationSection);

        var kafkaConfiguration = configuration.GetSection(KafkaConfigurationSection);

        return services
            .AddCategoryConsumers(kafkaConfiguration.GetSection(nameof(KafkaConfiguration.CategoryConsumer)))
            .AddGenreConsumers(
                kafkaConfiguration.GetSection(nameof(KafkaConfiguration.GenreConsumer)),
                kafkaConfiguration.GetSection(nameof(KafkaConfiguration.GenreCategoryConsumer)))
            .AddCastMemberConsumers(kafkaConfiguration.GetSection(nameof(KafkaConfiguration.CastMemberConsumer)));
    }

    private static IServiceCollection AddGenreConsumers(
        this IServiceCollection services,
        IConfigurationSection genreConfigurationSection,
        IConfigurationSection genreCategoryConfigurationSection)
    {
        return services
            .AddScoped(typeof(SaveGenreMessageHandler<>))
            .AddScoped<DeleteGenreMessageHandler>()
            .AddKafkaConsumer<GenrePayloadModel>()
            .Configure(genreConfigurationSection)
            .WithRetries()
            .With<SaveGenreMessageHandler<GenrePayloadModel>>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteGenreMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register()
            .AddKafkaConsumer<GenreCategoryPayloadModel>()
            .Configure(genreCategoryConfigurationSection)
            .WithRetries()
            .WithDefault<SaveGenreMessageHandler<GenreCategoryPayloadModel>>()
            .Register();
    }

    private static IServiceCollection AddCategoryConsumers(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        return services
            .AddScoped<SaveCategoryMessageHandler>()
            .AddScoped<DeleteCategoryMessageHandler>()
            .AddKafkaConsumer<CategoryPayloadModel>()
            .Configure(configurationSection)
            .WithRetries()
            .With<SaveCategoryMessageHandler>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteCategoryMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register();
    }

    private static IServiceCollection AddCastMemberConsumers(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        return services
            .AddScoped<SaveCastMemberMessageHandler>()
            .AddScoped<DeleteCastMemberMessageHandler>()
            .AddKafkaConsumer<CastMemberPayloadModel>()
            .Configure(configurationSection)
            .WithRetries()
            .With<SaveCastMemberMessageHandler>()
            .When(message => message.Payload.Operation is
                MessageModelOperation.Create or
                MessageModelOperation.Read or
                MessageModelOperation.Update)
            .And
            .With<DeleteCastMemberMessageHandler>()
            .When(message => message.Payload.Operation is MessageModelOperation.Delete)
            .Register();
    }
}