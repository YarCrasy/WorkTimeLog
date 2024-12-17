using System.Diagnostics;

namespace WorkTimeLog
{
    public class AppDbContext : DbContext
    {
        private readonly string _databasePath;
        public DbSet<User> Users { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }

        public AppDbContext()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _databasePath = Path.Combine(folderPath, "worktimelog.sqlite");
            Debug.WriteLine(_databasePath);

        }

        internal string GetDatabasePath()
        {
            return _databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }
    }
}
