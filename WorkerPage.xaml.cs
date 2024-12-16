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
    }

    private void LogTimeClicked(object sender, EventArgs e)
    {
        WorkLog workLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            UserNif = user.Nif,
            Date = datePicker.Date + timePicker.Time,
            IsEntry = true
        };
    }
}
