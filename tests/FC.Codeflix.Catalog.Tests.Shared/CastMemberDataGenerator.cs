using FC.Codeflix.Catalog.Domain.Enums;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Tests.Shared;

public class CastMemberDataGenerator : DataGeneratorBase
{
    
    public DomainEntity.CastMember GetValidCastMember()
        => new(
            Guid.NewGuid(),
            GetValidName(),
            GetRandomCastMemberType()
        );
    
    public string GetValidName()
        => Faker.Name.FullName();
    
    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)new Random().Next(1, 2);

    public IList<CastMemberModel> GetCastMemberModelList(int count)
        => Enumerable.Range(0, count)
            .Select(_ =>
            {
                Task.Delay(5).GetAwaiter().GetResult();
                return CastMemberModel.FromEntity(GetValidCastMember());
            })
            .ToList();
}