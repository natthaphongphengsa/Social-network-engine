using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDInlämningsuppgift.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public User PostedBy { get; set; }
    }
}
