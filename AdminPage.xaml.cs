namespace WorkTimeLog;

public partial class AdminPage : ContentPage
{
    readonly MainPage main;
    readonly User admin;

    public AdminPage(MainPage m, User u)
    {
        InitializeComponent();
        main = m;
        admin = u;
    }

    private async void CreateUserButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(main));
    }

    private async void ManageUsersButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsersListPage(main));
    }

    private async void ChangePasswordButtonClicked(object sender, EventArgs e)
    {
        admin.Password = 
            await DisplayPromptAsync(
                "Cambiar contraseņa", 
                "Introduce la nueva contraseņa", 
                "Aceptar", "Cancelar", 
                "Nueva contraseņa", 18, Keyboard.Default, admin.Password);
        await Database.UpdateUserAsync(admin);
    }
}
