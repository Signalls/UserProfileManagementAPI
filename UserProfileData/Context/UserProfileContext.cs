using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserProfileData.Domain;

namespace UserProfileData.Context
{
    public class UserProfileContext : IdentityDbContext<UserProfile>
    {

        public UserProfileContext(DbContextOptions<UserProfileContext> Options) : base(Options)
        {



        }
    }
}
