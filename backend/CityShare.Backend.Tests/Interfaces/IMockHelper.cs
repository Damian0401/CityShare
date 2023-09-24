using System.Linq.Expressions;

namespace CityShare.Backend.Tests.Interfaces;

internal interface IMockHelper<TMock>
    where TMock : class
{
    TMock Object { get; }
    public void Setup<TResult>(Expression<Func<TMock, TResult>> expression, TResult result);
    public void SetupAsync<TResult>(Expression<Func<TMock, Task<TResult>>> expression, TResult result);
}
