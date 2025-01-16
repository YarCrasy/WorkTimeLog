using WorkTimeLog.Resources.Localization;

namespace WorkTimeLog;

public partial class UpdateEmployerPage : ContentPage
{
    readonly AdminPage adminPage;

    public UpdateEmployerPage(AdminPage a)
	{
		InitializeComponent();
        adminPage = a;
    }

    private async void UpdateButtonClicked(object sender, EventArgs e)
    {
        Database.employerData.nif = nifInput.Text;
        Database.employerData.name = employerNameInput.Text;

        Database.SaveEmployerData();
        adminPage.DisplayCompanyData();

        await DisplayAlert(LanguageResource.Success, LanguageResource.UpdatedMsg, LanguageResource.Confirm);
        await Navigation.PopAsync();
    }
}