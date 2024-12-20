using SQLite;
using System.IO;

namespace WorkTimeLog
{
    public class AppDbContext
    {
        private readonly string databasePath;
        private readonly SQLiteConnection database;

        public AppDbContext()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            databasePath = Path.Combine(folderPath, "worktimelog.db");
            database = new SQLiteConnection(databasePath);
            database.CreateTable<User>();
            database.CreateTable<WorkLog>();
        }

        public TableQuery<User> Users => database.Table<User>();
        public TableQuery<WorkLog> WorkLogs => database.Table<WorkLog>();

        internal string GetDatabasePath()
        {
            return databasePath;
        }

        public void InsertUser(User user)
        {
            database.Insert(user);
        }

        public void InsertWorkLog(WorkLog workLog)
        {
            database.Insert(workLog);
        }

    }
}
