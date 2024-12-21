namespace WorkTimeLog;

public partial class UsersListPage : ContentPage
{
    public UsersListPage()
    {
        InitializeComponent();
        LoadUsers();
    }

    private async void LoadUsers()
    {
        List<User> users = await Database.GetUsersAsync();
        users = users.Where(u => u.Nif != "Admin").ToList();
    }
}
