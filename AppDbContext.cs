using SQLite;
using System.Text.Json;

namespace WorkTimeLog
{
    public class AppDbContext
    {
        public static AppDbContext Database = new();

        private readonly SQLiteAsyncConnection _database;

        private readonly User admin = new()
        {
            Nif = "Admin",
            NameSurname = "Admin",
            Password = "123",
            LastIsEntry = false
        };

        internal static Company companyData = new();
        private readonly string companyJsonPath;

        public AppDbContext()
        {
            Database = this;
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(localPath, "worktimelog.db");
            companyJsonPath = Path.Combine(localPath, "company.json");

            _database = new SQLiteAsyncConnection(dbPath);
            bool dbExists = _database != null && !IsDatabaseCorrupted();

            if (!dbExists) InitDatabase();

            InitCompany();
        }

        private bool IsDatabaseCorrupted()
        {
            bool result = false;
            try
            {
                var user = _database.Table<User>().FirstOrDefaultAsync().Result;
                var workLog = _database.Table<WorkLog>().FirstOrDefaultAsync().Result;
            }
            catch (Exception)
            {
                return true;
            }
            return result;
        }

        private void InitDatabase()
        {
            _database.CreateTableAsync<User>().Wait();
            InsertUserAsync(admin).Wait();
            _database.CreateTableAsync<WorkLog>().Wait();
        }

        //load company data if exists, or create new company
        private void InitCompany()
        {
            if (File.Exists(companyJsonPath))
            {
                LoadCompanyData();
            }
            else SaveCompanyData();
        }

        private void LoadCompanyData()
        {
            string json = File.ReadAllText(companyJsonPath);
            companyData = JsonSerializer.Deserialize<Company>(json);
        }

        public void SaveCompanyData()
        {
            string json = JsonSerializer.Serialize(companyData);
            File.WriteAllText(companyJsonPath, json);
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
            if (GetUserByNifAsync(user.Nif) != null) result = true;
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
}
