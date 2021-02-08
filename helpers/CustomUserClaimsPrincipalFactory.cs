using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace customerPhoneApi.helpers
{

  public class CustomUserClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser> where TUser : class
  {
    private readonly HttpContext _httpContext;
    public CustomUserClaimsPrincipalFactory(UserManager<TUser> userManager
        , IOptions<IdentityOptions> optionsAccessor
        , IHttpContextAccessor httpContextAccessor)
        : base(userManager, optionsAccessor)
    {
      _httpContext = httpContextAccessor.HttpContext;
    }
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
      var ci = await base.GenerateClaimsAsync(user);
      ci.AddClaim(new Claim("RequestPath", _httpContext.Request.Path.Value));
      return ci;
    }
  }


}