namespace WorkTimeLog
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using AppDbContext db = new();
            db.Database.EnsureCreated();

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}