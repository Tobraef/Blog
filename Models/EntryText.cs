using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EntryText
    {
        public string Heading
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public int Nest
        {
            get
            {
                return Heading.Take(Heading.IndexOf(' ')).Count(c => c == '.');
            }
        }
    }
}