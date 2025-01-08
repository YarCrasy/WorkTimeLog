namespace WorkTimeLog
{
    public partial class MainPage : ContentPage
    {
        public MainPage(AppDbContext db)
        {
            InitializeComponent();
            Database = db;
            LoadUsers();
        }

        private async void LoadUsers()
        {
            User[] userList = [.. (await Database.GetUsersAsync())];
            for (int i = 0; i < userList.Length; i++)
            {
                UserPicker.Items.Add(userList[i].NameSurname);
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
            var userName = (string)UserPicker.SelectedItem;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password.Text))
            {
                await DisplayAlert("Error", "you must fill all the fields", "OK");
                return;
            }

            var user = await Database.GetUserByNameAsync(userName);
            if (user?.Password == password.Text)
            {
                UserPicker.SelectedIndex = -1;
                await Navigation.PushAsync(user.IsAdmin() ? 
                    new AdminPage(this, user) : new WorkerPage(user));
            }
            else
            {
                await DisplayAlert("Error", "User does not exist or password incorrect", "OK");
            }
            password.Text = string.Empty;
        }
    }

}
