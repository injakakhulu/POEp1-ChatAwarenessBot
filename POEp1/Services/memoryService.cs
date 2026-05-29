using POEp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using POEp1.Models;

namespace POEp1.Services
{
    public class MemoryService
    {
        private UserProfile _user;

        public MemoryService(UserProfile user)
        {
            _user = user;
        }

        public void SaveName(string name)
        {
            _user.Name = name;
        }

        public string GetName()
        {
            return _user.Name;
        }

        public void SaveTopic(string topic)
        {
            _user.FavouriteTopic = topic;
        }

        public string GetTopic()
        {
            return _user.FavouriteTopic;
        }
    }
}