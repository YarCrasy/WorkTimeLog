using Microsoft.Maui.Controls.Shapes;

namespace WorkTimeLog;

public partial class UsersListPage : ContentPage
{
    internal MainPage main;

    public UsersListPage(MainPage m)
    {
        InitializeComponent();
        LoadUsers();
        main = m;
    }

    private async void LoadUsers()
    {
        List<User> users = await Database.GetUsersAsync();
        users = users.Where(u => u.Nif != "Admin").ToList();

        foreach (var user in users)
        {
            var userLabel = new Label
            {
                Text = user.NameSurname + " (" + user.Nif + ")",
                Margin = 10,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            var deleteButton = new Button
            {
                Margin = 10,
                Padding = 3,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 15,
            };
            deleteButton.Clicked += DeleteUserButtonClicked;
            deleteButton.ImageSource = new FileImageSource { File = "delete_icon.png" };


            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new());
            grid.ColumnDefinitions.Add(new());

            grid.Children.Add(userLabel);
            grid.Children.Add(deleteButton);
            Grid.SetColumn(userLabel, 0);
            Grid.SetColumn(deleteButton, 2);

            Border userLayout = new()
            {
                Padding = 10,
                BackgroundColor = Colors.Blue,
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                Margin = new Thickness(0, 0, 0, 10),
                ClassId = user.Nif,
                Content = grid
            };

            UserList.Add(userLayout);
        }
    }

    private async void DeleteUserButtonClicked(object? sender, EventArgs e)
    {
        Button deleteButton = (Button)sender!;
        Border userLayout = (Border)deleteButton.Parent.Parent;
        string userId = userLayout.ClassId;

        if (string.IsNullOrEmpty(userId))
        {
            await DisplayAlert("Error", "ID nulo o vacío.", "OK");
            return;
        }

        User? user = await Database.GetUserByNifAsync(userId);
        if (user == null)
        {
            await DisplayAlert("Error", "No se pudo encontrar el usuario en la base de datos.", "OK");
            return;
        }

        await Database.DropUserAsync(user);
        UserList.Children.Remove(userLayout);
        main.RemoveUser(user);

        await DisplayAlert("Usuario Eliminado", "El usuario ha sido eliminado correctamente.", "OK");
    }
}
