using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }

        public virtual Account Author
        {
            get;
            set;
        }

        public virtual BlogEntry Entry
        {
            get;
            set;
        }

        [Required]
        [MaxLength(250, ErrorMessage="Comment cannot be longer than 250 characters")]
        public string Text
        {
            get;
            set;
        }

        [Display(Name="Post date")]
        public DateTime Date
        {
            get;
            set;
        }
    }
}