using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.ArchitectureTests.Base;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;

namespace CityShare.Backend.Tests.ArchitectureTests;

public class ApplicationTests : TestBase
{
    [Fact]
    public void Application_ShouldNot_DependOnInfrastructure()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_ShouldNot_DependOnPersistence()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn(PersistenceNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ValidatorNames_Should_EndsWithValidator()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith(ClassNames.Suffix.Validator)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void HandlerNames_Should_EndsWithHandler()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .HaveNameEndingWith(ClassNames.Suffix.Handler)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void CQRSNames_Should_EndsWithCommandOrQuery()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith(ClassNames.Suffix.Command)
            .Or()
            .HaveNameEndingWith(ClassNames.Suffix.Query)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_ShouldNot_ContainServiceImplementations()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith(ClassNames.Suffix.Service)
            .Should()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_ShouldNot_ContainRepositoryImplementations()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith(ClassNames.Suffix.Repository)
            .Should()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
