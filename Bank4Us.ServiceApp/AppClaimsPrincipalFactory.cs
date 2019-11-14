using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bank4Us.Common.Core;
using Bank4Us.DataAccess.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bank4Us.ServiceApp
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2019
    ///   Name: William J Leannah
    ///   Description: Introduction to Identity in ASP.NET Core
    ///                https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.1&tabs=visual-studio 
    /// </summary>
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private DataContext _dbContext;

        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            DataContext dbContext,
            IOptions<IdentityOptions> optionsAccessor) : base(
            userManager, optionsAccessor)
        {

            _dbContext = dbContext;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            var userCliams = _dbContext.UserClaims.Where(uc => uc.UserId.Equals(user.Id)).ToList();

            foreach (var cl in userCliams)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(cl.ClaimType, cl.ClaimValue) });
            }

            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.Surname, "Scott"),
                new Claim(ClaimTypes.Role, "Manager"),
                new Claim(ClaimTypes.Role, "Supervisor") });

            return principal;
        }

    }
}
