using Elasticsearch.Net;
using FC.Codeflix.Catalog.Infra.Data.ES;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using Nest;

namespace FC.Codeflix.Catalog.Tests.Shared;
public static class ElasticSearchExtensions
{
    public static async Task CreateGenreIndexAsync(this IElasticClient elasticClient)
    {
        await elasticClient.DeleteIndexAsync(ElasticsearchIndices.Genre);
        _ = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.Genre, c => c
            .Map<GenreModel>(m => m
                .Properties(ps => ps
                    .Keyword(k => k
                        .Name(genre => genre.Id)
                    )
                    .Date(d => d
                        .Name(genre => genre.CreatedAt)
                    )
                    .Boolean(b => b
                        .Name(genre => genre.IsActive)
                    )
                    .Text(t => t
                        .Name(genre => genre.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(genre => genre.Name.Suffix("keyword"))
                            )
                        )
                    )
                    .Nested<GenreCategoryModel>(n => n
                        .Properties(pss => pss
                            .Keyword(k => k
                                .Name(category => category.Id)
                            )
                            .Keyword(k => k
                                .Name(category => category.Name)
                            )
                        )
                    )
                )
            )    
        );
    }
    public static async Task CreateCategoryIndexAsync(this IElasticClient elasticClient)
    {
        await elasticClient.DeleteIndexAsync(ElasticsearchIndices.Category);
        _ = await elasticClient.Indices.CreateAsync(ElasticsearchIndices.Category, c => c
            .Map<CategoryModel>(m => m
                .Properties(ps => ps
                    .Keyword(t => t
                        .Name(category => category.Id)
                    )
                    .Text(t => t
                        .Name(category => category.Name)
                        .Fields(fs => fs
                            .Keyword(k => k
                                .Name(category => category.Name.Suffix("keyword")))
                        )
                    )
                    .Text(t => t
                        .Name(category => category.Description)
                    )
                    .Boolean(b => b
                        .Name(category => category.IsActive)
                    )
                    .Date(d => d
                        .Name(category => category.CreatedAt)
                    )
                )
            )
        );
    }

    private static async Task DeleteIndexAsync(this IElasticClient elasticClient, string indexName)
    {
        var existsResponse = await elasticClient.Indices.ExistsAsync(indexName);
        if (existsResponse.Exists)
        {
            await elasticClient.Indices.DeleteAsync(indexName);
        }
    }

    public static void DeleteDocuments<T>(this IElasticClient elasticClient)
        where T: class
    {
        elasticClient.DeleteByQuery<T>(del => del
            .Query(q => q.QueryString(qs => qs.Query("*")))
                .Conflicts(Conflicts.Proceed));
    }

    public static void DeleteCategoryIndex(this IElasticClient elasticClient)
    {
        elasticClient.Indices.Delete(ElasticsearchIndices.Category);
    } 
    
    public static void DeleteGenreIndex(this IElasticClient elasticClient)
    {
        elasticClient.Indices.Delete(ElasticsearchIndices.Genre);
    } 
}
