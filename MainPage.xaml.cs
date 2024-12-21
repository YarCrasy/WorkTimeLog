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

        private async void UserPickerSelection(object sender, EventArgs e)
        {
            PlaceholderLabel.IsVisible = UserPicker.SelectedIndex == -1 ? true : false;
            if (UserPicker.SelectedIndex == 0)
            {
                UserPicker.SelectedIndex = -1;
                await Navigation.PushAsync(new RegisterPage(this));
            }
        }

        internal void AddUser(User user)
        {
            UserPicker.Items.Add(user.NameSurname);
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            string nombreUsuario = (string)UserPicker.SelectedItem;
            string contraseñaIngresada = password.Text;

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseñaIngresada))
            {
                await DisplayAlert("Error", "Por favor, ingresa el usuario y la contraseña.", "OK");
                return;
            }

            User? usuario = await Database.GetUserByNameAsync(nombreUsuario);

            if (usuario != null && usuario.Password == contraseñaIngresada)
            {
                if(usuario.NameSurname != "Admin") await Navigation.PushAsync(new WorkerPage(usuario));
                else await Navigation.PushAsync(new AdminPage(this));
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
            password.Text = string.Empty;
        }
    }

}
