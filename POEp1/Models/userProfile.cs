using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace POEp1.Models
{
    public class UserProfile
    {
        public string Name { get; set; } = "Agent";
        public string FavouriteTopic { get; set; } = "";

        // State machine memory tracker for follow-up logic flow
        public string LastDiscussedTopic { get; set; } = "";
    }
}