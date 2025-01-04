namespace WorkTimeLog.Services
{
    internal class LocalizationService
    {
        public static LocalizationService Instance { get; } = new();

        private LocalizationService()
        {
            SetCulture(new CultureInfo("en"));
        }

        public void SetCulture(CultureInfo culture)
        {
            Resources.Localization.LanguageResource.Culture = culture;
        }


    }
}
