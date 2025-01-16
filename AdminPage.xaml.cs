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

    public void DisplayCompanyData()
    {
        employerName.Text = Database.employerData.name;
        employerNif.Text = Database.employerData.nif;
    }

    private async void UpdateEmployerDataClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateEmployerPage(this));
    }


    private async void CreateUserButtonClicked(object sender, EventArgs e)
    {
        if (Database.employerData.IsEmpty())
        {
            await DisplayAlert(LanguageResource.Error, LanguageResource.EmployerUpdatedMsg, LanguageResource.Confirm);
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
                    "Cambiar contraseña",
                    "Introduce la nueva contraseña",
                    LanguageResource.Confirm, "Cancelar",
                    "Nueva contraseña", 18, Keyboard.Default);

        if (!admin.ChangePassword(newPassword)) return;
        else await DisplayAlert(LanguageResource.Success, "Contraseña cambiada correctamente.", LanguageResource.Confirm);

        await Database.UpdateUserAsync(admin);
    }

}
