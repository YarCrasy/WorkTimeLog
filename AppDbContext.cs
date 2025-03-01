using SQLite;
using System.Text.Json;

namespace WorkTimeLog;
public class AppDbContext
{
    internal static AppDbContext Database = new();

    private readonly SQLiteAsyncConnection _database;

    private readonly User admin = new()
    {
        Nif = "Admin",
        NameSurname = "Admin",
        Password = "123",
        LastIsEntry = false
    };

    internal Company employerData = new();
    private readonly string employerDataPath, dbPath;

    public AppDbContext()
    {
        string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WorkTimeLog\\";
        string localPath = Directory.CreateDirectory(dataPath).FullName;
        dbPath = Path.Combine(localPath, "worktimelog.db");
        employerDataPath = Path.Combine(localPath, "employerData.json");
        Database = this;
        try
        {
            _database = new SQLiteAsyncConnection(dbPath);
            if (_database.Table<User>().FirstOrDefaultAsync().IsFaulted||
                _database.Table<WorkLog>().FirstOrDefaultAsync().IsFaulted)
            InitDatabase();
        }
        catch (Exception)
        {
            _database?.CloseAsync().Wait();
            File.Delete(dbPath);
            File.Delete(employerDataPath);

            _database = new SQLiteAsyncConnection(dbPath);
            InitDatabase();
        }
    }

    private void InitDatabase()
    {
        _database.CreateTableAsync<User>().Wait();
        InsertUserAsync(admin).Wait();
        _database.CreateTableAsync<WorkLog>().Wait();

        //load Employer data if exists, or create new one
        if (File.Exists(employerDataPath)) LoadEmployerData();
        else SaveEmployerData();
    }

    private void LoadEmployerData()
    {
        string json = File.ReadAllText(employerDataPath);
        Company? aux = JsonSerializer.Deserialize<Company>(json);
        employerData = aux ?? employerData;
    }

    public void SaveEmployerData()
    {
        string json = JsonSerializer.Serialize(employerData);
        File.WriteAllText(employerDataPath, json);
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _database.Table<User>().ToListAsync();
    }

    public Task<int> InsertUserAsync(User user)
    {
        return _database.InsertAsync(user);
    }

    public Task<int> DropUserAsync(User user)
    {
        if (user.IsAdmin()) return Task.FromResult(0);
        return _database.DeleteAsync(user);
    }

    public Task<User> GetUserByNifAsync(string nif)
    {
        return _database.Table<User>().Where(u => u.Nif == nif).FirstOrDefaultAsync();
    }

    public Task<User> GetUserByNameAsync(string name)
    {
        return _database.Table<User>().Where(u => u.NameSurname == name).FirstOrDefaultAsync();
    }

    public Task<bool> UserExist(User user)
    {
        bool result = false;
        if (GetUserByNifAsync(user.Nif).Result != null) result = true;
        return Task.FromResult(result);
    }

    public Task<int> InsertWorkLogAsync(WorkLog workLog)
    {
        return _database.InsertAsync(workLog);
    }

    public Task<List<WorkLog>> GetWorkLogsAsync()
    {
        return _database.Table<WorkLog>().ToListAsync();
    }

    public Task<int> UpdateUserAsync(User user)
    {
        return _database.UpdateAsync(user);
    }

    public Task<List<WorkLog>> GetWorkLogsByUserNifAsync(string userNif)
    {
        return _database.Table<WorkLog>().Where(w => w.UserNif == userNif).ToListAsync();
    }
}
