using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.ArchitectureTests.Base;
using NetArchTest.Rules;

namespace CityShare.Backend.Tests.ArchitectureTests;

public class DomainTests : TestBase
{
    [Fact]
    public void Domain_ShouldNot_DependOnApplication()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Domain_ShouldNot_DependOnInfrastructure()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Domain_ShouldNot_DependOnPersistence()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn(PersistenceNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
