using static LocalizationService;

namespace WorkTimeLog
{
    public partial class MainPage : ContentPage
    {

        public MainPage(AppDbContext db)
        {
            InitializeComponent();
            Database = db; // Asignar la instancia a la propiedad estática
            LoadUsers();
        }

        private async void LoadUsers()
        {
            List<User> usuarios = await Database.GetUsersAsync();

            foreach (var usuario in usuarios)
            {
                UserPicker.Items.Add(usuario.NameSurname);
            }
        }

        private void UserPickerSelection(object sender, EventArgs e)
        {
            PlaceholderLabel.IsVisible = UserPicker.SelectedIndex == -1;
        }

        internal void AddUser(User user)
        {
            UserPicker.Items.Add(user.NameSurname);
        }

        internal void RemoveUser(User user)
        {
            UserPicker.Items.Remove(user.NameSurname);
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            string userName = (string)UserPicker.SelectedItem;
            string inputPassword = password.Text;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(inputPassword))
            {
                await DisplayAlert(GetString("Error"), GetString("LoginPrompt"), "OK");
                return;
            }

            User? user = await Database.GetUserByNameAsync(userName);

            if (user != null && user.Password == inputPassword)
            {
                UserPicker.SelectedIndex = -1;
                if (user.NameSurname != "Admin") await Navigation.PushAsync(new WorkerPage(user));
                else await Navigation.PushAsync(new AdminPage(this, user));
            }
            else
            {
                await DisplayAlert(GetString("Error"), GetString("LoginError"), "OK");
            }
            password.Text = string.Empty;
        }
    }

}
