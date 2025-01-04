using WorkTimeLog.Services;

namespace WorkTimeLog;

public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();

        // Cargar ajustes guardados
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Cargar tema oscuro
        bool isDarkModeEnabled = Preferences.Get("IsDarkModeEnabled", Application.Current?.RequestedTheme == AppTheme.Dark);
        isDarkMode.IsToggled = isDarkModeEnabled;

        // Cargar idioma
        string savedLanguage = Preferences.Get("AppLanguage", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        PlaceholderLabel.Text = savedLanguage switch
        {
            "es" => "Español",
            "zh" => "简体中文",
            _ => "English"
        };
        ChangeLanguage(savedLanguage);
    }

    private void IsDarkModeToggled(object sender, ToggledEventArgs e)
    {
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
            Preferences.Set("IsDarkModeEnabled", e.Value);
        }
    }

    private void ChangeLanguage(string languageCode)
    {
        CultureInfo culture = new(languageCode);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        LocalizationService.Instance.SetCulture(culture);

        Preferences.Set("AppLanguage", languageCode);

    }

    private void SelectLanguage(object sender, EventArgs e)
    {
        string selectedLanguage = (string)languagePicker.SelectedItem;

        switch (selectedLanguage)
        {
            case "Español":
                ChangeLanguage("es");
                break;
            case "简体中文":
                ChangeLanguage("zh");
                break;
            default:
                ChangeLanguage("en");
                break;
        }

        if (Application.Current?.Windows.Count > 0) Application.Current.Windows[0].Page = new AppShell();
    }
}
