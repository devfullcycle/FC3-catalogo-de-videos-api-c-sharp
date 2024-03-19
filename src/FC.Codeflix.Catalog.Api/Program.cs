using FC.Codeflix.Catalog.Api.CastMembers;
using FC.Codeflix.Catalog.Api.Categories;
using FC.Codeflix.Catalog.Api.Filters;
using FC.Codeflix.Catalog.Api.Genres;
using FC.Codeflix.Catalog.Api.Videos;
using FC.Codeflix.Catalog.Application;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Messaging;
using FC.Codeflix.Catalog.Infra.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddUseCases()
    .AddMemoryCache()
    .AddHttpClients(builder.Configuration)
    .AddConsumers(builder.Configuration)
    .AddElasticSearch(builder.Configuration)
    .AddRepositories()
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<CategoryQueries>()
    .AddTypeExtension<CategoryMutations>()
    .AddTypeExtension<GenreQueries>()
    .AddTypeExtension<CastMemberQueries>()
    .AddTypeExtension<VideoQueries>()
    .AddErrorFilter<GraphQLErrorFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.MapControllers();

app.Run();

public partial class Program { }