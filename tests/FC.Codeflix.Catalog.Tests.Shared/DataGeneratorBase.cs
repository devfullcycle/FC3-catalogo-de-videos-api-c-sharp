using Bogus;

namespace FC.Codeflix.Catalog.Tests.Shared;
public abstract class DataGeneratorBase
{
    public Faker Faker { get; set; }

    protected DataGeneratorBase()
        => Faker = new Faker("pt_BR");

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}
