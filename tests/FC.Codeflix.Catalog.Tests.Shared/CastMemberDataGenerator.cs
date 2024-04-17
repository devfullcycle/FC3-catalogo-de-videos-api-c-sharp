using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.Repositories.DTOs;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Tests.Shared;

public class CastMemberDataGenerator : DataGeneratorBase
{
    
    public DomainEntity.CastMember GetValidCastMember()
        => new(
            Guid.NewGuid(),
            GetValidCastMemberName(),
            GetRandomCastMemberType()
        );
    
    public string GetValidCastMemberName()
        => Faker.Name.FullName();
    
    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)new Random().Next(1, 2);

    public IList<CastMemberModel> GetCastMemberModelList(int count)
        => Enumerable.Range(0, count)
            .Select(index =>
            {
                var model = CastMemberModel.FromEntity(GetValidCastMember());
                model.CreatedAt = DateTime.UtcNow.AddMinutes(index);
                return model;
            })
            .ToList();
    
    public IList<CastMemberModel> GetCastMemberModelList(IEnumerable<string> names)
        => names
            .Select(name =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                var castMemberModel = CastMemberModel.FromEntity(GetValidCastMember());
                castMemberModel.Name = name;
                return castMemberModel;
            })
            .ToList();

    public IList<CastMemberModel> CloneCastMembersListOrdered(
        IList<CastMemberModel> castMembersList, string orderBy, SearchOrder direction)
    {
        var listClone = new List<CastMemberModel>(castMembersList);
        var orderedEnumerable = (orderBy.ToLower(), direction) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }
}