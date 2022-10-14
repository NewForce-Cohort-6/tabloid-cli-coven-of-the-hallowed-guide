using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabloidCLI.Models
{
    public class Journal
    {
        public int Id {get; set;}

        public string Title {get; set;}

        public string Content {get; set;}   

        public DateTime CreationDate {get; set;}  

        public string JournalEntries
        {
            get
            {
                return $"{Title} {Content} {CreationDate}";
            }

        }
        public override string ToString()
        {
            return JournalEntries;
        }
    }
}
