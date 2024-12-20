namespace WorkTimeLog;

public partial class WorkerPage : ContentPage
{
    readonly User user;

    public WorkerPage(User user)
    {
        InitializeComponent();
        this.user = user;
        workerName.Text += user.NameSurname;
        workerNif.Text += user.Nif;

        datePicker.Date = DateTime.Now.Date;
        timePicker.Time = DateTime.Now.TimeOfDay;

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
            await db.SaveChangesAsync();
        }

        Label label = new() { Text = workLog.Date.ToString("dd-MM-yyyy HH:mm") };

        if (workLog.IsEntry) entry.Children.Add(label);
        else exit.Children.Add(label);
    }

    private void LoadWorkLogs()
    {
        entry.Children.Clear();
        exit.Children.Clear();
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
