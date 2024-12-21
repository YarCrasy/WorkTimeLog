using SQLite;

namespace WorkTimeLog
{
    public class User
    {
        [PrimaryKey]
        public string Nif { get; set; }
        public string NameSurname { get; set; }
        public string Password { get; set; }
        public bool LastIsEntry { get; set; }

        public bool IsAdmin()
        {
            return NameSurname == "Admin";
        }
    }

    public class WorkLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserNif { get; set; }
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
    }

}
