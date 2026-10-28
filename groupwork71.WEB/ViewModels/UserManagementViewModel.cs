using groupwork71.DAL.Models;

namespace groupwork71.ViewModels;

public class UserManagementViewModel
{
    public User User { get; set; } = null!;

    public IList<string> Roles { get; set; } = new List<string>();

    public string SelectedRole { get; set; } = "User";

    public bool IsCurrentUser { get; set; }
}