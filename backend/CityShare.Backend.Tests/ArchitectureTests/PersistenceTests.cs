using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.ArchitectureTests.Base;
using NetArchTest.Rules;

namespace CityShare.Backend.Tests.ArchitectureTests;

public class PersistenceTests : TestBase
{
    [Fact]
    public void Persistence_ShouldNot_DependOnPersistence()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(PersistenceAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Persistence_ShouldNot_ContaintServiceInterfaces()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(PersistenceAssembly)
            .That()
            .HaveNameEndingWith(ClassNames.Suffix.Service)
            .ShouldNot()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Persistence_ShouldNot_ContaintRepositoryInterfaces()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(PersistenceAssembly)
            .That()
            .HaveNameEndingWith(ClassNames.Suffix.Repository)
            .ShouldNot()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
