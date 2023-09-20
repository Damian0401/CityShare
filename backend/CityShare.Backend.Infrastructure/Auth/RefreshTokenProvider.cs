using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Infrastructure.Auth;

public class RefreshTokenProviderOptions : DataProtectionTokenProviderOptions
{
};

public class RefreshTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
    where TUser : IdentityUser
{
    public RefreshTokenProvider(IDataProtectionProvider dataProtectionProvider, 
        IOptions<RefreshTokenProviderOptions> options, 
        ILogger<DataProtectorTokenProvider<TUser>> logger) 
        : base(dataProtectionProvider, options, logger)
    {
    }
}
