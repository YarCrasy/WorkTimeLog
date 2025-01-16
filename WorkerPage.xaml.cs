namespace WorkTimeLog;

public partial class WorkerPage : ContentPage
{
    readonly User user;
    readonly bool canEdit;

    public WorkerPage(User u, bool editable = true)
    {
        InitializeComponent();
        user = u;
        canEdit = editable;
        InitializeUI();
        LoadWorkLogs();
    }

    private void InitializeUI()
    {
        workerName.Text = user.NameSurname;
        workerNif.Text = user.Nif;

        employerName.Text = Database.employerData.name;
        companyNif.Text = Database.employerData.nif;

        if (!canEdit)
        {
            editableInputs.Clear();
        }
        else
        {
            datePicker.Date = DateTime.Now.Date;
            timePicker.Time = DateTime.Now.TimeOfDay;
            isEntry.IsToggled = !user.LastIsEntry;
        }
    }

    private async void LogTimeClicked(object sender, EventArgs e)
    {
        var workLog = new WorkLog
        {
            UserNif = user.Nif,
            Date = datePicker.Date + timePicker.Time,
            IsEntry = isEntry.IsToggled
        };

        await Database.InsertWorkLogAsync(workLog);
        user.LastIsEntry = workLog.IsEntry;
        await Database.UpdateUserAsync(user);

        AddLogToUI(workLog);
        await DisplayAlert(LanguageResource.Success, "Fecha y hora registrado correctamente.", LanguageResource.Confirm;
        await Navigation.PopAsync();
    }

    private void AddLogToUI(WorkLog workLog)
    {
        var label = new Label
        {
            Text = workLog.Date.ToString("dd-MM-yyyy HH:mm"),
            HorizontalOptions = LayoutOptions.Center
        };
        (workLog.IsEntry ? entry : exit).Children.Add(label);
    }

    private async void LoadWorkLogs()
    {
        entry.Children.Clear();
        exit.Children.Clear();

        Label entryLabel = new()
        {
            Text = LanguageResource.Entry,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
        entry.Children.Add(entryLabel);

        Label exitLabel = new()
        {
            Text = LanguageResource.Exit,
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
                    "ChangePassword",
                    "Enter New Password",
                    LanguageResource.Confirm, "Cancel",
                    "New Password", 18, Keyboard.Default);

        if (!user.ChangePassword(newPassword)) return;
        else await DisplayAlert(LanguageResource.Success, "Password Changed Successfully", LanguageResource.Confirm);

        await Database.UpdateUserAsync(user);
    }
}
