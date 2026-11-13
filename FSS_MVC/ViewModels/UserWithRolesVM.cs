namespace FSS_MVC.ViewModels
{
    public class UserWithRolesVm
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
