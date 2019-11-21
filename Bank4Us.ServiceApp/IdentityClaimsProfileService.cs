using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Bank4Us.Common.Core;
using Bank4Us.DataAccess.Core;

namespace Bank4Us.ServiceApp
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private DataContext _dbContext;

        public IdentityClaimsProfileService(UserManager<ApplicationUser> userManager, DataContext dbContext, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _dbContext = dbContext;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            //claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            // note: to dynamically add roles (ie. for users other than consumers - simply look them up by sub id
            //claims.Add(new Claim(ClaimTypes.Role, Roles.Consumer)); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4

            //*************
            //INFO: Read the [AspNetUserClaims] from the database and put them in the token.
            var userCliams = _dbContext.UserClaims.Where(uc => uc.UserId.Equals(user.Id)).ToList();
            foreach (var cl in userCliams)
            {
                claims.Add(new Claim(cl.ClaimType, cl.ClaimValue));
            }
            //**************

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
