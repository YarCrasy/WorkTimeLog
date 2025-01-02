namespace WorkTimeLog;

public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
        if (Application.Current != null) isDarkMode.IsToggled = Application.Current.RequestedTheme == AppTheme.Dark;

        // Establecer el idioma seleccionado actual
        var currentCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        switch (currentCulture)
        {
            case "es":
                languagePicker.SelectedIndex = 0;
                break;
            case "zh":
                languagePicker.SelectedIndex = 1;
                break;
            default:
                languagePicker.SelectedIndex = 2;
                break;
        }
    }

    private void IsDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if (Application.Current != null) Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        var selectedLanguage = languagePicker.SelectedIndex;
        switch (selectedLanguage)
        {
            case 0:
                SetCulture("es");
                break;
            case 1:
                SetCulture("zh");
                break;
            default:
                SetCulture("en");
                break;
        }
    }

    private void SetCulture(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        LocalizationService.LoadLocalization(cultureCode);

        // Recargar la página para aplicar el nuevo idioma
        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }
}
