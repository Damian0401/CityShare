using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace CityShare.Backend.Tests.Other.Helpers;

internal class UserManagerMockHelper<TUser>
    : IMockHelper<UserManager<TUser>>
    where TUser : IdentityUser
{
    private readonly Mock<UserManager<TUser>> _userManagerMock;

    public UserManagerMockHelper()
    {
        var store = new Mock<IUserStore<TUser>>();
        _userManagerMock = new Mock<UserManager<TUser>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    public void Setup<TResult>(Expression<Func<UserManager<TUser>, TResult>> expression, TResult result)
    {
        _userManagerMock.Setup(expression).Returns(result);
    }

    public void SetupAsync<TResult>(Expression<Func<UserManager<TUser>, Task<TResult>>> expression, TResult result)
    {
        _userManagerMock.Setup(expression).ReturnsAsync(result);
    }

    public UserManager<TUser> Object => _userManagerMock.Object;
}
