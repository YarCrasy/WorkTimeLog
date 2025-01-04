using SQLite;

namespace WorkTimeLog
{
    public class AppDbContext
    {
        public static AppDbContext Database = new();

        private readonly SQLiteAsyncConnection _database;

        public AppDbContext()
        {
            Database = this;
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "worktimelog.db");

            // Verificar si la base de datos ya existe
            bool dbExists = File.Exists(dbPath);

            _database = new SQLiteAsyncConnection(dbPath);
            User admin = new()
            {
                Nif = "Admin",
                NameSurname = "Admin",
                Password = "123",
                LastIsEntry = false,
                CompanyData = new()
            };

            if (!dbExists)
            {
                // Crear las tablas si la base de datos no existe
                 _database.CreateTableAsync<User>();
                InsertUserAsync(admin);
                _database.CreateTableAsync<WorkLog>();
            }
            else if(!UserExist(admin).Result)
            {
                InsertUserAsync(admin);
            }
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
            return  _database.Table<User>().Where(u => u.Nif == nif).FirstOrDefaultAsync();
        }

        public Task<User> GetUserByNameAsync(string name)
        {
            return _database.Table<User>().Where(u => u.NameSurname == name).FirstOrDefaultAsync();
        }

        public async Task<Company> GetCompanyAsync()
        {
            User admin = await GetUserByNifAsync("Admin");
            return admin.CompanyData;
        }

        public Task<bool> UserExist(User user)
        {
            return  _database.Table<User>().Where(u => u.Nif == user.Nif || u.NameSurname == user.NameSurname).CountAsync().ContinueWith(t => t.Result > 0);
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
