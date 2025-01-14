using SQLite;

namespace WorkTimeLog
{
    public class User
    {
        [PrimaryKey]
        public string Nif { get; set; } = "";
        public string NameSurname { get; set; } = "";
        public string Password { get; set; } = "";
        public bool LastIsEntry { get; set; }
        public bool IsDeleting { get; set; } = false;

        public DateTime DeleteDate { get; set; }

        public bool IsAdmin()
        {
            return NameSurname == "Admin";
        }

        public bool ChangePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword)) return false;
            else
            {
                Password = newPassword;
                return true;
            }
        }
    }

    public class WorkLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserNif { get; set; } = "";
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
    }

    internal class Company
    {
        public string nif { get; set; } = "";
        public string name { get; set; } = "";

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(name) || string.IsNullOrEmpty(nif);
        }
    }
}
