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
            await DisplayAlert(LanguageResource.Success, LanguageResource.UserCreatedMsg, LanguageResource.Confirm);
            await Navigation.PopAsync();
        }
    }

    private async Task<bool> ValidateRegistration(User user, string confirmPassword)
    {
        if (string.IsNullOrEmpty(user.Nif) || string.IsNullOrEmpty(user.NameSurname) || 
            string.IsNullOrEmpty(user.Password))
        {
            await DisplayAlert(LanguageResource.Error, LanguageResource.NullOrEmptyMsg, LanguageResource.Confirm);
            return false;
        }
        if(!IsValidNif(user.Nif))
        {
            await DisplayAlert(LanguageResource.Error, LanguageResource.NifNotValid, LanguageResource.Confirm);
            return false;
        }
        if (await Database.UserExist(user))
        {
            await DisplayAlert(LanguageResource.Error, LanguageResource.UserExists, LanguageResource.Confirm);
            return false;
        }
        if (user.Password != confirmPassword)
        {
            await DisplayAlert(LanguageResource.Error, LanguageResource.PasswordConfirmErr, LanguageResource.Confirm);
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
