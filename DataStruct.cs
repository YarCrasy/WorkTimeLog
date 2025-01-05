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

    public struct Company
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string NIF { get; set; }

        public readonly bool IsEmpty()
        {
            return string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(NIF);
        }
    }
}
