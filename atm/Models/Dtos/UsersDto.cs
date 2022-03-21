namespace atm.Models.ViewModels
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserDto : UsersDto
    {
        public string RoleName { get; set; }
    }
}