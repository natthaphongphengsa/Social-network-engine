using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDInlämningsuppgift.Model
{
    public class Chat
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public User SendTo { get; set; }
        public int SendFromId { get; set; }
    }
}
