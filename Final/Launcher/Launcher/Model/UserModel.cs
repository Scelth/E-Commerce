using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Launcher.Model
{
    public class UserModel
    {
        public string Email { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        [JsonIgnore]
        public string ConfirmPassword { set; get; }
        public DateTime BirthDate { set; get; } = DateTime.Now;
    }
}
