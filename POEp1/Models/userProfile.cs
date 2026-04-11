using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEp1.Models
{
    public class UserProfile
    {
        public string Name { get; set; }

        public UserProfile(string name)
        {
            Name = name;
        }
    }
}