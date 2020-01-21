using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class EntryPreview
    {
        [Required]
        public int EntryId
        {
            get;
            set;
        }

        [Required]
        public string Title
        {
            get;
            set;
        }
    }
}