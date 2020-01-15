using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class TagToUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagToUserId
        {
            get;
            set;
        }

        public int TimesSeen
        {
            get;
            set;
        }

        public virtual EntryTag Tag
        {
            get;
            set;
        }

        public virtual Account Account
        {
            get;
            set;
        }
    }
}