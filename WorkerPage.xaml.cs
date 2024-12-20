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
        if (user.LastIsEntry) isEntry.IsToggled = false;
        else isEntry.IsToggled = true;

        LoadWorkLogs();
    }

    private async void LogTimeClicked(object sender, EventArgs e)
    {

        WorkLog workLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            UserNif = user.Nif,
            Date = datePicker.Date + timePicker.Time,
            IsEntry = isEntry.IsToggled
        };

        using (AppDbContext db = new())
        {
            db.WorkLogs.Add(workLog);
            db.Users.Update(user);
            await db.SaveChangesAsync();
        }


        Label label = new() { Text = workLog.Date.ToString("dd-MM-yyyy HH:mm") };

        if (workLog.IsEntry) entry.Children.Add(label);
        else exit.Children.Add(label);

        await DisplayAlert("Éxito", "Fecha y hora registrado correctamente.", "OK");
        await Navigation.PopAsync();
    }

    private void LoadWorkLogs()
    {
        entry.Children.Clear();
        exit.Children.Clear();

        Label EntryLabel = new() { Text = "Entrada", FontAttributes = FontAttributes.Bold };
        entry.Children.Add(EntryLabel);

        Label exitLabel = new() { Text = "Salida", FontAttributes = FontAttributes.Bold };
        exit.Children.Add(exitLabel);

        using AppDbContext db = new();
        List<WorkLog> workLogs = [.. db.WorkLogs.Where(w => w.UserNif == user.Nif)];
        for (int i = 0; i < workLogs.Count; i++)
        {
            Label label = new() { Text = workLogs[i].Date.ToString("dd-MM-yyyy HH:mm") };

            if (workLogs[i].IsEntry) entry.Children.Add(label);
            else exit.Children.Add(label);
        }
    }
}
