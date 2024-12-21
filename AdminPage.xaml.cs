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

    private async void DeleteUserButtonClicked(object sender, EventArgs e)
    {
        // L�gica para eliminar un usuario
        await DisplayAlert("Eliminar Usuario", "Funcionalidad para eliminar usuario no implementada.", "OK");
    }

    private async void ViewUsersButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la p�gina UsersListPage
        await Navigation.PushAsync(new UsersListPage());
    }
}
