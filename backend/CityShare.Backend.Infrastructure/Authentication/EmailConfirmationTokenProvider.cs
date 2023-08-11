using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Infrastructure.Authentication;

public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{
}

public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
    where TUser : IdentityUser
{
    public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, 
        IOptions<EmailConfirmationTokenProviderOptions> options, 
        ILogger<DataProtectorTokenProvider<TUser>> logger) 
        : base(dataProtectionProvider, options, logger)
    {
    }
}
