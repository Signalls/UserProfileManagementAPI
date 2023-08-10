using Microsoft.AspNetCore.Identity;
using UserProfileData.Enum;

namespace UserProfileData.Domain
{
    public class UserProfile : IdentityUser
    {
        public string Address { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public MaritalStatus maritalStatus { get; set; }
    }
}