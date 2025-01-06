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

        DisplayCompanyData();
    }

    private void DisplayCompanyData()
    {
        companyName.Text = "Empleador: " + companyData.name;
        companyNif.Text = "Nif: " + companyData.nif;
    }

    private async void UpdateCompanyDataClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateEmployerPage());
    }

    private async void CreateUserButtonClicked(object sender, EventArgs e)
    {
        if (companyData.IsEmpty())
        {
            await DisplayAlert("Error", "Company data is not set yet", "OK");
            return;
        }

        await Navigation.PushAsync(new RegisterPage(main));
    }

    private async void ManageUsersButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UsersListPage(main));
    }

    private async void ChangePasswordButtonClicked(object sender, EventArgs e)
    {
        string newPassword =
                await DisplayPromptAsync(
                    "Cambiar contrase�a",
                    "Introduce la nueva contrase�a",
                    "Aceptar", "Cancelar",
                    "Nueva contrase�a", 18, Keyboard.Default);

        if (!admin.ChangePassword(newPassword)) return;
        else await DisplayAlert("�xito", "Contrase�a cambiada correctamente.", "OK");

        await Database.UpdateUserAsync(admin);
    }

}
