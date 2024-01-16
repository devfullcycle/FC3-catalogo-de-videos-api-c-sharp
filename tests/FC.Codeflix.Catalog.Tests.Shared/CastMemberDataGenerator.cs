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
}