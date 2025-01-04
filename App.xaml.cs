using WorkTimeLog.Services;

namespace WorkTimeLog
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LoadSettings();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private void LoadSettings()
        {
            // Cargar tema oscuro
            bool isDarkModeEnabled = Preferences.Get("IsDarkModeEnabled", RequestedTheme == AppTheme.Dark);
            UserAppTheme = isDarkModeEnabled ? AppTheme.Dark : AppTheme.Light;

            // Cargar idioma
            string savedLanguage = Preferences.Get("AppLanguage", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            CultureInfo culture = new(savedLanguage);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Actualizar la cultura en el servicio de localización
            LocalizationService.Instance.SetCulture(culture);
        }
    }
}