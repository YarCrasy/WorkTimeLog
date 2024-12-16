using System.ComponentModel.DataAnnotations;

namespace WorkTimeLog
{
    public class User
    {
        [Key]
        public string Nif { get; set; }
        public string NameSurname { get; set; }
        public string Password { get; set; }
    }

    public class WorkLog
    {
        [Key]
        public string Id { get; set; }
        public string UserNif { get; set; }
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
    }

}
