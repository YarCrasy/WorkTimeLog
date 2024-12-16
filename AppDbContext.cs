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
            _databasePath = Path.Combine(folderPath, "worktimelog.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }
    }
}
