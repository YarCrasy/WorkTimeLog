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
        //Company company = await Database.GetCompanyAsync();
        //if (company.IsEmpty())
        //{
        //    await DisplayAlert("Error", "Company data is not set yet", "OK");
        //    return;
        //}

        await Navigation.PushAsync(new RegisterPage(main, new()));
    }

    private async void ManageUsersButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsersListPage(main));
    }

    private async void ChangePasswordButtonClicked(object sender, EventArgs e)
    {
        string newPassword =
                await DisplayPromptAsync(
                    "Cambiar contraseña",
                    "Introduce la nueva contraseña",
                    "Aceptar", "Cancelar",
                    "Nueva contraseña", 18, Keyboard.Default);

        if (!admin.ChangePassword(newPassword)) return;
        else await DisplayAlert("Éxito", "Contraseña cambiada correctamente.", "OK");

        await Database.UpdateUserAsync(admin);
    }
}
