using Microsoft.Maui.Controls.Shapes;

namespace WorkTimeLog;

public partial class UsersListPage : ContentPage
{

    public UsersListPage()
    {
        InitializeComponent();
        LoadUsers();
    }

    private async void LoadUsers()
    {
        List<User> users = await Database.GetUsersAsync();
        users = users.Where(u => u.Nif != "Admin").ToList();

        foreach (var user in users)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new());
            grid.ColumnDefinitions.Add(new());


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
                WidthRequest = 15,
            };
            deleteButton.Clicked += DeleteUserButtonClicked;
            deleteButton.ImageSource = new FileImageSource { File = "delete_icon.png" };

            grid.Children.Add(userLabel);
            grid.Children.Add(deleteButton);
            Grid.SetColumn(userLabel, 0);
            Grid.SetColumn(deleteButton, 2);


            Border userLayout = new()
            {
                Padding = 10,
                BackgroundColor = Colors.AliceBlue,
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                Margin = new Thickness(0, 0, 0, 10),
                ClassId = user.Nif,
                Content = grid
            };

            UserList.Add(userLayout);
        }
    }

    private async void DeleteUserButtonClicked(object sender, EventArgs e)
    {
        Button deleteButton = (Button)sender;

        Border userLayout = (Border)deleteButton.Parent.Parent;
        if (string.IsNullOrEmpty(userLayout.ClassId))
        {
            if (Database.GetUserByNifAsync(userLayout.ClassId) != null)
            {
                User u = await Database.GetUserByNifAsync(userLayout.ClassId);
                if (u != null) await Database.DropUserAsync(u);

                UserList.Children.Remove(userLayout);

                await DisplayAlert("Usuario Eliminado", "El usuario ha sido eliminado correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo encontrar el usuario en la base de datos.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "ID nulo o vacio.", "OK");
        }
    }
}
