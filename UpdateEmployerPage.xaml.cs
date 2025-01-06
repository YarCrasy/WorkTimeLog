namespace WorkTimeLog;

public partial class UpdateEmployerPage : ContentPage
{
	public UpdateEmployerPage()
	{
		InitializeComponent();
	}

    private async Task<bool> ValidateRegistration()
    {
        await DisplayPromptAsync("Comfirmation", "Company data updated correctly", "OK");
        return true;
    }

    private async void UpdateButtonClicked(object sender, EventArgs e)
    {
        companyData = new Company
        {
            nif = nifInput.Text,
            name = employerNameInput.Text
        };

        await DisplayAlert("Success", "Company data updated correctly", "OK");

    }
}