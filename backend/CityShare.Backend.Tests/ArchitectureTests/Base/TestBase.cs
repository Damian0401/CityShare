using System.Reflection;

namespace CityShare.Backend.Tests.ArchitectureTests.Base;

public class TestBase
{
    protected Assembly ApplicationAssembly { get; }
    protected Assembly DomainAssembly { get; }
    protected Assembly InfrastructureAssembly { get; }
    protected Assembly PersistenceAssembly { get; }    
    protected string ApplicationNamespace { get; }
    protected string DomainNamespace { get; }
    protected string InfrastructureNamespace { get; }
    protected string PersistenceNamespace { get; }

    public TestBase()
    {
        ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
        DomainAssembly = typeof(Domain.Shared.Result).Assembly;
        InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;
        PersistenceAssembly = typeof(Persistence.DependencyInjection).Assembly;

        ApplicationNamespace = "CityShare.Backend.Application";
        DomainNamespace = "CityShare.Backend.Domain";
        InfrastructureNamespace = "CityShare.Backend.Infrastructure";
        PersistenceNamespace = "CityShare.Backend.Persistance";
    }
}
