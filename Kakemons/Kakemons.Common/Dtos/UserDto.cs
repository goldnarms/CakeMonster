using Kakemons.Common.Models;

namespace Kakemons.Common.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public DbPosition Position { get; set; }
        public string Fullname => $"{FirstName} {LastName}";
        public string AvatarUrl { get; set; }

        public UserDto()
        {
            
        }
        
        public UserDto(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
