using static LocalizationService;
namespace WorkTimeLog;

public partial class WorkerPage : ContentPage
{
    readonly User user;

    public WorkerPage(User u)
    {
        InitializeComponent();
        user = u;
        workerName.Text += user.NameSurname;
        workerNif.Text += user.Nif;

        datePicker.Date = DateTime.Now.Date;
        timePicker.Time = DateTime.Now.TimeOfDay;
        isEntry.IsToggled = !user.LastIsEntry;

        LoadWorkLogs();
    }

    private async void LogTimeClicked(object sender, EventArgs e)
    {
        WorkLog workLog = new()
        {
            UserNif = user.Nif,
            Date = datePicker.Date + timePicker.Time,
            IsEntry = isEntry.IsToggled
        };

        await Database.InsertWorkLogAsync(workLog);
        user.LastIsEntry = workLog.IsEntry;
        await Database.UpdateUserAsync(user);

        Label label = new() { Text = workLog.Date.ToString("dd-MM-yyyy HH:mm"), HorizontalOptions = LayoutOptions.Center };

        if (workLog.IsEntry) entry.Children.Add(label);
        else exit.Children.Add(label);

        await DisplayAlert("Éxito", "Fecha y hora registrado correctamente.", "OK");
        await Navigation.PopAsync();
    }

    private async void LoadWorkLogs()
    {
        entry.Children.Clear();
        exit.Children.Clear();

        Label entryLabel = new()
        {
            Text = "Entrada",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
        entry.Children.Add(entryLabel);

        Label exitLabel = new()
        {
            Text = "Salida",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
        exit.Children.Add(exitLabel);

        List<WorkLog> workLogs = await Database.GetWorkLogsByUserNifAsync(user.Nif);
        foreach (WorkLog workLog in workLogs)
        {
            Label label = new() { Text = workLog.Date.ToString("dd-MM-yyyy HH:mm"), HorizontalOptions = LayoutOptions.Center};

            if (workLog.IsEntry) entry.Children.Add(label);
            else exit.Children.Add(label);
        }
    }

    private async void ChangePasswordButtonClicked(object sender, EventArgs e)
    {
        string newPassword =
                await DisplayPromptAsync(
                    GetString("ChangePassword"),
                    GetString("EnterNewPassword"),
                    GetString("Accept"), GetString("Cancel"),
                    GetString("NewPassword"), 18, Keyboard.Default);

        if (!user.ChangePassword(newPassword)) return;
        else await DisplayAlert(GetString("Success"), GetString("PasswordChangedSuccessfully"), "OK");

        await Database.UpdateUserAsync(user);
    }
}
