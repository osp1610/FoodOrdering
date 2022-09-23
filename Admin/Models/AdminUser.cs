namespace Admin.Models
{
    public class AdminUser
    {
        public AdminUser()
        {

        }
        public AdminUser(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
