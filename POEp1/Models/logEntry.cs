using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace POEp1.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Category { get; set; } // "Task", "Reminder", "Quiz", "NLP"
        public string Description { get; set; }

        public LogEntry(string category, string description)
        {
            Timestamp = DateTime.Now;
            Category = category;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] ({Category}) {Description}";
        }
    }
}
