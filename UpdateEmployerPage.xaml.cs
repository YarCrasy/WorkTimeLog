namespace WorkTimeLog;

public partial class UpdateEmployerPage : ContentPage
{
    AdminPage adminPage;

    public UpdateEmployerPage(AdminPage a)
	{
		InitializeComponent();
        adminPage = a;
    }

    private async void UpdateButtonClicked(object sender, EventArgs e)
    {
        companyData.nif = nifInput.Text;
        companyData.name = employerNameInput.Text;

        Database.SaveEmployerData();
        adminPage.DisplayCompanyData();

        await DisplayAlert("Comfirmation", "Employer data updated correctly", "OK");
        await Navigation.PopAsync();
    }
}