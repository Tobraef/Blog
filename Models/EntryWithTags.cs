using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class EntryWithTags
    {
        public BlogEntry Entry
        {
            get;
            set;
        }

        public List<EntryTag> AllTags
        {
            get;
            set;
        }

        public HttpPostedFileBase File
        {
            get;
            set;
        }
    }
}