using System.Diagnostics;

namespace WorkTimeLog;

public partial class RegisterPage : ContentPage
{
    readonly MainPage mainPage;
    public RegisterPage(MainPage mainRef)
    {
        InitializeComponent();
        mainPage = mainRef;
    }

    private async void RegisterButtonClicked(object sender, EventArgs e)
    {
        User newUser = new()
        {
            Nif = nif.Text,
            NameSurname = userNameRegister.Text,
            Password = passwordRegister.Text,
            LastIsEntry = false
        };
        string confirmPassword = confirmPasswordRegister.Text;

        if (string.IsNullOrEmpty(newUser.Nif) ||
            string.IsNullOrEmpty(newUser.NameSurname) ||
            string.IsNullOrEmpty(newUser.Password))
        {
            await DisplayAlert("Error", "Faltan campos por rellenar", "OK");
            return;
        }
        else if (await Database.UserExist(newUser))
        {
            await DisplayAlert("Error", "El nombre o nif del usuario ya exsiste", "OK");
            return;
        }
        else if (newUser.Password != confirmPassword)
        {
            await DisplayAlert("Error", "Las contraseñas son distintas", "OK");
            return;
        }

        await Database.InsertUserAsync(newUser);

        await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
        mainPage.AddUser(newUser);
        await Navigation.PopAsync();
    }


}
