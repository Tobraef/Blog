using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class EntryTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId
        {
            get;
            set;
        }

        [Required]
        [MaxLength(15)]
        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<BlogEntry> Entries
        {
            get;
            set;
        }

        public virtual ICollection<TagToUser> TagToUsers
        {
            get;
            set;
        }
    }
}