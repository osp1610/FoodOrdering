using System.ComponentModel.DataAnnotations;

namespace LoginPage.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Name { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public User()
        {

        }
        public User(string username, string password)
        {
            Name = username;
            Password = password;
        }

    }
}
