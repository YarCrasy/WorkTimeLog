namespace WorkTimeLog;

public partial class WorkerPage : ContentPage
{
    User user;

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

        LoadWorkLogs();
    }

    private void LoadWorkLogs()
    {
        workLogStackLayout.Children.Clear();

        using AppDbContext db = new();
        List<WorkLog> workLogs = [.. db.WorkLogs.Where(w => w.UserNif == user.Nif)];
        string aux;
        for (int i = 0; i < workLogs.Count; i++)
        {
            aux = workLogs[i].Date + "";
            if (workLogs[i].IsEntry) aux += " Entrada";
            else aux += " Salida";

            workLogStackLayout.Children.Add(new Label
            {
                Text = aux
            });
        }
    }
}
