using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.ArchitectureTests.Base;
using NetArchTest.Rules;

namespace CityShare.Backend.Tests.ArchitectureTests;

public class InfrastructureTests : TestBase
{
    [Fact]
    public void Infrastructure_ShouldNot_DependOnPersistence()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(PersistenceNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }


    [Fact]
    public void Infrastructure_ShouldNot_ContaintServiceInterfaces()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith(ClassNames.Suffix.Service)
            .ShouldNot()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
