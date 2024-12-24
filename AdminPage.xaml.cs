namespace WorkTimeLog;

public partial class AdminPage : ContentPage
{
    readonly MainPage main;

    public AdminPage(MainPage m)
    {
        InitializeComponent();
        main = m;
    }

    private async void CreateUserButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(main));
    }

    private async void ManageUsersButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsersListPage());
    }

}
