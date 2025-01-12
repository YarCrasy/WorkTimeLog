using System.Text.RegularExpressions;

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
            Nif = nifInput.Text,
            NameSurname = userNameRegister.Text,
            Password = passwordRegister.Text,
            LastIsEntry = false,
        };

        if (await ValidateRegistration(newUser, confirmPasswordRegister.Text))
        {
            await Database.InsertUserAsync(newUser);
            mainPage.AddUser(newUser);
            await DisplayAlert("Success", "User Created correctly", "OK");
            await Navigation.PopAsync();
        }
    }

    private async Task<bool> ValidateRegistration(User user, string confirmPassword)
    {
        if (string.IsNullOrEmpty(user.Nif) || string.IsNullOrEmpty(user.NameSurname) || 
            string.IsNullOrEmpty(user.Password))
        {
            await DisplayAlert("Error", "you must fill all the fields", "OK");
            return false;
        }
        if(!IsValidNif(user.Nif))
        {
            await DisplayAlert("Error", "Nif is not valid", "OK");
            return false;
        }
        if (await Database.UserExist(user))
        {
            await DisplayAlert("Error", "User already exists", "OK");
            return false;
        }
        if (user.Password != confirmPassword)
        {
            await DisplayAlert("Error", "Password are not the same", "OK");
            return false;
        }
        return true;
    }
    private bool IsValidNif(string nif)
    {
        //NIF validation
        Regex nifRegex = new(@"^\d{8}[A-Za-z]$");

        //NIE validation
        Regex nieRegex = new(@"^[XYZxyz]\d{7}[A-Za-z]$");

        return nifRegex.IsMatch(nif) || nieRegex.IsMatch(nif);
    }
}
