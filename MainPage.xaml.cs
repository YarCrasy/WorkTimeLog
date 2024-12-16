namespace WorkTimeLog
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private async void CargarUsuarios()
        {
            using AppDbContext db = new();
            var usuarios = await db.Users.ToListAsync();

            foreach (var usuario in usuarios)
            {
                UserPicker.Items.Add(usuario.NameSurname);
            }
        }

        private async void UserPickerSelection(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            PlaceholderLabel.IsVisible = picker.SelectedIndex == -1 ? true : false;
            if (picker.SelectedIndex == 0)
            {
                picker.SelectedIndex = -1;
                await Navigation.PushAsync(new RegisterPage(this));
            }
        }

        internal void AddUser(User user)
        {
            UserPicker.Items.Add(user.NameSurname);
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            string? nombreUsuario = (string?)UserPicker.SelectedItem;
            string? contraseñaIngresada = password.Text;

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseñaIngresada))
            {
                await DisplayAlert("Error", "Por favor, ingresa el usuario y la contraseña.", "OK");
                return;
            }

            using AppDbContext db = new();
            User? usuario = await db.Users.FirstOrDefaultAsync(u => u.NameSurname == nombreUsuario);

            if (usuario != null && usuario.Password == contraseñaIngresada)
            {
                // Aquí puedes navegar a otra página o realizar alguna acción
                await Navigation.PushAsync(new WorkerPage(usuario));
            }
            else
            {
                // Credenciales incorrectas
                await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
            password.Text = string.Empty;
        }
    }

}
