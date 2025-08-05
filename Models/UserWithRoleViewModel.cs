namespace E_PRESCRIBING_SYSTEM.Models
{
    public class UserWithRoleViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
