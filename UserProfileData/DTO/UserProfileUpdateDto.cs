using System.ComponentModel.DataAnnotations;
using UserProfileData.Enum;

namespace UserProfileData.DTO
{
    public class UserProfileUpdateDto
    {

        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public MaritalStatus maritalStatus { get; set; }
    }



}
