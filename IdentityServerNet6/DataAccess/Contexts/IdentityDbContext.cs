using IdentityServerNet6.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerNet6.DataAccess.Contexts
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        //public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        //{
        //}
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
